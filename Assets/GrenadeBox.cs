using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBox : MonoBehaviour {

	// Use this for initialization
	TextMesh txt;
	InventoryManager im;
	public SpriteRenderer sr;

	void Start () {
		txt = GetComponentInChildren<TextMesh> ();
		im = InventoryManager.GetInstance ();
		if (im.currentGrenade != null) {
			if (im.grenades.ContainsKey (im.currentGrenade.name)) {
				txt.text = "x" + im.GetCurrentGrenadeAmount ();
			}
			sr.sprite = Resources.Load<Sprite>(im.currentGrenade.name);
		} else {
			sr.sprite = null;
		}
	}

	// Update is called once per frame
	void Update () {
		if (im.currentGrenade != null) {
			Debug.Log ("NOT NULL GRENADEBOX");
			if (im.grenades.ContainsKey (im.currentGrenade.name)) {
				txt.text = "x" + im.GetCurrentGrenadeAmount ();
			}
			Debug.Log (im.currentGrenade.name);
			sr.sprite = Resources.Load<Sprite>(im.currentGrenade.name);
		} 
	}
}

