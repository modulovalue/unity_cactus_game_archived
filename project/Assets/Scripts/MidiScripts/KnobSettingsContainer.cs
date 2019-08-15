using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MidiJack;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class KnobSettingsContainer {

	public string description;

	public float min;
	public float max;
	public int knobNumber;
	public Transform transform;
	public xoryorz xyz;
	public float knobValue = 0;
	public bool invert = false;

	public void set() {
		if (transform != null) {
			float value = Mathf.Abs (max - min);

			float tempKnobValue = ((MidiMaster.GetKnob (knobNumber, 0f) * Mathf.Abs(min - max) ) + min)  * ((invert) ? -1 : 1) ;

			if (tempKnobValue != knobValue) {
				#if UNITY_EDITOR
				Selection.activeGameObject = transform.gameObject;
				#endif
				Debug.Log ("set " + transform.name + " " + tempKnobValue + " " + knobValue);
				knobValue = tempKnobValue;

				Vector3 tmp = transform.localEulerAngles;
				switch (xyz) {
				case xoryorz.X:
					tmp.x = knobValue;
					break;
				case xoryorz.Y:
					tmp.y = knobValue;
					break;
				case xoryorz.Z:
					tmp.z = knobValue;
					break;
				default:
					break;
				}

				transform.localEulerAngles = tmp;
			}
		}
	}
}