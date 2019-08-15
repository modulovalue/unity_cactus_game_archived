using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelPortalScript : MonoBehaviour {

	public GameObject particleEffect;
	public TextMesh lvlText;

	public AnimationCurve animationCurve;
	public GameObject obj; 
	public GameObject objParticleEffect; 

	public string levelName;
	public string displayLevelName;

	public GameObject highLight;
	GameObject player;

	// Use this for initialization
	void Start () {
		lvlText.text = displayLevelName;
		objParticleEffect.SetActive (false);
		player = GameObject.FindGameObjectWithTag ("Player");
		//initHighLight ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.tag == "Player") {
			UIController uicontroller = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
			if (MainGameController.instance.levelManager.isUnlocked (levelName)) {
				objParticleEffect.SetActive (true);
				uicontroller.setCloseObjectButton (UIController.CloseButtonStyle.LevelOpen, uiControllerAction, "Level " + levelName);
			} else {
				objParticleEffect.SetActive (false);
				uicontroller.setCloseObjectButton (UIController.CloseButtonStyle.LevelClosed, null, "Level " + levelName);
			}
		}
	}

	public void uiControllerAction() {
		MainGameController.instance.StartLevelWithFadeOut(levelName);
	}

	void OnTriggerExit(Collider other) {
		if (other.transform.parent.tag == "Player") {
			objParticleEffect.SetActive (false);
			UIController uicontroller = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
			uicontroller.disableCloseObjectButton ();
		}
	}

	public void initHighLight() {
		highLight.SetActive (false);
	
		int level = -1;

		if(int.TryParse (levelName, out level)) {
			if (MainGameController.instance.levelManager.isUnlocked (levelName) &&
				!MainGameController.instance.levelManager.isUnlocked ((int.Parse (levelName) + 1) + "")) {
				highLight.SetActive (true);
				highLight.GetComponent<LightShafts> ().m_Cameras [0] = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
			}
		}

		//print (levelName + "  " + int.TryParse (levelName, out level) + "  " + level + "  " + ((int.Parse (levelName) + 1) + ""));
	}
}
