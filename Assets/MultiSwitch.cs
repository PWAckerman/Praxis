using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SwitchableState{
	ON,
	OFF
}

public class MultiSwitch : MonoBehaviour {

	// Use this for initialization
	public GameObject[] switchables;

	public Sprite onSprite;
	public Sprite offSprite;
	bool playerIsPresent;
	SwitchableState state;

	void Start () {
		playerIsPresent = false;
	} 

	// Update is called once per frame
	void Update () {
		if (playerIsPresent && Input.GetKeyDown(KeyCode.Joystick1Button17) && state == SwitchableState.ON) {
			switchables.ToList ().ForEach ((swch) => {
				swch.GetComponent<ISwitchable> ().TurnOff ();
			});
			state = SwitchableState.OFF;
			GetComponent<SpriteRenderer> ().sprite = offSprite;
		} else if (playerIsPresent && Input.GetKeyDown(KeyCode.Joystick1Button17)  && state == SwitchableState.OFF) {
			switchables.ToList ().ForEach ((swch) => {
				swch.GetComponent<ISwitchable> ().TurnOn ();
			});
			state = SwitchableState.ON;
			GetComponent<SpriteRenderer> ().sprite = onSprite;
		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			playerIsPresent = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			playerIsPresent = false;
		}
	}
}