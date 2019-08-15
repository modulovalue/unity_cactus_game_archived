using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeCanvasManager : MonoBehaviour {

	public Text title;
	public Text description;

	public MVProgressBar movementSpeed;

	public MVProgressBar turnSpeed;

	public Button previousBtn;
	public Button nextBtn;

	// Use this for initialization
	void Start () {

		title.text = "General Stone";
		description.text = "You are strong blabla bla";

		movementSpeed.outerText.text = "Movement Speed";
		turnSpeed.outerText.text = "Turning Speed";

		movementSpeed.setValue(50, 200);
		turnSpeed.setValue (35, 200);

	}

	public void nextCostume() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStyleController> ().nextCostume(this);

	}

	public void previousCostume() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStyleController> ().previousCostume(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
