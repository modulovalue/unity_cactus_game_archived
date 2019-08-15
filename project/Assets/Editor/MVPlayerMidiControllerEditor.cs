using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using MidiJack;
using System.Reflection;
using System;

[CustomEditor(typeof(MVPlayerMidiController))]
public class MVPlayerMidiControllerEditor : Editor {

	public const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

	public override void OnInspectorGUI() {
		//Debug.Log ("Was");
		DrawDefaultInspector ();
		MVPlayerMidiController script = (MVPlayerMidiController) target;


		if (GUILayout.Button ("Focus on Anim")) {
			focusOnAnimation ();
		}
			
		foreach (ListOfKnobSettingsContainer con0 in script.knobSettingList) {
			foreach (KnobSettingsContainer con in con0.list) {
				con.set ();
			}
		}
	

		try {
			MidiMessage mmm = MidiDriver.Instance.History.ToArray()[MidiDriver.Instance.History.ToArray().Length-1];
			if (mmm.data1 == 15 && mmm.data2 == 127) {
				focusOnAnimation ();
			}
		} catch (IndexOutOfRangeException e) { }
			

		if (UnityEditor.EditorWindow.focusedWindow != null) {

			EditorWindow window = UnityEditor.EditorWindow.focusedWindow;

			if (window.GetType ().Name == "AnimationWindow") {

				object obj = window.GetType ().GetField ("m_AnimEditor", flags).GetValue (window);

				Debug.Log ("focudsed");

				if (MidiMaster.GetKey (0) != 0) {
					invoke (obj, "MoveToPreviousKeyframe");
					invoke (obj, "Repaint");
				}

				if (MidiMaster.GetKey (1) != 0) {
					invoke (obj, "MoveToNextKeyframe");
					invoke (obj, "Repaint");
				}

				if (MidiMaster.GetKey (36) != 0) {
					invoke (obj, "MoveToPreviousKeyframe");
					invoke (obj, "Repaint");
				}

				if (MidiMaster.GetKey (37) != 0) {
					invoke (obj, "MoveToNextKeyframe");
					invoke (obj, "Repaint");
				}

				Debug.Log ((MidiMaster.GetKnob (0, 0f) - 0.5f) * 40 * 2f);

				try {
					MidiMessage mm = MidiDriver.Instance.History.ToArray () [MidiDriver.Instance.History.ToArray ().Length - 1];

					if (mm.data1 == 14 && mm.data2 == 127) {   //Fastforward

						//invoke (obj, "MoveToNextKeyframe");
						//invoke (obj, "Repaint");

						object obj2 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
						float currentTime = (float)obj2.GetType ().GetProperty ("currentTime", flags).GetValue (obj2, null);
						float maxTime = (float)obj2.GetType ().GetProperty ("maxTime", flags).GetValue (obj2, null);
						obj2.GetType ().GetProperty ("currentTime", flags).SetValue (obj2, (currentTime + 0.05f < maxTime) ? currentTime + 0.05f : maxTime, null);
						invoke (obj, "PlaybackUpdate");
						invoke (obj, "Repaint");


					} else if (mm.data1 == 13 && mm.data2 == 127) {   //Rewind

						object obj2 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
						float currentTime = (float)obj2.GetType ().GetProperty ("currentTime", flags).GetValue (obj2, null);
						float maxTime = (float)obj2.GetType ().GetProperty ("maxTime", flags).GetValue (obj2, null);
						obj2.GetType ().GetProperty ("currentTime", flags).SetValue (obj2, (currentTime - 0.05f > 0) ? currentTime - 0.05f : 0, null);
						invoke (obj, "PlaybackUpdate");
						invoke (obj, "Repaint");


					} else if (mm.data1 == 10 && mm.data2 == 127) {
						//Play
						object obj1 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("playing", flags).SetValue (obj1, true, null);
						invoke (obj, "PlaybackUpdate");
						invoke (obj, "Repaint");

					} else if (mm.data1 == 11 && mm.data2 == 127) {	
						//Stop

						// PlaybackUpdate set time to max time and call playback update
						object obj1 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("playing", flags).SetValue (obj1, false, null);
						invoke (obj, "MoveToFirstKeyframe");
						invoke (obj, "Repaint");


					} else if (mm.data1 == 12 && mm.data2 == 127) {
						object obj1 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("recording", flags).SetValue (obj1, true, null);
						invoke (obj, "Repaint");

					} else if (mm.data1 == 12 && mm.data2 == 0) {
						object obj1 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("recording", flags).SetValue (obj1, false, null);
						invoke (obj, "Repaint");
						//obj.GetType().GetField("m_PreviousUpdateTime").SetValue(obj, Time.
					}

					object obj22 = obj.GetType ().GetField ("m_State", flags).GetValue (obj);
					float currentTime2 = (float)obj22.GetType ().GetProperty ("currentTime", flags).GetValue (obj22, null);


					Debug.Log (" data1 " + mm.data1 + " data2 " + mm.data2 + " status " + mm.status + "  " + mm.ToString ());

				} catch (IndexOutOfRangeException e) {

				}

			}
		}

		EditorUtility.SetDirty(target);
	}

	public void invoke(object obj, String method) {
		obj.GetType ().GetMethod (method, flags).Invoke (obj, null);
	}

	public void focusOnAnimation() {
		UnityEditor.EditorWindow.GetWindow (Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.AnimationWindow"));
	}


}
