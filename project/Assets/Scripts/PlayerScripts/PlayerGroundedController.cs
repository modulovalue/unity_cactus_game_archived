using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class PlayerGroundedController : MonoBehaviour {

	private SphereCollider collider;

	public UnityEvent notGroundedAction;
	public UnityEvent groundedAction;

	private bool switched = false;

	private int objects = 0;

	void Start() {
		groundedAction.Invoke ();
	}

	void OnTriggerEnter(Collider other) {
		objects++;

	}

	void OnTriggerExit(Collider other) {
		objects--;
	}
		
	void Update() {
		if (objects < 1) {
			if (switched) {

			} else {
				notGroundedAction.Invoke ();
				switched = true;
			}
		} else {
			if (switched) {
				groundedAction.Invoke ();
				switched = false;
			} else {
			}
		}
	}	

}
