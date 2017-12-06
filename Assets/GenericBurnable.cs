using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBurnable : MonoBehaviour, IBurnable {

	public bool burning;
	public GameObject fire;
	public GameObject dust;

	void Start () {
		burning = false;
	}

	public void Burn(){
		if (!isBurning ()) {
			burning = true;
			StartCoroutine ("StopBurning");
		}
	}

	public bool isBurning(){
		return burning;
	}

	IEnumerator StopBurning(){
		Instantiate (fire, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(3f);
		burning = false;
		Instantiate (dust, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
	}
}
