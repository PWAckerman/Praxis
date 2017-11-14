using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour, IBurnable {

	public GameObject fire;
	bool burning;
	// Use this for initialization
	void Start () {
		burning = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Burn(){
		if (!burning) {
			burning = true;
			GameObject firee = Instantiate (fire, transform.position, Quaternion.identity);
			firee.transform.parent = transform;
			Destroy (this.gameObject, 3f);
		}
	}

	public bool isBurning(){
		return burning;
	}
}
