using System.Collections;
using System.Collections.Generic;
using System;
using CnControls;
using UnityEngine;
using UnityEngine.UI;
using Prime31.MessageKit;
using MonsterLove.StateMachine;
using DG.Tweening;

[System.Serializable]
public class PlayerMovementController : MonoBehaviour{

	public float movementSpeed = 14f;
	public float rotateSpeed = 200f;

	private PlayerController controller;
	private Rigidbody playerRB;

	private Vector3 playerInitPos;
	private Quaternion playerInitQuat;

	public StateMachine<PlayerMovementStates> fsm;

	public bool isGrounded = false;
	public bool trueGrounded = false;

	public GravityVertPull gravityVertPull = null;

	void Awake() {
		fsm = StateMachine<PlayerMovementStates>.Initialize(this, PlayerMovementStates.CanWalk);
		controller = GetComponent<PlayerController> ();
		playerRB = GetComponent<Rigidbody> ();
	}

	void Start() {
		setInitPosAndQuat (transform.position, transform.rotation);
	}

	public float x = 0;
	public float z = 0;

	public void setGravityPuller(GravityVertPull gravityPull) {
		
		this.gravityVertPull = gravityPull;

		if (gravityPull == null) {
			GetComponent<Rigidbody> ().useGravity = true;
			Quaternion targetRotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
			transform.DORotateQuaternion (targetRotation, 0.5f);
			//transform.parent = null;
		} else {
			GetComponent<Rigidbody> ().useGravity = false;
			//transform.parent = gravityPull.worldObjects2.transform;
		}

	}


	private void CanWalk_Update() { 
		
		x = CnInputManager.GetAxis ("Horizontal") * Time.deltaTime * rotateSpeed;
		z = CnInputManager.GetAxis ("Vertical") * Time.deltaTime * movementSpeed;

		if (Mathf.Abs (z) > 0 && controller.getFSM().State != PlayerStates.Walking) { 
			if (controller.getFSM ().State != PlayerStates.Falling) {
				controller.changeState (PlayerStates.Walking);
			}
		} else if (z == 0 && controller.getFSM().State == PlayerStates.Walking) {
			controller.changeState (PlayerStates.Idle);
		}

		trueGrounded = isGrounded || Physics.Linecast (transform.position, transform.position +( transform.up * -1 * 3f), LayerMask.NameToLayer("Objects"));

		move (z);
		if (gravityVertPull == null) {
			playerRB.MoveRotation (transform.rotation * Quaternion.Euler (new Vector3 (0, x, 0)));
		}
		//	GetComponent<Rigidbody>().MoveRotation (transform.rotation * Quaternion.Euler(new Vector3(0,x,0)));

		if (!trueGrounded) {
			if (controller.getFSM().State != PlayerStates.Falling) {
				MessageKit.post (LevelDesignMessages.isFalling);
				controller.changeState (PlayerStates.Falling);
			} 
		} else {
			if (controller.getFSM().State == PlayerStates.Falling) {
				MessageKit.post (LevelDesignMessages.notFalling);
				controller.changeState (PlayerStates.Idle);
			}
		}

	}

	private void move(float amount) {
		playerRB.MovePosition (transform.position + transform.forward * z );
	}
		
	private void Reset_Enter() {
		GetComponent<Rigidbody> ().isKinematic = true;
		transform.position = playerInitPos;
		transform.rotation = playerInitQuat;
		GetComponent<Rigidbody> ().isKinematic = false;
		fsm.ChangeState (PlayerMovementStates.CanWalk);
	}
		
	public void setInitPosAndQuat(Vector3 pos, Quaternion quat) {
		playerInitPos = pos;
		playerInitQuat = quat;
	}

	public void changeState(PlayerMovementStates state) {
		fsm.ChangeState(state);
	}

	public void setGrounded(bool bo) {
		isGrounded = bo;
	}

	void OnDrawGizmosSelected () {
		//Gizmos.color = Color.red;
		//Gizmos.DrawLine ((Physics.gravity.normalized * 10) + transform.position, transform.position);
	}

}

public enum PlayerMovementStates {
	CanWalk,
	CantWalk,
	Reset,
}