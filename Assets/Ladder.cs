using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

	// Use this for initialization
	public GameObject player;
	public BoxCollider2D platformCollider;

	void Start () {
		platformCollider = GetComponents<BoxCollider2D> () [1];
	}
	
	// Update is called once per frame
	void Update () {
			if (Input.GetAxis ("Vertical") < 0) {
				platformCollider.enabled = false;
			}
			else if(Input.GetAxis ("Vertical") >= 0) {
				platformCollider.enabled = true;
			}
		}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			player = coll.gameObject;
			player.SendMessage ("Climbing", this.gameObject);
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		Debug.Log ("exit");
		player.SendMessage ("NotClimbing");
		player = null;
	}
}
