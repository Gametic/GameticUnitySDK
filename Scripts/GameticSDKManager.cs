using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Gametic
{
	[RequireComponent(typeof(GameticRequestManager))]
	public class GameticSDKManager : MonoBehaviour
	{

		public string developerID;
		public string projectName;

		[HideInInspector]
		public static GameticSDKManager instance;
		private GameticRequestManager requestManager;
		private string userId;
		private static bool sessionSentOnce;

		public int activationRequiredMinutes = 20;

		private void Awake(){
			instance = this;
			requestManager = GetComponent<GameticRequestManager>();
			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			if (developerID == ""){
				Debug.LogError("Gametic: developerID should has correct value");
				return;
			}
			if (projectName == ""){
				Debug.LogError("Gametic: projectName should has correct value");
				return;
			}

			#if !UNITY_EDITOR || GAMETIC_DEBUG
			if (!sessionSentOnce){
				if (!PlayerPrefs.HasKey("GameticUserId")){
					NewUser();
				}else{
					userId = PlayerPrefs.GetString("GameticUserId");
					NewSession();
				}
				sessionSentOnce = true;
			}

			SceneManager.activeSceneChanged += (Scene arg0, Scene arg1) => {
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters["FromScene"] = arg0.name;
				parameters["ToScene"] = arg1.name;
				SendEventReq("#SceneChanged", parameters, true);
			};

			SendSegment("#Version", Application.version);

			CheckActivationSegment();
			#endif
		}
			
// 		When SceneManager.activeSceneChanged is unavailable
//		public void OnLevelWasLoaded(){
//			SendEventReq("#SceneChanged", new Dictionary<string, object>(), true);
//		}

		#region defaults
		private void NewUser(){
			JSONObject json = new JSONObject();
			json.AddField("developer_id", developerID);
			json.AddField("project_name", projectName);
			requestManager.Post("analytic/signup", json, true, NewUserResponse);
		}

		private void NewUserResponse(JSONObject responeJson){
			userId = responeJson.GetField("app_user_id").str;
			PlayerPrefs.SetString("GameticUserId", userId);
			NewSession();
		}

		private void NewSession(){
			SendEventReq("#NewSession", new Dictionary<string, object>(), true);
		}

		private void CheckActivationSegment(){
			if (!PlayerPrefs.HasKey ("GameticActivationSegment")) {
				StartCoroutine (TrackActivationSegment());
			}
		}
			
		private IEnumerator TrackActivationSegment(){
			yield return new WaitForSeconds (60*activationRequiredMinutes);
			SendSegment ("#Activation", "Activated", true);
			PlayerPrefs.SetInt ("GameticActivationSegment", 1);
		}
		#endregion

		#region send event and segment
		public void SendEvent(string eventName,
			Dictionary<string, object> parameters,
			bool tryAgainInCaseOfError = false){
			SendEventReq (eventName, parameters, false, tryAgainInCaseOfError);
		}

		private void SendEventReq(string eventName,
		                      Dictionary<string, object> parameters,
			                  bool isDefaultEvent = false,
		                      bool tryAgainInCaseOfError = false){

			try{
				JSONObject json = new JSONObject();
				json.AddField("developer_id", developerID);
				json.AddField("project_name", projectName);
				json.AddField("id", userId);
				JSONObject eventJson = new JSONObject();
				eventJson.AddField("name", eventName);
				JSONObject eventParametersJson = new JSONObject(JSONObject.Type.ARRAY);
				foreach (var element in parameters) {
					if (element.Value is string) {
						JSONObject parameter = new JSONObject();
						parameter.AddField("name", element.Key);
						parameter.AddField("type", "string");
						parameter.AddField("value", element.Value.ToString());
						eventParametersJson.Add(parameter);
					} else if ((element.Value is int) || (element.Value is long) || (element.Value is double) || (element.Value is float)) {
						JSONObject parameter = new JSONObject();
						parameter.AddField("name", element.Key);
						parameter.AddField("type", "number");
						parameter.AddField("value", element.Value.ToString());
						eventParametersJson.Add(parameter);
					} else if (element.Value is bool){
						JSONObject parameter = new JSONObject();
						parameter.AddField("name", element.Key);
						parameter.AddField("type", "number");
						parameter.AddField("value", (bool)element.Value? "1":"0");
						eventParametersJson.Add(parameter);
					}else {
						Debug.LogError("Gametic: parameter '"+element.Key+"' has invalid type");
					}
				}
				eventJson.AddField("parameters", eventParametersJson);
				json.AddField("event", eventJson);
				if (isDefaultEvent){
					requestManager.Post("analytic/default_events", json, tryAgainInCaseOfError, null);
				}else{
					requestManager.Post("analytic/events", json, tryAgainInCaseOfError, null);
				}
			#if GAMETIC_DEBUG
			}catch (System.Exception e) {
					Debug.LogError (e.Message);
			}
			#else
			}catch{}
			#endif
		}

		public void SendSegment(string segmentName, string segmentValue, bool tryAgainInCaseOfError = false){
			try{
				JSONObject json = new JSONObject();
				json.AddField("developer_id", developerID);
				json.AddField("project_name", projectName);
				json.AddField("id", userId);
				JSONObject segmentJson = new JSONObject();
				segmentJson.AddField("name", segmentName);
				segmentJson.AddField("propertyValue", segmentValue);
				json.AddField("segment", segmentJson);
				requestManager.Post("analytic/segments", json, tryAgainInCaseOfError, null);
			#if GAMETIC_DEBUG
			}catch (System.Exception e) {
				Debug.LogError (e.Message);
			}
			#else
			}catch{}
			#endif
		}
			
		public void SendPurchase(string market, int value, bool tryAgainInCaseOfError = true){
			try{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters["Market"] = market;
				parameters["Count"] = value;

				parameters["Day"] = System.DateTime.Now.ToString("dd");
				parameters["Hour"] = System.DateTime.Now.ToString("HH");

				SendEventReq("#Purchase", parameters, true, tryAgainInCaseOfError);
				SendSegment("#Purchase", "Purchased", true);
			#if GAMETIC_DEBUG
			}catch (System.Exception e) {
				Debug.LogError (e.Message);
			}
			#else
			}catch{}
			#endif
		}
		#endregion

	}
}
