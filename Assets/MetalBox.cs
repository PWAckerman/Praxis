using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBox : MonoBehaviour, IAttractable, IElectrifiable, IPickupable{

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

	public void Pickup(GameObject picker){
		picker.SendMessage ("Pickup", this.gameObject);
	}

	public GameObject GetGameObject(){
		return gameObject;
	}

	public void Throw (bool direction, float force){
		Debug.Log ("THROW");
		transform.parent = null;
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.constraints = RigidbodyConstraints2D.None;
		rb.velocity = new Vector2 ( direction ? force : -force, force);
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
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.electricitySource);
		}
		if (coll.gameObject.GetComponent<IElectrocutable>() != null && electrified) {
			coll.gameObject.GetComponent<IElectrocutable> ().Electrocute ();
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null && electrified) {
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
