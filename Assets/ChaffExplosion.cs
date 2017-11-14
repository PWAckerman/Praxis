using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaffExplosion : MonoBehaviour {

	// Use this for initialization
	List<GameObject> electronics;

	void Start () {
		electronics = new List<GameObject> ();
		GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ().LoopWithLocalAudioSource(GetComponent<AudioSource>(), "water");
		Destroy (this.gameObject, 20f);
	}

	void OnDestroy(){
		electronics.ForEach (obj => obj.SendMessage ("ResumePatrol"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Drone") {
			electronics.Add (coll.gameObject);
		}
	}
}
