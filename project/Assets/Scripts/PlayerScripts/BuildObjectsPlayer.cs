using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class BuildObjectsPlayer : MonoBehaviour {

	private GameObject lastObj = null;

	private bool isBuilding = false;

	private RaycastHit myhit = new RaycastHit();
	private Ray myray;
	public LayerMask builderClickLayerMask;

	public InventoryItemScript objectToBuild;

	public GameObject parentObject;

	public Camera mainCamera;

	void Start () {
		myray = new Ray ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		// UI Layer and BuildFrameClickThing Layer
		builderClickLayerMask = 2080;
		parentObject = GameObject.Find("BuiltObjects");
		if (parentObject == null) {
			parentObject = new GameObject ();
			parentObject.name = "BuiltObjects";
		}
	}

	// Update is called once per frame
	void Update () {
		
		myray = mainCamera.ScreenPointToRay (Input.mousePosition);

		//cast raycast, get mousebutton, check if not ui but on joystick
		if (Physics.Raycast (myray, out myhit, 1000.0f, builderClickLayerMask) && 
			Input.GetMouseButtonDown(0) &&
			(!EventSystem.current.IsPointerOverGameObject()  || EventSystem.current.currentSelectedGameObject == null))
		{
			BuilderController objBuilderController;
			if (myhit.transform.parent != null) {
				if (myhit.transform.parent.GetComponentInChildren<BuilderController> () != null) {
					objBuilderController = myhit.transform.parent.GetComponentInChildren<BuilderController> ();
					if (objBuilderController != null) {
						myhit.transform.parent.parent = parentObject.transform;
						objBuilderController.showObject ();
					}
				}
			}
		}

	}
		
	public void activateBuildMode() {
		isBuilding = true;
		if (lastObj != null) {
			lastObj.transform.Find ("Controller").GetComponent<BuilderController> ().activateBuildingMode (objectToBuild);
		}
	}

	public void deactivateBuildMode() {
		isBuilding = false;
		if (lastObj != null) {
			lastObj.transform.Find ("Controller").GetComponent<BuilderController> ().deactivateBuildingMode ();
			lastObj = null;
		}

	}

	public GameObject getLastObj() {
		return lastObj;
	}

	public void setLastObj(GameObject obj) {
		if (lastObj != obj && isBuilding) {
			obj.transform.Find ("Controller").GetComponent<BuilderController> ().activateBuildingMode (objectToBuild);
			if (lastObj != null) {
				lastObj.transform.Find ("Controller").GetComponent<BuilderController> ().deactivateBuildingMode ();
			} 
		}
		lastObj = obj;
	}

	public void OnCollisionEnter(Collision other) {
		if(other.transform.parent != null ) {
			if(other.transform.parent.parent != null ) {
				if (other.transform.parent.parent.gameObject.tag == "BuiltThing") {
					if (lastObj != other.transform.parent.parent.gameObject) {
						setLastObj(other.transform.parent.parent.gameObject);
					}
				}
			}
		}
	}

}
