using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Serialization;
using DG.Tweening;
using CinemaDirector;
using Prime31.MessageKit;
using MonsterLove.StateMachine;

public class LevelDesignScript : MonoBehaviour {

	public GameObject camera; 
	public GameObject lookingAt; 

	public Vector3 initPos;
	public Vector3 initRot;
	public Vector3 selfiePos;
	public Vector3 selfieRot;
	public Vector3 buildingPos;
	public Vector3 buildingRot;
	public Vector3 fallingPos;
	public Vector3 fallingRot;

	private Vector3 camCurPos;
	private Vector3 camCurRot;



	private StateMachine<LevelDesignCameraStates> fsm;

	void Awake() {
		fsm = StateMachine<LevelDesignCameraStates>.Initialize(this, LevelDesignCameraStates.Normal);
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camCurPos = initPos;
		camCurRot = initRot;
		MessageKit.addObserver( LevelDesignMessages.isFalling, startFalling );
		MessageKit.addObserver( LevelDesignMessages.notFalling, stopFalling );
	}

	public void startFalling() {
		fsm.ChangeState (LevelDesignCameraStates.Falling);
	}
	public void stopFalling() {
		fsm.ChangeState (LevelDesignCameraStates.Normal);
	}
		

	private void Normal_Enter() { 
		this.camCurPos = initPos;
		this.camCurRot = initRot;
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().changeState(PlayerStates.Idle);
	}
	private void Building_Enter() { 
		this.camCurPos = buildingPos;
		this.camCurRot = buildingRot;

	}
	private void Selfie_Enter() { 
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().changeState(PlayerStates.Selfie);
	}

	public void switchCam() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().switchCam ();
	}

	private void NormalNoAnimation_Enter() { 
		this.camCurPos = initPos;
		this.camCurRot = initRot;
	}
	private void Falling_Enter() {
		this.camCurPos = fallingPos;
		this.camCurRot = fallingRot;
	}

	public void changeState(LevelDesignCameraStates state) {
		fsm.ChangeState(state);
	}

	public StateMachine<LevelDesignCameraStates> getFSM() {
		return fsm;
	}
		


	public void Update() {

		Vector3 alternatePos = this.camCurPos;
		Vector3 alternateRot = this.camCurRot;

		float posChangeDur = 1f;

		RaycastHit hitinfo;

		Vector3 tempcameralocalpos = camera.transform.localPosition;

		camera.transform.localPosition = alternatePos;

		if ( (fsm.State == LevelDesignCameraStates.Normal ||
			fsm.State == LevelDesignCameraStates.NormalNoAnimation )
			&& Physics.Linecast (lookingAt.transform.position, camera.transform.position, out hitinfo) 
			&& hitinfo.collider.gameObject.tag == "CaveWall") {
			float dstToWall = Vector3.Distance (hitinfo.point, lookingAt.transform.position);
			alternatePos.z = -1 * (dstToWall / 2);
			alternateRot.x +=( (dstToWall / 12f) * -30f ) + 40;
			posChangeDur = 0.4f;
		}

		camera.transform.localPosition = tempcameralocalpos;

		camera.transform.DOLocalMove (alternatePos, posChangeDur, false);
		camera.transform.DOLocalRotate (alternateRot, 0.8f, RotateMode.Fast);
	}


	public void nightVision(bool active) {
		camera.GetComponent<CameraFilterPack_Oculus_NightVision3> ().enabled = active;
	}		

}


public enum LevelDesignCameraStates { 
	Building, 
	Selfie, 
	Normal,
	NormalNoAnimation,
	Falling
}
public class LevelDesignMessages
{
	public const int isFalling = 0;
	public const int notFalling = 1;
}
