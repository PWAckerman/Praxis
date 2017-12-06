using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPole : MonoBehaviour, IDamageable {

	// Use this for initialization
	public HingeJoint2D cableConnection1;
	public HingeJoint2D cableConnection2;
	public HingeJoint2D cableConnection3;
	public HingeJoint2D cableConnection4;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(int amount){
		if (cableConnection1 != null) {
			Destroy (cableConnection1);
		}
		if (cableConnection1 == null && cableConnection2 != null) {
			Destroy (cableConnection2);
		}
		if (cableConnection2 == null && cableConnection3 != null) {
			Destroy (cableConnection3);
		}
		if (cableConnection3 == null && cableConnection3 != null) {
			Destroy (cableConnection3);
		}
	}
}
