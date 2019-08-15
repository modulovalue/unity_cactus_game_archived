using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(destroyroutine());
	}

	public IEnumerator destroyroutine() {
		yield return new WaitForSeconds(2f);
		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
