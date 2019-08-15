using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[ExecuteInEditMode]
public class TriggerScript : MonoBehaviour {

	private SphereCollider collider;
	private bool entered = false;
	public string levelString = "";

	void Start () {
		collider = transform.GetComponent<SphereCollider> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.tag == "Player" && !entered) {
			print ("ENTERERD");
			entered = true;
			MainGameController.instance.levelManager.unlockLvl (levelString);
			MainGameController.instance.StartLevelWithFadeOut("SelectScene");
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.transform.root.tag == "Player" && entered) {
			print ("EXITted");
			entered = false;
		}
	}

	void OnDrawGizmosSelected() {
		Color color = Color.blue;
		color.a = 0.2f;
		Gizmos.color = color;
		Gizmos.DrawSphere(collider.center + transform.position, collider.radius);
	}
}
