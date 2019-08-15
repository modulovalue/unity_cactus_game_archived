using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CinemaDirector;

public class CutsceneMouseButtonSkip : MonoBehaviour {

	Cutscene cutscene;

	void Start () {
		cutscene = GetComponent<Cutscene> ();
	}
		
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			if (cutscene != null && cutscene.State == CinemaDirector.Cutscene.CutsceneState.Playing) {
				cutscene.Skip();
			}
		}
	}
}
