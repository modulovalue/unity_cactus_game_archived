using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		PlayerController myScript = (PlayerController) target;
		if (GUILayout.Button ("Change ")) {
			Debug.Log ("kodkeo");
		}
	}

}