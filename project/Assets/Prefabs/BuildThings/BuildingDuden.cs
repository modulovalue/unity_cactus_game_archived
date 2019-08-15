using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BuildingDuden : MonoBehaviour {

	public List<DudenEntry> duden;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class DudenEntry {
	public InventoryItemScript obj;
	public List<DudenPosition> positions;

	public DudenEntry(InventoryItemScript obj, List<DudenPosition> positions) {
		this.obj = obj;
		this.positions = positions;
	}

	public DudenEntry() {
		this.obj = null;
		this.positions = null;
	}
}

[System.Serializable]
public class DudenPosition {
	public Vector3 positionRelToPreviousObject;
	public Vector3 newRotation;

	public DudenPosition(Vector3 pos, Vector3 quat) {
		this.positionRelToPreviousObject = pos;
		this.newRotation = quat;
	}
}
