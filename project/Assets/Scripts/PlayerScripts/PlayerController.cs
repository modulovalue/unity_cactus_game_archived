using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using Prime31.MessageKit;
using MonsterLove.StateMachine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {
	
	public Animator anim;

	public PlayerMovementController movementController;
	public PlayerStyleController styleController;
	public PlayerCollisionTriggerCheckController colTrigController; 

	public float addedAngle = 0;
	public GameObject cameraa;

	public string lastTrigger;

	private StateMachine<PlayerStates> fsm;

	public GameObject phone;

	void Awake() {
		fsm = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
		movementController = GetComponent<PlayerMovementController> ();
		styleController = GetComponent<PlayerStyleController> ();
		colTrigController = transform.FindChild ("PlatformTrigger").GetComponent<PlayerCollisionTriggerCheckController>();
		cameraa = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	public void portalAnim() {
		cameraa.GetComponent<Animator>().SetTrigger (Animator.StringToHash ("Portal"));
	}

	private void Idle_Enter() { changeAnim("Idle");}
	private void Walking_Enter() { changeAnim ("Move");}
	private void Falling_Enter() { changeAnim("Fall");}
	private void Attack_Enter() { changeAnim("Attack");}

	private IEnumerator Selfie_Enter() { 
		movementController.changeState (PlayerMovementStates.CantWalk);
		changeAnim("TakeOutPhone");
		phone.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		phone.GetComponent<SmartphoneController> ().turnOn ();
		cameraa.SetActive (false);
	}

	private void Selfie_Exit() {
		cameraa.SetActive (true);
		phone.GetComponent<SmartphoneController> ().turnOff ();
		phone.SetActive (false);
		movementController.changeState (PlayerMovementStates.CanWalk);
	}

	private void Falling_Update() {
		if (movementController.trueGrounded == true) {
			fsm.ChangeState (PlayerStates.Idle);
			MessageKit.post (LevelDesignMessages.notFalling);
		}
	}

	private void Walking_Update() {
		if (movementController.z == 0) {
			fsm.ChangeState (PlayerStates.Idle);
		}
	}

	public void switchCam() {
		phone.GetComponent<SmartphoneController>().switchCam();
	}

	public void changeAnim(string str) {
		anim.SetTrigger (Animator.StringToHash (str));
	}
		

	public void changeState(PlayerStates state) { 
		if (fsm == null) {
			fsm = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
		}
		fsm.ChangeState(state);
	}

	public StateMachine<PlayerStates> getFSM() { 
		return fsm; 
	}


	void OnDrawGizmosSelected () {
		Gizmos.color = Color.red; 
		Gizmos.DrawLine (transform.position, transform.position + transform.rotation.eulerAngles);
		Gizmos.DrawSphere (transform.position, 0.2f);
		Gizmos.color = Color.yellow; 
		Gizmos.DrawSphere (transform.position + (transform.rotation.eulerAngles).normalized, 0.2f);
	}

}

public enum PlayerStates {
	Idle,
	Walking,
	Falling,
	Attack,
	Selfie,
}
