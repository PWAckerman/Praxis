using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour {
	public GameObject dustCloud;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Destruct(){
		Debug.Log ("Destruct!");
		Instantiate (dustCloud, new Vector3(transform.position.x + 3f, transform.position.y, transform.position.y), Quaternion.identity);
		Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Grenade") {
			Debug.Log ("it was a projectile");
			Destroy(coll.gameObject);
			Destruct ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Grenade") {
			Debug.Log ("it was a projectile");
			Destroy(coll.gameObject);
			Destruct ();
		}
	}

	void OnDestroy(){
		
	}
}
