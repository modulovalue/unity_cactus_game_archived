using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CinemaDirector;
using UnityEngine.UI;
using DG.Tweening;

public class MainGameController : MonoBehaviour {


	private PlayerController player;
	private UIController uicontroller;

	public InventoryItemScript standartBuildThing;

	public float playerHeightToReset = -100f;

	public Cutscene levelChangeFade;
	public Cutscene levelStartFade;

	public GameObject levelChangeFadePrefab;
	public GameObject levelStartFadePrefab;

	public static MainGameController instance = null;  

	public LevelManager levelManager = null;


	private List<string> initialyTruePlayerPrefVariables = new List<string>() {
		"prefsInitialized",
		"Level1Unlocked"
	};


	void Awake() {

		initPrefs ();

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);   
		}

		levelManager = new LevelManager ();
		levelManager.unlockLvl ("Forest");

		levelChangeFade = Instantiate(levelChangeFadePrefab, Vector3.zero, Quaternion.identity).GetComponent<Cutscene>();
		levelStartFade  = Instantiate(levelStartFadePrefab, Vector3.zero, Quaternion.identity).GetComponent<Cutscene>();
	
		levelChangeFade.transform.FindChild ("Main Camera Group").GetComponent<ActorTrackGroup> ().Actor = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	public void StartLevelWithFadeOut(string level) {
		StartCoroutine(levelManager.startlevel (
			() => { levelChangeFade.Play();	},
			level, 
			0.2f));
	}

	public void TeleportPlayerTo(Transform trans) {
		StartCoroutine(teleportPlayer(trans));
	}

	public IEnumerator teleportPlayer(Transform trans) {
		uicontroller.viewCamera.GetComponent<Animator> ().SetTrigger ("Teleport");
		yield return new WaitForSeconds(1f);
		player.transform.position = trans.position;
	}

	public void ResetPlayerInitPos(Vector3 pos) {
		player.movementController.setInitPosAndQuat (pos, Quaternion.Euler(0, player.transform.rotation.y, 0));
	}


	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		uicontroller = GameObject.FindWithTag("UIController").GetComponent<UIController>();

		player.gameObject.AddComponent<BuildObjectsPlayer> ();
		player.GetComponent<BuildObjectsPlayer> ().objectToBuild = standartBuildThing;
	}

	void OnDrawGizmosSelected() {
		Color color = Color.blue;
		color.a = 0.4f;
		Gizmos.color = color;
		Gizmos.DrawCube(new Vector3(0, playerHeightToReset, 0), new Vector3(10000, 1, 10000));
	}

	void Update() {
		if (player.transform.position.y < playerHeightToReset) {
			player.movementController.changeState (PlayerMovementStates.Reset);
		}
	}

	public void initPrefs() {
		if(PlayerPrefs.GetInt("prefsInitialized") < 1 ) {
			foreach (string str in initialyTruePlayerPrefVariables) {
				PlayerPrefs.SetInt (str, 1);
			}
		}
	}

}

public class MessageTypes {
	public const int playerFellDown = 0;
}

