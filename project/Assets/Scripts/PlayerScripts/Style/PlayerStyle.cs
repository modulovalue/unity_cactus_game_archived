using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class PlayerStyle : MonoBehaviour {

	public string name;
	public string description;

	public GameObject headAccessory;
	public Vector3 headAccPos;
	public Vector3 headAccEuler;
	public Vector3 headAccScale;

	public GameObject faceAccessory;
	public Vector3 faceAccPos;
	public Vector3 faceAccEuler;
	public Vector3 faceAccScale;

	public float movementSpeed = 13f;
	public float rotateSpeed = 150f;

	public CameraStyle cameraStyle = CameraStyle.None;
}

