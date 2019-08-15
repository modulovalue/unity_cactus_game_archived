using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayerPrefs {

	public static void SetVector3(string name, Vector3 vector) {
		PlayerPrefs.SetFloat (name + "xvalue", vector.x);
		PlayerPrefs.SetFloat (name + "yvalue", vector.y);
		PlayerPrefs.SetFloat (name + "zvalue", vector.z);
	}

	public static Vector3 GetVector3(string name) {
		return new Vector3 (
			PlayerPrefs.GetFloat (name + "xvalue"),
			PlayerPrefs.GetFloat (name + "yvalue"),
			PlayerPrefs.GetFloat (name + "zvalue")
		);
	}
}
