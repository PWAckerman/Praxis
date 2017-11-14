using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduit : MonoBehaviour, IElectrifiable {

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

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && electrified) {
			Debug.Log ("electrify");
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.electricitySource);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && electrified) {
			Debug.Log ("electrify");
			coll.gameObject.GetComponent<IElectrifiable> ().Deelectrify ();
		}
	}

	public void Deelectrify(){
		electrified = false;
		electricitySource = null;
		myAnim.SetBool ("electrified", false);
	}

	public bool isElectrified(){
		return electrified;
	}
}
