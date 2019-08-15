using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnyEventTrigger : MonoBehaviour {

	public UnityEvent evnt;
	// Use this for initialization
	void Start () {
		evnt.Invoke();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
