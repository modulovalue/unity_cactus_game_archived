using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonScript : MonoBehaviour {

	public Image image;
	public Text title;
	public Text description;
	public InventoryItemScript inventoryItem;

	// Use this for initialization
	void Start () {
		
	}

	public void setImage(Sprite sprite) {
		image.sprite = sprite;
	}

	public void setTitle(string t) {
		title.text = t;
	}

	public void setDescription(string d) {
		description.text = d;
	}

	public void setPositionInPanel(int number) {
		GetComponent<RectTransform> ().offsetMin = new Vector2 (10, -70 * number);
		GetComponent<RectTransform> ().offsetMax = new Vector2 (-10, -70 * number + 60);
		GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
	}

	public void setInventoryItemScript(InventoryItemScript f) {
		this.inventoryItem = f;
		this.setImage(f.image);
		this.setTitle(f.title);
		this.setDescription(f.description);
	}

}
