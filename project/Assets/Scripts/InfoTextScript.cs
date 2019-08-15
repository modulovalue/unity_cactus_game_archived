using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class InfoTextScript : MonoBehaviour {

	bool fadeIn = false;
	bool fadeOut = false;
	float fadeSpeed = 0.3f;
	float minAlpha = 0.0f;
	float maxAlpha = 1.0f;
	public Color color = Color.white;


	GameObject player;
	public float distanceToShow = 30f;
	public Vector3 triggerOffset = Vector3.zero;

	void Awake() {
		color = color;
	}

	void Start() {
		player = GameObject.FindWithTag("Player");
		GetComponent<TextMesh>().color = Color.clear;
		color.a = 0f;
	}

	void Update() {    


		float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position + triggerOffset);


		if (!Application.isPlaying) {
			Color clr = color;
			clr.a = 1f;
			GetComponent<TextMesh> ().color = clr;
		} else {
			GetComponent<TextMesh> ().color = color;
		}

		if (fadeIn && !fadeOut) {
			color.a += fadeSpeed;
		}

		if (fadeOut && !fadeIn) {
			color.a -= fadeSpeed;
		}

		if(color.a <= minAlpha) {
			fadeOut = false;
			if(distanceToPlayer <= distanceToShow) {
				fadeIn = true;    
			}
		}

		if(color.a >= maxAlpha) {
			fadeIn = false;
			if(distanceToPlayer > distanceToShow) {
				fadeOut = true;    
			}
		}
	}

	void OnDrawGizmosSelected() {
		Color color = Color.red;
		color.a = 0.2f;
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position + triggerOffset, distanceToShow);
	}
}
