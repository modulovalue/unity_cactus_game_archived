using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Colorful;
using UnityStandardAssets.ImageEffects;

[System.Serializable]
public class PlayerStyleController : MonoBehaviour {

	public Transform headAccessoryParent;
	public Transform faceAccessoryParent;
	public int styleIndex = 1;
	public PlayerStyle style;
	public List<GameObject> styles;

	public static float MAXMOVEMENTSPEED = 50;
	public static float MAXTURNINGSPEED = 300;

	public GameObject camera;

	public List<MonoBehaviour> cameraStyleDict = new List<MonoBehaviour>();
	public VintageFast vintageFast;

	void Awake() {
		//TODO Load Loaded Costume
		//setStyle(0);
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		vintageFast = camera.GetComponent<VintageFast> ();
	}

	private void destroyAllChildren(Transform transform) {
		foreach (Transform child in transform) {
			DestroyImmediate (child.gameObject);
		}
	}

	public void setStyle(int styleIndex) {
		
		this.styleIndex = styleIndex;

		destroyAllChildren (headAccessoryParent);
		destroyAllChildren (faceAccessoryParent);

		style = styles [styleIndex].GetComponent<PlayerStyle> ();

		vintageFast.Filter = Vintage.InstragramFilter.None;

		for (int i = 0; i < cameraStyleDict.Count; i++) {
			cameraStyleDict [i].enabled = false;
		}

		switch (style.cameraStyle) {
		case CameraStyle.None:
			break;
		case CameraStyle.Snowy:
			cameraStyleDict [0].enabled = true;
			break;
		case CameraStyle.NightVision:
			cameraStyleDict [1].enabled = true;
			break;
		case CameraStyle.BlackAndWhite:
			vintageFast.Filter = Vintage.InstragramFilter.Inkwell;
			break;
		case CameraStyle.Desaturated:
			cameraStyleDict [3].enabled = true;
			break;
		case CameraStyle.Girly:
			vintageFast.Filter = Vintage.InstragramFilter.F1977;
			break;
		case CameraStyle.King:
			vintageFast.Filter = Vintage.InstragramFilter.Perpetua;
			break;
		case CameraStyle.Builder:
			vintageFast.Filter = Vintage.InstragramFilter.Hefe;
			break;
		case CameraStyle.Pixel:
			cameraStyleDict [2].enabled = true;
			break;
		default:

			break;
		}


		if(style.headAccessory != null) {
			GameObject head = Instantiate (style.headAccessory, Vector3.zero, Quaternion.identity);
			head.transform.parent = headAccessoryParent;
			head.transform.localPosition = style.headAccPos;
			head.transform.localRotation = Quaternion.Euler(style.headAccEuler);
			head.transform.localScale = style.headAccScale;
			head.SetActive (true);
		}

		if(style.faceAccessory != null) {
			GameObject face = Instantiate (style.faceAccessory, Vector3.zero, Quaternion.identity);
			face.transform.parent = faceAccessoryParent;
			face.transform.localPosition = style.faceAccPos;
			face.transform.localRotation = Quaternion.Euler(style.faceAccEuler);
			face.transform.localScale = style.faceAccScale;
			face.SetActive (true);
		}

	}

	public void nextCostume(CostumeCanvasManager manager) {
		
		int tempVal = styleIndex + 1;
		if (tempVal < styles.Count) {
			setStyle (tempVal);
		}
		setCostumeManagerValues (manager);
	}

	public void previousCostume(CostumeCanvasManager manager) {
		int tempVal = styleIndex - 1;
		if (tempVal >= 0) {
			setStyle (tempVal);
		}
		setCostumeManagerValues (manager);
	}

	public void setCostumeManagerValues(CostumeCanvasManager manager) {
		manager.movementSpeed.setValue (style.movementSpeed, MAXMOVEMENTSPEED);
		manager.turnSpeed.setValue (style.rotateSpeed, MAXTURNINGSPEED);
		manager.title.text = style.name;
		manager.description.text = style.description;
	}

}

public enum CameraStyle {
	None,
	Snowy,
	NightVision,
	BlackAndWhite,
	Desaturated,
	Girly,
	King,
	Builder,
	Pixel
}
