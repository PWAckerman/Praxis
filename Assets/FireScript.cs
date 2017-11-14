using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour {

	public float timeToLive;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, timeToLive);
		StartCoroutine ("Flash");
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
	}

	public IEnumerator Flash(){
		Vector4 tempColor = GetComponent<SpriteRenderer> ().color;
		yield return new WaitForSeconds(timeToLive / 2f);
		while(true){
			GetComponent<SpriteRenderer> ().color = new Vector4 (tempColor.x, tempColor.y, tempColor.z, 1f);
			yield return new WaitForSeconds (0.1f);
			GetComponent<SpriteRenderer> ().color = new Vector4 (tempColor.x, tempColor.y, tempColor.z, 0.5f);
			yield return new WaitForSeconds (0.1f);
		}
	}
}
