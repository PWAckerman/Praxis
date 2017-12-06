using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour, IBurnable, IElectrocutable, IDamageable, IAttachReceivable {

	Animator myAnim;
	bool burning;
	public GameObject attachPoint { get; set;}
	// Use this for initialization
	void Start () {
		myAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Burn(){
		burning = true;
		Pop ();
	}

	public void Damage(int amt){
		Pop ();
	}

	public bool isBurning(){
		return burning;
	}

	public void Electrocute(){
		Pop ();
	}

	void Pop(){
		myAnim.SetBool ("popping", true);
	}

	void Die(){
		Destroy (this.gameObject);
	}
}
