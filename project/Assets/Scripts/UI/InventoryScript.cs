using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour {

	public List<InventoryItemScript> buildThings;
	public GameObject content;
	public GameObject contentItem;

	void Start () {
		populateForBuilding ();
	}

	public void populateForBuilding() {
		content.transform.DetachChildren ();
		int i = 1;
		foreach (InventoryItemScript f in buildThings)  { 

			GameObject newBtn = Instantiate (contentItem);
			newBtn.GetComponent<InventoryButtonScript>().setInventoryItemScript(f);
			newBtn.transform.SetParent (content.transform);
			newBtn.GetComponent<InventoryButtonScript>().setPositionInPanel(i);
			newBtn.GetComponent<Button>().onClick.AddListener(() => {
				GameObject.FindWithTag("Player").GetComponent<BuildObjectsPlayer>().objectToBuild = f;
				GameObject.FindWithTag("Player").GetComponent<BuildObjectsPlayer>().deactivateBuildMode();
				GameObject.FindWithTag("UIController").GetComponent<UIController>().setBuilding();
			});
			i++;

		}

		setScrollViewContentHeight (i);
	}

	public void setScrollViewContentHeight(int i) {
		content.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, -((60f + 10f) * i)); //BUTON HEIGHT = 60, PADDING = 10
	}
	
	// Update is called once per frame
	void Update () {
	}
}
