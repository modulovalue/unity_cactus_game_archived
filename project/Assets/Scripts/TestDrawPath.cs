using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrawPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public float somevariable = 2;
	
	// Update is called once per frame
	void Update () {
		Vector3 lastNode = transform.position + Vector3.up * 2;

			Vector3 newNode = lastNode + Vector3.forward * 3;

			float x = newNode.x;
			newNode = new Vector3(newNode.x, newNode.y +  ((-(x*x))), newNode.z);

			Debug.DrawLine(lastNode, newNode, Color.red);
			lastNode = newNode;

	}
}
