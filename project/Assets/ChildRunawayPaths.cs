using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRunawayPaths : MonoBehaviour {

	public Vector3[] pathsToGoTo;


	void Start () {
		
	}
		
	void Update () {
		
	}

	public void OnDrawGizmosSelected () {
		Gizmos.color = Color.yellow;
		foreach (Vector3 vec in pathsToGoTo) {
			Gizmos.DrawSphere (vec, 3);
		}
	}
}
