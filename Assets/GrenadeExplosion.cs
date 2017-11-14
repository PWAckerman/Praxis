using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour {

	// Use this for initialization
	public float finalRadius;
	public float increaseSpeed;
	public CircleCollider2D collider;
	public float rad;

	void Start () {
		GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera");
		cam.GetComponent<CameraFollows> ().ShakeCamera ();
	}

	// Update is called once per frame
	void Update () {
		rad = collider.radius;
		if (collider.radius < finalRadius) {
			// The size is smaller than our final size
			// Increase the size a little bit
			rad += 1 * increaseSpeed * Time.deltaTime;
			// Check if we overshot our final size
			if (rad > finalRadius) {
				// The size is now bigger than the final size
				// Set the size to the final size
				rad = finalRadius;
			}
			// Update the collider with the new size
			collider.radius = rad;
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		Debug.Log (coll.tag);
		coll.gameObject.SendMessage("Damage", 2);
		coll.gameObject.SendMessage ("Destruct");
	}
		

	void Die(){
		Destroy (this.gameObject);
	}
}
