using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePortalScript : MonoBehaviour {


	public string displayLevelName;

	public GameObject teleportFX;

	public TeleportType teleportType = TeleportType.SceneNormal;


	public GameObject twoWayPortal;
	public SimplePortalScript otherPortal;


	public GameObject sceneNormalPortal;
	public string levelName;


	public GameObject oneWayPortal;
	public GameObject whereToTeleport;


	public GameObject unlockLevelPortal;
	public string unlockLevelString = "";
	public string startLevelString = "";


	void Awake() {
		
		GameObject obj = null;

		switch (teleportType) {
			case TeleportType.TwoWay:
			obj = Instantiate (twoWayPortal);
			break;
		case TeleportType.SceneNormal:
			obj = Instantiate (sceneNormalPortal);
			break;
		case TeleportType.OneWay:
			obj = Instantiate (oneWayPortal);
			break;
		case TeleportType.UnlockLevel:
			obj = Instantiate (unlockLevelPortal);
			break;
		}

		obj.transform.parent = teleportFX.transform.parent;
		obj.transform.localPosition = teleportFX.transform.localPosition;
		obj.transform.localRotation = teleportFX.transform.localRotation;


		Destroy (teleportFX);
			
	}

	public void uiControllerAction() {

		switch (teleportType) {
		case TeleportType.TwoWay:
			MainGameController.instance.TeleportPlayerTo (otherPortal.transform);
			MainGameController.instance.ResetPlayerInitPos (otherPortal.transform.position);
			break;

		case TeleportType.SceneNormal:
			MainGameController.instance.StartLevelWithFadeOut (levelName);
			break;

		case TeleportType.OneWay:
			MainGameController.instance.TeleportPlayerTo (whereToTeleport.transform);
			break;

		case TeleportType.UnlockLevel:
			MainGameController.instance.levelManager.unlockLvl (unlockLevelString);
			MainGameController.instance.StartLevelWithFadeOut (startLevelString);
			break;

		default:
			break;
		}
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.tag == "Player") {
			UIController uicontroller = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
			uicontroller.setCloseObjectButton (UIController.CloseButtonStyle.LevelOpen, uiControllerAction, displayLevelName);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.transform.root.tag == "Player") {
			UIController uicontroller = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
			uicontroller.disableCloseObjectButton ();
		}
	}

	public void setWhereToTeleport(GameObject obj) {
		whereToTeleport = obj;
	}

	void OnDrawGizmosSelected () {
		// Draw a yellow sphere at the transform's position
		if (teleportType == TeleportType.OneWay) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (whereToTeleport.transform.position, 5);
		}
	}
		
}

public enum TeleportType {
	TwoWay,
	SceneNormal,
	OneWay,
	UnlockLevel
}
