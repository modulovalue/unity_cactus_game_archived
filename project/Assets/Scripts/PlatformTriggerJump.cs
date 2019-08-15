using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriggerJump : MonoBehaviour {

	private bool triggered = false;

	public float jumpStrength = 4f;

	void OnTriggerEnter(Collider other) {
		if (other.transform.parent.tag == "Player"  && triggered == false) {
			triggered = true;
			trigger (other.transform.parent.gameObject);
		}
	}

	public void trigger(GameObject obj) {
		obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().AddForce (0, jumpStrength, 0, ForceMode.Impulse);
		StartCoroutine (wait ());
	}

	public IEnumerator wait() {
		yield return new WaitForSeconds (1f);
		triggered = false;
	}
}
