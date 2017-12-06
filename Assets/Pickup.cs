using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ICollectable {


	public string popCategory;
	public string popSubCategory;
	public string displayName;
	string name;
	public int amount;
	public string category { get; set;}
	public string subCategory { get; set;}

	void Start(){
		category = popCategory;
		subCategory = popSubCategory;
		name = displayName.Replace ("\\n", "\n");
		name = name.Replace ("\\", "");
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			GetComponentInChildren<TextMesh> ().text = name;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			GetComponentInChildren<TextMesh> ().text = "";
		}
	}
}
