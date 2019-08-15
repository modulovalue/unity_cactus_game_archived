using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerStyleController))]
public class PlayerStyleControllerEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		PlayerStyleController styleController = (PlayerStyleController) target;
	
		foreach (GameObject obj in styleController.styles) {
			
			PlayerStyle style = obj.GetComponent<PlayerStyle> ();

			if (GUILayout.Button (style.name)) {
				styleController.setStyle (styleController.styles.IndexOf (obj));
			}

		} 

	}

}