using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrifiedBarrier : MonoBehaviour, IElectrifiable {

	bool electrified;
	GameObject electricitySource;
	Animator myAnim;
	// Use this for initialization
	void Start () {
		myAnim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (electricitySource == null) {
			Deelectrify ();
		}
	}

	public void Electrify(GameObject src){
		if (electricitySource == null) {
			src.GetComponent<Electricity> ().AddElectrified (this.gameObject);
			electrified = true;
			electricitySource = src;
			myAnim.SetBool ("electrified", true);
		}
	}

	public void Deelectrify(){
		electrified = false;
		electricitySource = null;
		myAnim.SetBool ("electrified", false);
	}

	public void TurnOffCollider(){
		GetComponent<BoxCollider2D> ().offset = new Vector2 (0f,-0.22f);
	}

	public void TurnOnCollider(){
		GetComponent<BoxCollider2D> ().offset = new Vector2 (0f,0f);
	}


	public bool isElectrified(){
		return electrified;
	}
}
