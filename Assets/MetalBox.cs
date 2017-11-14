using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBox : MonoBehaviour, IAttractable, IElectrifiable{

	// Use this for initialization

	Rigidbody2D rb;
	Animator myAnim;
	public GameObject electricitySource;
	float initialMass;
	[SerializeField]
	bool electrified;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		initialMass = this.rb.mass;
		electrified = false;
		electricitySource = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (electricitySource == null) {
			Deelectrify ();
		}
	}

	public void Electrify(GameObject src){
		src.GetComponent<Electricity> ().AddElectrified (this.gameObject);
		electrified = true;
		electricitySource = src;
		myAnim.SetBool ("electrified", true);
	}

	public void Deelectrify(){
		electrified = false;
		electricitySource = null;
		myAnim.SetBool ("electrified", false);
	}

	public bool isElectrified(){
		return electrified;
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && electrified) {
			Debug.Log ("electrify");
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.electricitySource);
		}
		if (coll.gameObject.GetComponent<IElectrocutable>() != null && electrified) {
			Debug.Log ("electrify");
			coll.gameObject.GetComponent<IElectrocutable> ().Electrocute ();
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && electrified) {
			Debug.Log ("electrify");
			coll.gameObject.GetComponent<IElectrifiable> ().Deelectrify ();
		}
	}
		
	public void MoveTowards(Vector3 position){
		Vector3 diff = position - this.transform.position;
		rb.mass = 0.1f;
		rb.AddForce (diff, ForceMode2D.Force);
	}

	public void ResetMass(){
		this.rb.mass = initialMass;
	}
}
