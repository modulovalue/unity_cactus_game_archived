using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchyEffect : MonoBehaviour {

	public List<GameObject> touchEffectSpawnThing;

	[Range(0.0f, 0.2f)]
	public float spawnPause = 0.04f;

	[Range(0.0f, 0.2f)]
	public float offsetDistance = 0.05f;


	[Range(0.0f, 1f)]
	public float colorRange = 0.1f;

	[Range(0.0f, 1f)]
	public float colorVal = 0.2f;

	[Range(0.0f, 1f)]
	public float color2Range = 0.2f;

	[Range(0.0f, 1f)]
	public float color2Val = 0.5f;

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			keepSpawning = true;
			StartCoroutine (spawnMouseTouchEffect ());
		}

		if (Input.GetMouseButtonUp(0)) {
			keepSpawning = false;
		}
	}

	private bool keepSpawning = false;

	public IEnumerator spawnMouseTouchEffect() {
		int i = 0;
		while (keepSpawning) {
			GameObject obj = Instantiate (touchEffectSpawnThing[i++ % 2], Vector3.zero, Quaternion.identity);
			obj.transform.parent = this.transform;
			Vector3 point = Input.mousePosition;

			colorVal += Time.deltaTime / 9;
			colorVal = colorVal % 1;
			obj.GetComponent<Image> ().color = 
				Random.ColorHSV (
					Mathf.Clamp01(colorVal - colorRange), 
					Mathf.Clamp01(colorVal + colorRange), 
					Mathf.Clamp01(color2Val - color2Range), 
					Mathf.Clamp01(color2Val + color2Range), 
					1f, 
					1f);
			
			int screenRelative = (int) ( Screen.height * offsetDistance);
			float randomXOffset = Random.Range (-screenRelative, screenRelative);
			float randomYOffset = Random.Range (-screenRelative, screenRelative);
			obj.transform.localPosition = new Vector3 (point.x - Screen.width / 2 + randomXOffset, point.y - Screen.height / 2 + randomYOffset, 0.5f);
			yield return new WaitForSeconds (spawnPause);
		};
	}
}
