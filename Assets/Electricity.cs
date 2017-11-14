using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Electricity : MonoBehaviour, ISwitchable {

	public HashSet<GameObject> electrified;
	public bool on { get; set;}
	// Use this for initialization
	void Start () {
		on = true;
		electrified = new HashSet<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOff(){
		on = false;
		gameObject.SetActive (false);
	}

	public void TurnOn(){
		on = true;
		gameObject.SetActive (true);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z), 1f);
	}

	public void AddElectrified(GameObject obj){
		electrified.Add(obj);
	}

	public void RemoveElectricity(){
		electrified.ToList ().ForEach (obj => obj.GetComponent<IElectrifiable> ().Deelectrify ());
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null) {
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null) {
			coll.gameObject.GetComponent<IElectrifiable> ().Electrify (this.gameObject);
		}
		if (coll.gameObject.GetComponent<IElectrocutable>() != null) {
			coll.gameObject.GetComponent<IElectrocutable> ().Electrocute ();
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IElectrifiable>() != null) {
			RemoveElectricity ();
		}
	}
}
