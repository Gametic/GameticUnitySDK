using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameticSDK
{
	public class GameticEditor {

		[MenuItem("GameObject/Gametic/Add SDK Manager", false, 10)]
		static void InitManager() {
			GameObject manager = new GameObject("GameticSDKManager");
			manager.AddComponent<GameticSDKManager> ();
		}

	}
}