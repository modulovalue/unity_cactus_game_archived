using UnityEngine;
using System.Collections.Generic;
using MidiJack;
using System.Reflection;
using System;

public class ChildrenTransformTool : MonoBehaviour {

	public List<TransformContainer> arr;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void set(Transform transform, List<TransformContainer> dict) {
		foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
			foreach (TransformContainer tcon in arr) {
				if (tcon.name == t.name) {
					t.localScale = tcon.localScale;
					t.localEulerAngles = tcon.localEulerAng;
					t.localPosition = tcon.localPos;
				}

			}
		}
	}
		
	public void put(Transform transform, List<TransformContainer> dict) {

		foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
			TransformContainer con = new TransformContainer (t.name, t.localPosition, t.localEulerAngles, t.localScale);
			dict.Add (con);
			//put (t, dict);
		}

	}
}
