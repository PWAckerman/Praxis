using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBarried : MonoBehaviour, ISwitchable {

	public bool on{ get; set;}
	AudioManager am;
	// Use this for initialization

	void Start () {
		on = true;
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOff(){
		on = false;
		transform.Find ("Laser").gameObject.SetActive (false);
		am.Play ("laserOff");
		GetComponent<BoxCollider2D> ().enabled = false;
	}

	public void TurnOn(){
		on = true;
		transform.Find ("Laser").gameObject.SetActive (true);
		am.Play ("laserOn");
		GetComponent<BoxCollider2D> ().enabled = true;
	}

	void OnCollisionStay2D(Collision2D coll){
		coll.gameObject.SendMessage ("Damage", 5);
		if (coll.gameObject.tag == "Grenade") {
			Destroy (coll.gameObject);
		}
	}
}
