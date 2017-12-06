using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenBar : MonoBehaviour {

	public GameObject BarBackground;
	public PlayerController player;
	Vector3 originalScale;
	float oxygen;
	float oxygenCapacity;
	// Use this for initialization
	void Start () {
		originalScale = BarBackground.transform.localScale;
		CalculateWidth ();
	}

	// Update is called once per frame
	void Update () {
		CalculateWidth ();
//		CalculateOffset ();
	}

	void CalculateWidth(){
		oxygen = (float)player.oxygen;
		float ratio = oxygen / player.oxygenCapacity;
		Debug.Log (ratio);
		Vector3 currentScale = BarBackground.transform.localScale;
		if (ratio < 0) {
			ratio = 0f;
		}
		BarBackground.transform.localScale = new Vector3 (ratio, currentScale.y, currentScale.z);
	}

	void CalculateOffset(){
		Vector3 currentScale = BarBackground.transform.localScale;
		float dec = currentScale.x * 0.1f;
		Vector3 currentPosition = BarBackground.transform.localPosition;
		BarBackground.transform.localPosition = new Vector3 (-(0.105f - dec), currentPosition.y, currentPosition.z);
	}

}