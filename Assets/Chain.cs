using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chain : MonoBehaviour, IElectrifiable, IAttractable {

	// Use this for initialization
	Rigidbody2D rb;
	public GameObject electricitySource;
	Animator myAnim;
	public int chainTag;
	float initialMass;
	[SerializeField]
	bool electrified;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.centerOfMass = Vector2.zero;
		rb.inertia = 1f;
		myAnim = GetComponent<Animator> ();
		initialMass = rb.mass;
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
		if (electricitySource == null) {
			src.GetComponent<Electricity> ().AddElectrified (this.gameObject);
			electrified = true;
			electricitySource = src;
			myAnim.SetBool ("electrified", true);
			Collider2D[] results = new Collider2D[5];
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && this.electrified) {
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.electricitySource);
		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && this.electrified) {
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.electricitySource);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && this.electrified) {
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

	public void MoveTowards(Vector3 position){
		Vector3 diff = position - this.transform.position;
		rb.mass = 0.1f;
		rb.AddForce (diff, ForceMode2D.Force);
	}

	public void ResetMass(){
		this.rb.mass = initialMass;
	}
}
