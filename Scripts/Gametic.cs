using System.Collections.Generic;

namespace Gametic
{
	public static class Gametic {

		public static void SendEvent(string eventName, Dictionary<string, object> parameters, bool tryAgainInCaseOfError = false){
			GameticSDKManager.instance.SendEvent (eventName, parameters, tryAgainInCaseOfError);
		}

		public static void SendSegment(string segmentName, string segmentValue, bool tryAgainInCaseOfError = false){
			GameticSDKManager.instance.SendSegment (segmentName, segmentValue, tryAgainInCaseOfError);
		}

		public static void SendPurchaseEvent(string market, int value, bool tryAgainInCaseOfError = true){
			GameticSDKManager.instance.SendPurchase (market, value, tryAgainInCaseOfError);
		}

	}
}