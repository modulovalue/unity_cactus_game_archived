using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GravityVertPull : MonoBehaviour {

	public GameObject player;
	public float gravityAffectDistance = 10;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		gravmag = Physics.gravity.magnitude;
	}

	private float gravmag = 0;

	public GameObject worldObjects;
	public GameObject worldObjects2;
	public bool enableObjectPull = false;

	private bool playerSet = false;

	void Update () {
		if (Vector3.Distance (player.transform.position, transform.position) < gravityAffectDistance) {
			if (!playerSet) {
				playerSet = true;
				player.GetComponent<PlayerMovementController>().setGravityPuller (this);
			}
			setRotationAndAddForce (player.transform, player.GetComponent<PlayerMovementController> ().x);
		} else {
			if (playerSet) {
				if (player.GetComponent<PlayerMovementController>().gravityVertPull == this || 
					player.GetComponent<PlayerMovementController>().gravityVertPull == null) {
					player.GetComponent<PlayerMovementController>().setGravityPuller (null);
				}

				playerSet = false;
			}
		}

		if (enableObjectPull) {
			foreach (Transform obj in worldObjects.GetComponentInChildren<Transform>()) {
				pullObject (obj.gameObject);
			}
		}
		
	}

	public void setRotationAndAddForce(Transform trans, float yRotation) {
		
		Vector3 datVector = nearestPointToVertex (trans.position, false).normalized;
		Vector3 gravityUp = (player.transform.position - transform.position);

		Quaternion deltaRotation = Quaternion.Euler (new Vector3 (0, yRotation, 0));

		trans.GetComponent<Rigidbody>().MoveRotation (Quaternion.FromToRotation (trans.up, gravityUp) * trans.rotation * deltaRotation);
		trans.gameObject.GetComponent<Rigidbody> ().AddForce ( datVector * -1 * 4 );

	}


	public void pullObject(GameObject pull) {

		Vector3 datVector = nearestPointToVertex (pull.transform.position, false).normalized;

		Vector3 gravityUp = (pull.transform.position - transform.position).normalized;
		Vector3 bodyUp = pull.transform.up;

		pull.GetComponent<Rigidbody> ().useGravity = false;

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * pull.transform.rotation;

		pull.GetComponent <Rigidbody> ().AddForce (gravmag * datVector * -1 * 15);

		pull.transform.rotation =  targetRotation;

	}

	public Vector3 nearestPointToVertex(Vector3 point, bool trans) {

		// convert point to local space
		point = transform.parent.transform.InverseTransformPoint(point);

		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		float minDistanceSqr = Mathf.Infinity;
		Vector3 nearestVertex = Vector3.zero;
		// scan all vertices to find nearest
		foreach (Vector3 vertex in mesh.vertices)
		{
			Vector3 diff = point-vertex;
			float distSqr = diff.sqrMagnitude;
			if (distSqr < minDistanceSqr)
			{
				minDistanceSqr = distSqr;
				nearestVertex = vertex;
			}
		}
		// convert nearest vertex back to world space
		if (trans) {
			return transform.TransformPoint (nearestVertex);
		} else {
			return nearestVertex;
		}
	}
	Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c) {
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross(side1, side2).normalized;
	}

	void OnDrawGizmosSelected () {


		//Gizmos.color = Color.red;
		//
		//Gizmos.color = Color.cyan;
		//Gizmos.DrawLine ((Physics.gravity.normalized * -10) + player.transform.position, player.transform.position);
		//
		//Gizmos.color = Color.magenta;
		//Vector3 datVector = nearestPointToVertex (player.transform.position, true).normalized;
		//Gizmos.DrawLine (datVector, player.transform.position);
		Color color = Color.blue;
		color.a = 0.2f;
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, gravityAffectDistance);

	}


}
