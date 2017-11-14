using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	// Use this for initialization
	public GameObject switchable;

	public Sprite onSprite;
	public Sprite offSprite;
	bool playerIsPresent;

	void Start () {
		playerIsPresent = false;
	} 
	
	// Update is called once per frame
	void Update () {
		if (playerIsPresent && switchable.GetComponent<ISwitchable>().on == true && Input.GetKeyDown(KeyCode.Joystick1Button1)) {
			switchable.GetComponent<ISwitchable>().TurnOff ();
			GetComponent<SpriteRenderer> ().sprite = offSprite;
		} else if (playerIsPresent && switchable.GetComponent<ISwitchable>().on == false && Input.GetKeyDown(KeyCode.Joystick1Button1)) {
			switchable.GetComponent<ISwitchable>().TurnOn ();
			GetComponent<SpriteRenderer> ().sprite = onSprite;
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
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
