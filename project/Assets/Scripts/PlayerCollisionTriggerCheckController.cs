using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionTriggerCheckController : MonoBehaviour {

	public int countTriggered = 0;
	void OnTriggerEnter(Collider other) {
		if (other.transform.parent != null && other.transform.parent != null && 
			other.transform.parent.tag == "BuiltThing") {
			countTriggered++;
			transform.parent.parent = other.transform.parent;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.transform.parent != null && other.transform.parent.tag == "BuiltThing") {
			countTriggered--;
			transform.parent.parent = null;
		}
	}

}
