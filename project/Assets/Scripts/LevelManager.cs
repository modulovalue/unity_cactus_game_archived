using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager {

	private List<string> myFoldOuts2 = new List<string>() {
		"prefsInitialized"
	};

	public IEnumerator startlevel(Action action, string level, float waitTime) {
		action.Invoke ();
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene ("Level" + level);
	}

	public void unlockLvl(string levelname) {
		PlayerPrefs.SetInt ("Level" + levelname+ "Unlocked", 1);
	}	

	public void lockLvl(string levelname) {
		PlayerPrefs.SetInt ("Level" + levelname+ "Unlocked", -1);
	}

	public bool isUnlocked(string levelname) {
		if (PlayerPrefs.GetInt ("Level" + levelname+ "Unlocked") > 0) {
			return true;
		} else {
			return false;
		}
	}
}

