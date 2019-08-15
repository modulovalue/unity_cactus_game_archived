using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimplePortalScript))]
public class SimplePortalScriptEditor : Editor {

	void OnInspectorGUI() {
		SimplePortalScript script = (SimplePortalScript) target;
		DrawDefaultInspector ();
	}
}
