using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Gametic
{
	public class GameticRequestManager : MonoBehaviour
	{
		[HideInInspector]
		public string baseUrl = "http://api.gametic.ir";
		[HideInInspector]
		public int port = 4223;
		public void Post(string uri, JSONObject json, bool tryAgainInCaseOfError, System.Action<JSONObject> callback = null)
		{
			string url = baseUrl + ":" + port + "/" + uri;

			var encoding = new System.Text.UTF8Encoding();
	        var postHeader = new Dictionary<string, string>();
	        postHeader.Add("Content-Type", "application/json");

			#if GAMETIC_DEBUG
			Debug.Log("Gametic Debug: try to post " + url + " | " + json);
			#endif
			StartCoroutine(SendingRequest(url, encoding.GetBytes(json.ToString()), postHeader, callback, tryAgainInCaseOfError));
		}

		IEnumerator SendingRequest(string url, byte[] postData, Dictionary<string, string> postHeader , System.Action<JSONObject> callback, bool tryAgain)
		{
			WWW request = new WWW(url, postData, postHeader);
			do {
				yield return request;
				if (string.IsNullOrEmpty(request.error))
				{
					string msg = "";
					if (!string.IsNullOrEmpty(request.text))
					{
						msg = request.text;
						msg = System.Text.RegularExpressions.Regex.Unescape(msg);
						JSONObject json = new JSONObject(msg);
						if (callback == null){
//							#if GAMETIC_DEBUG
//							Debug.LogWarning("GameticSDK: callback is not defined");
//							#endif
							yield break;
						}
						else if (json != null && json.ToString() != "null")
						{
							if (json.HasField("content"))
								callback(json.GetField("content"));
							else
								callback(json);
							yield break;
						}
						else
						{
							#if GAMETIC_DEBUG
							Debug.LogWarning("GameticSDK: Error in parse server message");
							#endif
							yield break;
						}
					}
					yield break;
				}
				else
				{
					#if GAMETIC_DEBUG
					Debug.LogWarning("GameticSDK: Error in connect to server, let's try again .. | " + request.error + " | " + request.url);
					#endif
					yield return new WaitForSeconds(5);
				}
				if (tryAgain)
					request = new WWW(url, postData, postHeader);
			} while (tryAgain);
		}
	}
}
