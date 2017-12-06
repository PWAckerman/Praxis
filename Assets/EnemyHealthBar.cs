using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

	public GameObject HealthBarBackground;
	public GameObject Enemy;
	IEnemy enemy;
	bool flipped;
	Vector3 originalScale;
	int hitPoints;
	int maxHitPoints;
	// Use this for initialization
	void Start () {
		originalScale = HealthBarBackground.transform.localScale;
		enemy = Enemy.GetComponent<IEnemy> ();
		hitPoints = enemy.GetCurrentHitPoints();
		maxHitPoints = enemy.GetMaxHitPoints ();
		CalculateWidth ();
	}
	
	// Update is called once per frame
	void Update () {
		IsEnemyFlipped ();
		CalculateWidth ();
//		CalculateOffset ();
	}

	void IsEnemyFlipped(){
		flipped = Enemy.transform.localScale.x < 0f;
		Vector3 currentPosition = HealthBarBackground.transform.localPosition;
//		if (flipped) {
//			HealthBarBackground.transform.localPosition = new Vector3 (-currentPosition.x, currentPosition.y, currentPosition.z);
//		}
	}

	void CalculateWidth(){
		hitPoints = enemy.GetCurrentHitPoints();
		float ratio = (float)hitPoints / maxHitPoints;
		Debug.Log (ratio);
		Vector3 currentScale = HealthBarBackground.transform.localScale;
		if (ratio < 0) {
			ratio = 0f;
		}
		HealthBarBackground.transform.localScale = new Vector3 (ratio, currentScale.y, currentScale.z);
	}

	void CalculateOffset(){
		Vector3 currentScale = HealthBarBackground.transform.localScale;
		float dec = currentScale.x * 0.1f;
		Vector3 currentPosition = HealthBarBackground.transform.localPosition;
		if (!flipped) {
			HealthBarBackground.transform.localPosition = new Vector3 (-(0.109f - dec), currentPosition.y, currentPosition.z);
		} else {
			HealthBarBackground.transform.localPosition = new Vector3 ((0.109f - dec), currentPosition.y, currentPosition.z);
		}
	}
		
}
