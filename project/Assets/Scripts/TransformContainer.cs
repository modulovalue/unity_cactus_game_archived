using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TransformContainer {
	
	public Vector3 localPos;
	public Vector3 localEulerAng;
	public Vector3 localScale;
	public String name;

	public TransformContainer(String name, Vector3 locpos, Vector3 locAng, Vector3 locScale) {
		this.name = name;
		this.localPos = locpos;
		this.localEulerAng = locAng;
		this.localScale = locScale;
	}
}