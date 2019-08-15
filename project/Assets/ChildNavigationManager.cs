using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;


public class ChildNavigationManager : MonoBehaviour {

	public ChildRunawayPaths runawayPath;
	public NavMeshAgent agent;


	void Start () {
		agent = GetComponent<NavMeshAgent> ();

		agent.SetDestination (runawayPath.pathsToGoTo [0]);
		
	}


	void Update () {
		
	}
}
