using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraTest : MonoBehaviour {

	public GameObject player;
	public GameObject mainCamera;
	public GameObject otherPortal;
	public Vector3 test;

	public bool teleport = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	// Update is called once per frame
	void Update () {
		//transform.localPosition = mainCamera.transform.localPosition;

		//Vector3 t = mainCamera.transform.localRotation.eulerAngles;
		//transform.localRotation = Quaternion.Euler(new Vector3(t.x,(t.y) - ( test.y + mainCamera.transform.rotation.eulerAngles.y), t.z));

	}

	public void collided() {
		if (teleport == true) {
			player.gameObject.GetComponent<PlayerController> ().portalAnim ();
			//float ydif = Mathf.Abs(player.transform.position.y - otherPortal.transform.position.y);
			//Vector3 dot = Vector3.Dot (otherPortal.transform.position, otherPortal.transform.parent.FindChild ("Camera").transform.position);
			player.transform.localPosition = otherPortal.transform.position + (otherPortal.transform.up * 4f);
			//Debug.Log(" 1. " +transform.parent.eulerAngles.y+ " 2. " +otherPortal.transform.eulerAngles.y+ "3." + player.transform.eulerAngles.y );
			//float newYRot = transform.parent.eulerAngles.y - otherPortal.transform.eulerAngles.y;
			//player.transform.eulerAngles = new Vector3 (player.transform.eulerAngles.x, otherPortal.transform.parent.FindChild("Camera").transform.eulerAngles.y, player.transform.eulerAngles.z);
			Vector3 vel = player.GetComponent<Rigidbody> ().velocity;
			player.GetComponent<Rigidbody> ().velocity = (otherPortal.transform.up).normalized * vel.magnitude;

		}
	}

	void OnDrawGizmosSelected () {

		Gizmos.color = Color.red;
		//Gizmos.DrawLine(transform.parent.transform.position + transform.parent.transform.transform.right, transform.parent.transform.position);
		//Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.parent.position + transform.parent.transform.forward, transform.parent.position);
		Vector3 v = transform.parent.position;
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawLine (v, v + transform.parent.transform.right);
		//Gizmos.color = Color.red;
		//Gizmos.DrawLine (v, v + transform.parent.transform.up);

		Vector3 side1 = v + transform.parent.transform.right - v;
		Vector3 side2 =  v + transform.parent.transform.up - v;
		
		
		Vector3 cross = -1 * Vector3.Cross (side1, side2);

		//Gizmos.DrawLine(cross, transform.parent.position);
		Gizmos.color = Color.white;
	}
}
