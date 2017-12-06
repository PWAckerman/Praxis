using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour, IBurnable {

	// Use this for initialization
	public float timeToBurn;
	[SerializeField]
	bool burning;
	Animator myAnim;

	void Start () {
		myAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Burn(){
		burning = true;
		myAnim.SetBool ("burning", true);
		StartCoroutine ("StopBurning");
	}

	void OnTriggerStay2D(Collider2D coll){
		if (burning) {
			var burner = coll.gameObject.GetComponent<IBurnable> ();
			if (burner != null) {
				if (!burner.isBurning ()) {
					burner.Burn ();
				}
			}
		}
	}

	public bool isBurning(){
		return burning;
	}

	IEnumerator StopBurning(){
		yield return new WaitForSeconds (timeToBurn);
		myAnim.SetBool ("burning", false);
		burning = false;
	}
}
