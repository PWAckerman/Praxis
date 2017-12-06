using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPit : MonoBehaviour {

	public Animator myAnim;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
		

	void OnTriggerStay2D(Collider2D coll){
		var burner = coll.gameObject.GetComponent<IBurnable> ();
		if (burner != null) {
			if (!burner.isBurning ()) {
				burner.Burn ();
			}
		}
		coll.gameObject.SendMessage ("Damage", 1);
	}

	void OnTriggerEnter2D(Collider2D coll){
		var burner = coll.gameObject.GetComponent<IBurnable> ();
		if (burner != null) {
			if (!burner.isBurning ()) {
				burner.Burn ();
			}
		}
		coll.gameObject.SendMessage ("Damage", 1);
	}
}

