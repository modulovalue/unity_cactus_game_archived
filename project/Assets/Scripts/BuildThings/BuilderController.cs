using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MonoBehaviour {

	public GameObject builderFrame;
	public GameObject builderObject;

	bool isBuilding = false;

	GameObject objtemp;

	void Awake() {
		builderFrame.SetActive (false);
		builderObject.SetActive (true);
	}

	public void activateBuildingMode(InventoryItemScript inventoryItemScriptGiven) {

		if (!isBuilding) {
			objtemp = new GameObject();
			objtemp.name = GetInstanceID () + "" ;
			objtemp.transform.SetParent (transform.parent.transform);
			objtemp.transform.localPosition = Vector3.zero;
			objtemp.transform.localRotation = new Quaternion ();

			showObject ();
			isBuilding = true;
		
			foreach (DudenEntry f in transform.GetComponent<BuildingDuden>().duden) { 
				if (inventoryItemScriptGiven == f.obj) {
					foreach (DudenPosition pos in f.positions) {
						GameObject obj1 = Instantiate (f.obj.item);
						obj1.transform.parent = objtemp.transform;
						obj1.transform.localPosition = pos.positionRelToPreviousObject;
						obj1.transform.localRotation = Quaternion.Euler(pos.newRotation);
						obj1.transform.GetComponentInChildren<BuilderController> ().showFrame ();
					}
				}
			}
		}

	}

	public void deactivateBuildingMode() {
		if (isBuilding) {
			Destroy (objtemp);
			isBuilding = false;
		}
	}

	public void showFrame() {
		builderFrame.SetActive (true);
		builderObject.SetActive (false);
	}

	public void showObject() {
		builderFrame.SetActive (false);
		builderObject.SetActive (true);
	}
}
