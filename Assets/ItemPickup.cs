using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour{
	public string name;
	public string displayName;
	public string icon;
	public string description;

	public Item item;

	void Start(){
		item = new Item ();
		item.name = name;
		item.displayName = displayName;
		item.description = description;
		item.icon = icon;
	}
}

public class Item{
	public string name;
	public string displayName;
	public string icon;
	public string description;
}
