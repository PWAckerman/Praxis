using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

	// Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
		Debug.Log ("LAVA COLLISION");
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage ("Damage", 1);
		}
		if (coll.gameObject.tag == "Grenade") {
			Destroy (coll.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("LAVA TRIGGER");
		if (other.gameObject.tag == "Player") {
			other.gameObject.SendMessage ("Damage", 1);
		}
		if (other.gameObject.tag == "Grenade") {
			Destroy (other.gameObject);
		}
	}
}
