using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

	public Transform player;
	private bool opened = false;
	public float distanceToPlayerToOpen = 15f;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update() {
		if (Vector3.Distance (transform.position, player.position) <= distanceToPlayerToOpen && !opened) {
			GetComponent<Animator> ().SetTrigger (Animator.StringToHash ("open"));
			opened = true;
		} else if (Vector3.Distance (transform.position, player.position) > distanceToPlayerToOpen && opened){
			GetComponent<Animator> ().SetTrigger (Animator.StringToHash ("close"));
			opened = false;
		}
	}

	void OnDrawGizmosSelected() {
		Color color = Color.yellow;
		color.a = 0.15f;
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, distanceToPlayerToOpen);
	}
}
