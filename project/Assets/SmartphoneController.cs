using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class SmartphoneController : MonoBehaviour {

	public GameObject camCam;
	public GameObject frontCam;
	public GameObject backCam;
	public RenderTexture renderTexture;

	private StateMachine<SmartphoneStates> fsm;

	private const string CAMPREFNAME = "SmartphoneCam";

	void Awake() {
		fsm = StateMachine<SmartphoneStates>.Initialize (this, SmartphoneStates.Off);
	}

	private void FrontCam_Enter() {
		camCam.SetActive (true);
		PlayerPrefs.SetInt (CAMPREFNAME, 1);
		frontCam.SetActive (true);
		frontCam.GetComponent<Camera> ().targetTexture = renderTexture;
		backCam.SetActive (false);
	}

	private void BackCam_Enter() {
		camCam.SetActive (true);
		PlayerPrefs.SetInt (CAMPREFNAME, 0);
		frontCam.SetActive (false);
		backCam.GetComponent<Camera> ().targetTexture = renderTexture;
		backCam.SetActive (true);
	}

	private void Off_Enter() {
		camCam.SetActive (false);
		frontCam.SetActive (false);
		backCam.SetActive (false);
	}


	public void turnOn() {
		int frontOrBack = PlayerPrefs.GetInt (CAMPREFNAME, 1);
		if (frontOrBack == 1) {
			front ();
		} else {
			back ();
		}
	}
		
	public void turnOff() {
		fsm.ChangeState (SmartphoneStates.Off);
	}

	public void switchCam() {
		if (fsm.State == SmartphoneStates.FrontCam) {
			back ();
		} else {
			front ();
		}
	}

	private void front() {
		fsm.ChangeState (SmartphoneStates.FrontCam);
	}

	private void back() {
		fsm.ChangeState (SmartphoneStates.BackCam);
	}

}

public enum SmartphoneStates {
	FrontCam,
	BackCam,
	Off,
}