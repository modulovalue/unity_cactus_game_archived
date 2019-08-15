using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {


	bool fadeIn = true;
	bool fadeOut = false;
	float fadeSpeed = 0.05f;
	float minAlpha = 0.0f;
	float maxAlpha = 0.4f;
	Color color;

	void Awake() {
		color = GetComponent<MeshRenderer>().material.color;
	}

	void Start () {
		color.a = 0f;
		transform.GetComponent<MeshRenderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("update");
	}

	public IEnumerator update() {
		yield return new WaitForSeconds(.05f);
		transform.GetComponent<MeshRenderer>().material.color = color;

		if (fadeIn && !fadeOut) {
			color.a += fadeSpeed;
		}

		if (!fadeIn && fadeOut) {
			color.a -= fadeSpeed;
		}

		if(color.a <= minAlpha) {
			fadeOut = false;
			//if(distanceToPlayer <= distanceToShow) {
			//	fadeIn = true;    
			//}
		}

		if(color.a >= maxAlpha) {
			fadeIn = false;
			//if(distanceToPlayer > distanceToShow) {
			//	fadeOut = true;    
			//}
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.parent != null) {
			if (other.transform.parent.parent != null) {
				if (other.transform.parent.parent.tag == "BuiltThing") {
					Destroy (transform.parent.gameObject);
				}
			}
		}
	}
}

