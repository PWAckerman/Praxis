using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {

	// Use this for initialization
	InventoryManager im;
	public SpriteRenderer sr;

	void Start () {
		im = InventoryManager.GetInstance ();
		if (im.currentItem != null) {
			sr.sprite = Resources.Load<Sprite>(im.currentItem.name);
		} else {
			sr.sprite = null;
		}
	}

	// Update is called once per frame
	void Update () {
		if (im.currentItem != null) {
			sr.sprite = Resources.Load<Sprite>(im.currentItem.name);
		} 
	}
}
