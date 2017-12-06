using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBox : MonoBehaviour {

	TextMesh txt;
	InventoryManager im;
	public SpriteRenderer sr;
	public Weapon currentWeapon;

	void Start () {
		txt = GetComponentInChildren<TextMesh> ();
		im = InventoryManager.GetInstance ();
		if (im.currentWeapon != null) {
			if (im.ammunition.ContainsKey (im.currentWeapon.name)) {
				txt.text = "x" + im.GetCurrentAmmunitionAmount ().ToString ();
			}
			sr.sprite = Resources.Load<Sprite>(im.currentWeapon.name);
		} else {
			sr.sprite = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (im.currentWeapon != null) {
			Debug.Log ("NOT NULL GUNBOX");
			if (im.ammunition.ContainsKey (im.currentWeapon.name)) {
				Debug.Log (im.GetCurrentAmmunitionAmount ());
				txt.text = "x" + im.GetCurrentAmmunitionAmount ().ToString ();
			}
			sr.sprite = Resources.Load<Sprite>(im.currentWeapon.name);
		} 
	}
}
