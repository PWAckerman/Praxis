using UnityEngine;
using System.Collections;

public class BombableWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.gameObject.layer == 9) {
			Debug.Log ("it was a player");
		}
		if (coll.collider.gameObject.layer == 11) {
			Debug.Log ("it was a bomb");
			coll.collider.gameObject.GetComponent<IExplodeable> ().Explode ();
			Destroy (this.gameObject);
		}

	}
}
