using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFlow : MonoBehaviour, ISwitchable {

	public bool on{ get; set;}
	public bool initialState;
	public GameObject lavaContainer;
	public Animator myAnim;
	// Use this for initialization
	void Start () {
		on = initialState;
		if (on) {
			TurnOn ();
		} else if (!on) {
			TurnOff ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void TurnOff(){
		on = false;
		myAnim.SetBool ("on", false);
		lavaContainer.SetActive (false);
		GetComponent<BoxCollider2D> ().enabled = false;
	}

	public void TurnOn (){
		on = true;
		myAnim.SetBool ("on", true);
		lavaContainer.SetActive (true);
		GetComponent<BoxCollider2D> ().enabled = true;
	}

	void OnTriggerStay2D(Collider2D coll){
		var burner = coll.gameObject.GetComponent<IBurnable> ();
		if (burner != null) {
			if (!burner.isBurning ()) {
				burner.Burn ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		var burner = coll.gameObject.GetComponent<IBurnable> ();
		if (burner != null) {
			if (!burner.isBurning ()) {
				burner.Burn ();
			}
		}
	}
}
