using System.Collections.Generic;

namespace GameticSDK
{
	public static class Gametic {

		public static void CustomEvent(string eventName, Dictionary<string, object> parameters, bool tryAgainInCaseOfError = false){
			GameticSDKManager.instance.SendEvent (eventName, parameters, tryAgainInCaseOfError);
		}

		public static void CustomSegment(string segmentName, string segmentValue, bool tryAgainInCaseOfError = false){
			GameticSDKManager.instance.SendSegment (segmentName, segmentValue, tryAgainInCaseOfError);
		}

		public static void Purchase(string market, int value, bool tryAgainInCaseOfError = true){
			GameticSDKManager.instance.SendPurchase (market, value, tryAgainInCaseOfError);
		}

	}
}