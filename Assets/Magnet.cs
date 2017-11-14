using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Magnet : MonoBehaviour, IElectrifiable {

	// Use this for initialization
	[SerializeField]
	bool electrified;
	public GameObject electricitySource;
	public HashSet<GameObject> attractees;

	void Start () {
		attractees = new HashSet<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (electricitySource == null) {
			Deelectrify ();
		}
		if (!isElectrified ()) {
			attractees.ToList().ForEach ((go) => go.GetComponent<IAttractable> ().ResetMass ());
			attractees.Clear ();
		}
	}

	public void Electrify(GameObject src){
		src.GetComponent<Electricity> ().AddElectrified (this.gameObject);
		electricitySource = src;
		electrified = true;
	}

	public void Deelectrify(){
		electricitySource = null;
		electrified = false;
	}

	public bool isElectrified(){
		return electrified;
	}

	void OnTriggerStay2D(Collider2D coll){
		if (isElectrified ()) {
			if (coll.gameObject.GetComponent<IAttractable> () != null) {
				coll.gameObject.GetComponent<IAttractable> ().MoveTowards (transform.position);
				attractees.Add (coll.gameObject);
			}
		}
		if (!isElectrified ()) {
			if (coll.gameObject.GetComponent<IAttractable> () != null) {
				coll.gameObject.GetComponent<IAttractable> ().ResetMass ();
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IAttractable> () != null) {
			coll.gameObject.GetComponent<IAttractable> ().ResetMass ();
		}
	}
}
