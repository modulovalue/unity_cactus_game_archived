using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour {

	public float speed = 10f;

	public GameObject frontLeftRotor;
	public GameObject frontRightRotor;
	public GameObject backLeftRotor;
	public GameObject backRightRotor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		frontLeftRotor.transform.Rotate(Vector3.down, 45 * Time.deltaTime * speed);
		frontRightRotor.transform.Rotate(Vector3.down, 45 * Time.deltaTime * speed);
		backLeftRotor.transform.Rotate(Vector3.down, 45 * Time.deltaTime * speed);
		backRightRotor.transform.Rotate(Vector3.down, 45 * Time.deltaTime * speed);
	}
}
