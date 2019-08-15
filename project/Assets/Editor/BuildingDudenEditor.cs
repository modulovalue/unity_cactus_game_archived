using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BuildingDuden))]
public class BuildingDudenEditor : Editor {

	public override void OnInspectorGUI() {
		
		DrawDefaultInspector ();

		BuildingDuden myScript = (BuildingDuden) target;
		if (GUILayout.Button ("Add Object")) {
			myScript.duden.Add (new DudenEntry ());
		}

	}

}