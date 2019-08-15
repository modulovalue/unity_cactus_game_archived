using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ChildrenTransformTool))]
public class ChindrenTransformToolEditor : Editor {

	public override void OnInspectorGUI() {

		if(GUILayout.Button("Set")) {
			ChildrenTransformTool script = (ChildrenTransformTool) target;
			script.set (script.transform, script.arr);
		}
		
		if(GUILayout.Button("Put")) {
			ChildrenTransformTool script = (ChildrenTransformTool) target;
			script.arr.Clear ();
			script.put (script.transform, script.arr);
		}

		DrawDefaultInspector();
	}

}

