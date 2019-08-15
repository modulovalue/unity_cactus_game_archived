using UnityEngine;
using System.Collections;

public class GravityBody : MonoBehaviour {
	public GravityAttractor attractor;
	private Transform myTransform;

	void Awake() {        
		attractor = Object.FindObjectOfType<GravityAttractor>();
	}

	void Start () {
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		GetComponent<Rigidbody>().useGravity = false;
		myTransform = transform;
	}

	void Update () {   
		attractor.Attract(myTransform);
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
	}

	public float moveSpeed = 70;
	public Vector3 moveDir;


	void FixedUpdate() {
		GetComponent<Rigidbody>().MovePosition(transform.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime); 
	}
}