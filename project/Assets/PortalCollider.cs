using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollider : MonoBehaviour {

	public PortalCameraTest test;

	void OnTriggerEnter(Collider other) {
		test.collided();
	}

}
