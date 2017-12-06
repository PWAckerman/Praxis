using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrifiedWheel : MonoBehaviour, IElectrifiable{

	// Use this for initialization
	[SerializeField]
	bool electrified;
	GameObject electricitySource;
	HingeJoint2D joint;
	public float motorSpeed;
	// Use this for initialization
	void Start () {
		joint = GetComponent<HingeJoint2D> ();
		joint.useMotor = false;
		JointMotor2D motor = joint.motor;
		motor.motorSpeed = motorSpeed;
		joint.motor = motor;
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
		joint.useMotor = true;
		electricitySource = src;
	}

	public void Deelectrify(){
		electrified = false;
		joint.useMotor = false;
		electricitySource = null;
	}

	public bool isElectrified(){
		return electrified;
	}
}
