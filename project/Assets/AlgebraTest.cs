using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgebraTest : MonoBehaviour {

	public GameObject otherObj;
	public Vector3 lookAt;
	// Use this for initialization
	void Start () {

		//StartCoroutine(teleport ());
	}

	void Update () {
		Vector3 datVector = nearestPointToNormals (otherObj.transform.position, true).normalized;
		//Physics.gravity = Physics.gravity.magnitude * datVector * -1;

		//	Vector3 datPos = player.transform.eulerAngles;

		//	Vector3 newRot = Quaternion.FromToRotation (Vector3.up, datVector).eulerAngles;

		//	Vector3 newRot = (Quaternion.FromToRotation (Vector3.up, datVector) * datVector);
		//player.transform.LookAt (nearestPointToVertex (player.transform.position, true).normalized, Vector3.up);
		//DOLookAt (newRot, 2f, RotateMode.Fast);

		otherObj.transform.rotation = Quaternion.LookRotation(otherObj.transform.position - transform.position, lookAt);
		//	ding += 2f;

	}
		
	void OnDrawGizmosSelected () {
		
		Gizmos.color = Color.white;
		Gizmos.DrawLine (transform.position, nearestPointToNormals(otherObj.transform.position, true));
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (otherObj.transform.position, nearestPointToNormals(otherObj.transform.position, true));
	


		Gizmos.color = Color.red;
	
		Gizmos.color = Color.green;

		Gizmos.DrawSphere (nearestPointToNormals (otherObj.transform.position, true).normalized, 3f);

		//Vector3 v = transform.parent.transform.position;
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawLine (v, v + transform.parent.transform.right);
		//Gizmos.color = Color.red;
		//Gizmos.DrawLine (v, v + transform.parent.transform.up);
		//
		//Vector3 side1 = v + transform.parent.transform.right - v;
		//Vector3 side2 =  v + transform.parent.transform.up - v;
		//
		//
		//Vector3 cross = -1 * Vector3.Cross (side1, side2);
		//Gizmos.color = Color.white;
		//Gizmos.DrawLine (portalContainer.transform.position, portalContainer.transform.InverseTransformVector( cross ) );
	}



	public Vector3 nearestPointToNormals(Vector3 point, bool trans) {

		// convert point to local space
		point = transform.InverseTransformPoint(point);

		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		float minDistanceSqr = Mathf.Infinity;
		Vector3 nearestVertex = Vector3.zero;
		// scan all vertices to find nearest
		foreach (Vector3 vertex in mesh.normals)
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

	public IEnumerator teleport() {
		otherObj.GetComponent<Rigidbody> ().velocity = Vector3.down * 20f;
		yield return new WaitForSeconds (0.05f);
		otherObj.transform.position = transform.position;


		Vector3 vel = otherObj.GetComponent<Rigidbody> ().velocity;
		otherObj.GetComponent<Rigidbody> ().velocity = (transform.up).normalized * vel.magnitude;
		Debug.Log ("relepotpe");
		otherObj = Instantiate (GameObject.CreatePrimitive (PrimitiveType.Sphere));
		otherObj.AddComponent<Rigidbody> ();

		StartCoroutine (teleport ());

	}
}
