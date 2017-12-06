using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDisplay : MonoBehaviour {

	public AlertManager alertManager;
	public Text text;
	public Animator myAnim;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		switch (alertManager.currentMode) {
			case AlertMode.HIGH_ALERT:
				//ARED
				text.enabled = false;
				GetComponent<SpriteRenderer>().color = Color.white;
				myAnim.SetInteger("mode", 1);
				break;
			case AlertMode.ALERTED:
				//AYELLOW
				GetComponent<SpriteRenderer>().color = Color.white;
				myAnim.SetInteger ("mode", 2);
				text.enabled = true;
			text.text = "00:" + alertManager.tempCooldown.ToString().PadLeft(2,0.ToString().ToCharArray()[0]);
				break;
			case AlertMode.PATROLLING:
				//ANOSHOW
				text.enabled = false;
				GetComponent<SpriteRenderer>().color = Color.clear;
				myAnim.SetInteger("mode", 0);
				break;
		}
	}
}
