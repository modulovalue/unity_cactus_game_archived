using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVProgressBar : MonoBehaviour {

	public Text innerText;
	public Text outerText;
	public Image progressImage;
	// Use this for initialization
	void Start () {
		
	}

	public void setValue(float value, float max) {
		progressImage.fillAmount = Mathf.Clamp01 (value/max);
		setInnerText (value+"");
	}

	public void setInnerText(string str) {
		innerText.text = str;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
