using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using MidiJack;
using System.Reflection;
using System;


public class MVMidiController : EditorWindow {
	
	public Transform transform;
	public Animator animator;
	public Vector3 origEulerAngles;


	public List<KnobSettingsContainer> knobSettingList = new List<KnobSettingsContainer>();


	[MenuItem("Window/MVMidiController")]
	public static void ShowWindow() {
		EditorWindow.GetWindow<MVMidiController>("MVMidiController");
	}

	public const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

	public void invoke(object obj, String method) {
		obj.GetType ().GetMethod (method, flags).Invoke (obj, null);
	}

	public void focusOnAnimation() {
		UnityEditor.EditorWindow.GetWindow (Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.AnimationWindow"));
	}

	void OnGUI() {


		SerializedObject so = new SerializedObject(this);
		SerializedProperty stringsProperty = so.FindProperty("knobSettingList");
		EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
		so.ApplyModifiedProperties(); // Remember to apply modified properties


		foreach (KnobSettingsContainer con in knobSettingList) {
			con.set ();
		}


		transform = EditorGUILayout.ObjectField("root", transform, typeof(Transform), true) as Transform;
		animator = EditorGUILayout.ObjectField("root", animator, typeof(Animator), true) as Animator;

		GUILayout.Label ("MVMidiController", EditorStyles.boldLabel);



		if (GUILayout.Button ("Set Orig")) {
			focusOnAnimation ();
		}

		try {
			MidiMessage mmm = MidiDriver.Instance.History.ToArray()[MidiDriver.Instance.History.ToArray().Length-1];
			if (mmm.data1 == 15 && mmm.data2 == 127) {
				focusOnAnimation ();
			}
		} catch (IndexOutOfRangeException e) {

		}


		if (UnityEditor.EditorWindow.focusedWindow != null) {
			
			EditorWindow window = UnityEditor.EditorWindow.focusedWindow;

			if (window.GetType().Name == "AnimationWindow") {
				
				object obj = window.GetType().GetField ("m_AnimEditor", flags).GetValue (window);

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
					MidiMessage mm = MidiDriver.Instance.History.ToArray()[MidiDriver.Instance.History.ToArray().Length-1];

					if (mm.data1 == 14 && mm.data2 == 127) 
					{   //Fastforward
						
						//invoke (obj, "MoveToNextKeyframe");
						//invoke (obj, "Repaint");

						object obj2 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
						float currentTime = (float) obj2.GetType ().GetProperty ("currentTime", flags).GetValue(obj2, null);
						float maxTime = (float) obj2.GetType ().GetProperty ("maxTime", flags).GetValue(obj2, null);
						obj2.GetType ().GetProperty ("currentTime", flags).SetValue(obj2, (currentTime + 0.05f < maxTime) ? currentTime + 0.05f : maxTime, null);
						invoke (obj, "PlaybackUpdate");
						invoke (obj, "Repaint");


					} else if (mm.data1 == 13 && mm.data2 == 127) 
					{   //Rewind

						object obj2 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
						float currentTime = (float) obj2.GetType ().GetProperty ("currentTime", flags).GetValue(obj2, null);
						float maxTime = (float) obj2.GetType ().GetProperty ("maxTime", flags).GetValue(obj2, null);
						obj2.GetType ().GetProperty ("currentTime", flags).SetValue(obj2, (currentTime - 0.05f > 0) ? currentTime - 0.05f : 0, null);
						invoke (obj, "PlaybackUpdate");
						invoke (obj, "Repaint");


					} else if ( mm.data1 == 10 && mm.data2 == 127) 
					{
						//Play
						object obj1 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("playing", flags).SetValue (obj1, true, null);
						invoke (obj, "PlaybackUpdate");
						invoke (obj, "Repaint");
						
					} else if (mm.data1 == 11 && mm.data2 == 127) 
					{	
						//Stop

						// PlaybackUpdate set time to max time and call playback update
						object obj1 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("playing", flags).SetValue (obj1, false, null);
						invoke(obj, "MoveToFirstKeyframe");
						invoke (obj, "Repaint");
				

					} else if (mm.data1 == 12 && mm.data2 == 127) 
					{
						object obj1 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("recording", flags).SetValue (obj1, true, null);
						invoke (obj, "Repaint");
						
					} else if (mm.data1 == 12 && mm.data2 == 0) 
					{
						object obj1 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
						obj1.GetType ().GetProperty ("recording", flags).SetValue (obj1, false, null);
						invoke (obj, "Repaint");
						//obj.GetType().GetField("m_PreviousUpdateTime").SetValue(obj, Time.
					}

					object obj22 = obj.GetType().GetField ("m_State", flags).GetValue (obj);
					float currentTime2 = (float) obj22.GetType ().GetProperty ("currentTime", flags).GetValue(obj22, null);


					Debug.Log (" data1 " + mm.data1 + " data2 " + mm.data2 + " status " + mm.status + "  " + mm.ToString());

				} catch (IndexOutOfRangeException e) {
					  
				}

			}

		}
			

		////
		////
		////if (transform != null) {
		////	origEulerAngles = new Vector3 (((MidiMaster.GetKnob (0, 0f) - 0.5f) * 90),  ((MidiMaster.GetKnob (1, 0f) - 0.5f) * 90), ((MidiMaster.GetKnob (2, 0f) - 0.5f) * 90));
		////	Vector3 newEulerAngles = origEulerAngles;
		////	transform.localEulerAngles = newEulerAngles;
		////}
		//Debug.Log ((MidiMaster.GetKnob (0, 0f) - 0.5f) * 180f);

	}

	public void OnInspectorUpdate() {
		this.Repaint();
	}
}