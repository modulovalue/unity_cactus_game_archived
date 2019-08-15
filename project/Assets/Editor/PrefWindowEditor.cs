using UnityEditor;
using UnityEngine;
using UnityEditor.UI;
using System.Collections.Generic;

public class PrefWindowEditor : EditorWindow {

	private List<bool> myFoldOuts = new List<bool>();

	public static void ShowWindow() {
		//EditorWindow.GetWindow(typeof(PrefWindowEditor));
	}

	public bool showPosition = true;
	public string status = "Select a GameObject";

	void OnGUI() {


		EditorGUILayout.Toggle ("Prefs Initialized", PlayerPrefs.GetInt ("prefsInitialized", 0) > 0);

		if (GUILayout.Button ("Change Inizialization Status")) {
			if (PlayerPrefs.GetInt ("prefsInitialized", 0) > 0) {
				PlayerPrefs.SetInt ("prefsInitialized", 0);
			} else {
				PlayerPrefs.SetInt ("prefsInitialized", 1);
			}
		}

		if (myFoldOuts.Count < 1) {
			for (int i = 1; i < 21; i++) {
				myFoldOuts.Add(PlayerPrefs.GetInt ("Level" + i + "Unlocked", 0) > 0);
			}
			Debug.Log ("ok");
		}

		GUILayout.Label ("Prefs", EditorStyles.boldLabel);
		if (GUILayout.Button ("Save Prefs")) {
			
		}

		for (int i = 1; i < 21; i++) {
			myFoldOuts[i-1] = EditorGUILayout.Foldout(myFoldOuts[i-1], "Level " + i);
			if (myFoldOuts[i-1]) {
				EditorGUI.indentLevel++;

				EditorGUILayout.Toggle ("Level " + i + " Unlocked", PlayerPrefs.GetInt ("Level" + i + "Unlocked", 0) > 0);
				if (GUILayout.Button ("Change Status")) {
					if (PlayerPrefs.GetInt ("Level" + i + "Unlocked", 0) > 0) {
						PlayerPrefs.SetInt ("Level" + i + "Unlocked", 0);
					} else {
						PlayerPrefs.SetInt ("Level" + i + "Unlocked", 1);
					}
				}
				EditorGUI.indentLevel--;
			}
		}

		if (GUILayout.Button ("Save Prefs")) {
			
		}

	}

	public void OnInspectorUpdate() {
		this.Repaint();
	}
}