using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;
using System;
using CinemaDirector;
using DG.Tweening;

public class UIController : MonoBehaviour {

	public Camera viewCamera;

	public GameObject Canvases;
	public GameObject camCanvas;
	public GameObject walkingCanvas;
	public GameObject buildingCanvas;
	public GameObject selectItemCanvas;
	public GameObject changeClothesCanvas;
	public GameObject menuCanvas;

	public GameObject changeClothesCutcene;
	private GameObject changeClothesCutsceneInstance;

	public PlayerController player;

	private Action closeObjectAction;
	public Image closeObjectButton;
	public Text closeObjectButtonTextLabel;

	public LevelDesignScript lvldesignscript;

	void Awake() {
		changeClothesCutsceneInstance = Instantiate (changeClothesCutcene);

		changeClothesCutsceneInstance.transform.FindChild ("Main Camera Group").GetComponent<ActorTrackGroup> ().Actor = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		//LookAtTarget was made public in TransformLookAtAction code
		changeClothesCutsceneInstance.transform.FindChild ("Main Camera Group").FindChild ("Actor Track").FindChild("Look At").GetComponent<TransformLookAtAction>().LookAtTarget = GameObject.FindGameObjectWithTag ("Player").transform.FindChild("LookAt").gameObject;
		changeClothesCutsceneInstance.SetActive (false);

		viewCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		lvldesignscript = GetComponent<LevelDesignScript> ();

		this.closeObjectButton.gameObject.SetActive(false);

	}
		
	void Start() {
		setWalking ();
	}

	public void setWalking() {
		disableAllCanvas (walkingCanvas);
		changeClothesCutsceneInstance.SetActive (false);
		lvldesignscript.changeState(LevelDesignCameraStates.Normal);
		enableBuilding (false);
	}

	public void setWalkingNoAnimation() {
		disableAllCanvas (walkingCanvas);
		changeClothesCutsceneInstance.SetActive (false);
		lvldesignscript.changeState(LevelDesignCameraStates.NormalNoAnimation);
	}

	public void setCamera() {
		disableAllCanvas (camCanvas);
		lvldesignscript.changeState(LevelDesignCameraStates.Selfie);
	}


	public void switchCamera() {
		lvldesignscript.switchCam ();
	}

	public void setBuilding() {
		disableAllCanvas (buildingCanvas);
		lvldesignscript.changeState(LevelDesignCameraStates.Building);
		enableBuilding (true);
	}

	public void setChangeClothes() {
		disableAllCanvas (changeClothesCanvas);
		changeClothesCutsceneInstance.SetActive (true);
		changeClothesCutsceneInstance.GetComponent<Cutscene> ().Stop ();
		changeClothesCutsceneInstance.GetComponent<Cutscene> ().Play ();
	}
		
	public void settings() {
		disableAllCanvas (menuCanvas);
		lvldesignscript.changeState(LevelDesignCameraStates.Normal);
	}

	public void setChooseItemCanvas() {
		disableAllCanvas (selectItemCanvas);
		lvldesignscript.changeState(LevelDesignCameraStates.Normal);
	}

	public void disableAllCanvas(GameObject canvas) {
		foreach (Transform child in Canvases.transform) {
			child.gameObject.SetActive (false);
		}
		canvas.SetActive (true);
	}
		


	public void enableBuilding(bool val) {
		if (player.GetComponent<BuildObjectsPlayer> () != null) {
			if (val) {
				player.GetComponent<BuildObjectsPlayer>().activateBuildMode ();
			} else {
				player.GetComponent<BuildObjectsPlayer>().deactivateBuildMode ();
			}
		}
	}

	public void shutter() {
		//StartCoroutine (screenshot());
	}

	//public IEnumerator screenshot() {
		//CaptureAndSave snapShot = GameObject.FindObjectOfType<CaptureAndSave>();
		//camCanvas.SetActive (false);
		//snapShot.CaptureAndSaveToAlbum();
		//yield return new WaitForSeconds(0.3f);
		//camCanvas.SetActive (true);
	//}
		
	public Sprite closedSign;
	public Sprite openSign;

	private bool flagSet = false;

	public void setCloseObjectButton(CloseButtonStyle style, Action action, string closeObjectButtonText) {
		this.closeObjectButton.gameObject.SetActive(true);
		this.closeObjectAction = action;
		this.closeObjectButtonTextLabel.text = closeObjectButtonText;

		if (style == CloseButtonStyle.LevelOpen && !flagSet) {
			this.closeObjectButton.sprite = openSign;
			this.closeObjectButton.transform.GetComponent<DOTweenAnimation> ().DOPlay ();
			flagSet = true;
		} else if(style == CloseButtonStyle.LevelClosed && !flagSet){
			this.closeObjectButton.sprite = closedSign;
			this.closeObjectButton.transform.GetComponent<DOTweenAnimation> ().DOKill ();
			flagSet = true;
		}
	}

	public void disableCloseObjectButton() {
		this.closeObjectButton.gameObject.SetActive(false);
		flagSet = false;
	}

	public void closeObjectEventCall() {
		if (closeObjectAction != null) {
			closeObjectAction ();
		}
	}

	void Update() {
		
			if (Input.GetKeyDown (KeyCode.Joystick1Button3)) {
				//dreieck
				if (lvldesignscript.getFSM().State == LevelDesignCameraStates.Selfie) {
					setWalking ();
				} else {
					setCamera ();
				}

			} else if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
				//kreis
			} else if (Input.GetKeyDown (KeyCode.Joystick1Button2)) {
				//viereck
			} else if (Input.GetKeyDown (KeyCode.Joystick1Button0)) {
				//x
				if (lvldesignscript.getFSM().State == LevelDesignCameraStates.Selfie) {
					shutter ();
				} else {
					closeObjectEventCall ();
				}
			} 
		//foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
		//	if (Input.GetKeyDown(kcode))
        //		Debug.Log("KeyCode down: " + kcode);
		//}
	}

	public enum CloseButtonStyle { LevelOpen, LevelClosed }

}
