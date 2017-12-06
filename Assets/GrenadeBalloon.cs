using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrenadeBalloon : MonoBehaviour, IBurnable, IElectrocutable, IDamageable, IExplodeable, IAttachReceivable {

	Animator myAnim;
	bool burning;
	bool popping;
	public GameObject explosion;
	public GameObject dust;
	public GameObject currentAttached;
	public GameObject realAttachPoint;
	public GameObject attachPoint { get; set;}
	// Use this for initialization
	void Start () {
		myAnim = GetComponent<Animator> ();
		attachPoint = realAttachPoint;
	}

	// Update is called once per frame
	void Update () {

	}

	public void Explode(){
		if (!popping) {
			Pop ();
			Instantiate (explosion, transform.position, Quaternion.identity);
			Instantiate (dust, transform.position, Quaternion.identity);
		}
	}

	public void Burn(){
		burning = true;
		Explode ();
	}

	public void Damage(int amt){
		Explode ();
	}

	public bool isBurning(){
		return burning;
	}

	public void Electrocute(){
		Pop ();
	}

	void Pop(){
		popping = true;
		myAnim.SetBool ("popping", true);
	}

	void Die(){
		Destroy (this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll){
//		Debug.Log ("GRENADE BALLOON COLLISION");
//		Debug.Log (coll.gameObject.name);
//		if (coll.gameObject.GetComponent<IAttachable> () != null) {
//			ContactPoint2D[] contacts = new ContactPoint2D[5];
//			coll.GetContacts (contacts);
//			coll.gameObject.GetComponent<IAttachable> ().Attach (attachPoint, contacts [0].point);
//			currentAttached = coll.gameObject;	
//		}
	}

	void OnCollisionStay2D(Collision2D coll){
//		Debug.Log ("GRENADE BALLOON COLLISION");
//		if (coll.otherCollider.gameObject.GetComponent<IAttachable> () != null) {
//			ContactPoint2D[] contacts = new ContactPoint2D[5];
//			coll.GetContacts (contacts);
//			coll.otherCollider.gameObject.GetComponent<IAttachable> ().Attach (attachPoint, contacts [0].point);
//			currentAttached = coll.otherCollider.gameObject;	
//		}
	}
}
