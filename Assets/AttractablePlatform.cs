using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractablePlatform : MonoBehaviour, IAttractable{

	Rigidbody2D rb;
	float initialMass;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.centerOfMass = Vector2.zero;
		rb.inertia = 1f;
		initialMass = rb.mass;
	}
	
	// Update is called once per frame
	void Update () {
		
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
