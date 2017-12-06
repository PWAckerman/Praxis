using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour{

	public string name;
	public string description;
	public string icon;
	public string projectile;
	public string displayName;
	public int amount;
	public int power;
	public int limit;

	public InventoryGrenade grenade;

	void Start(){
		grenade = new InventoryGrenade ();
		grenade.name = name;
		grenade.description = description;
		grenade.displayName = displayName;
		grenade.icon = icon;
		grenade.projectile = projectile;
		grenade.power = power;
		grenade.amount = amount;
		grenade.limit = limit;
	}
}


public class InventoryGrenade{
	public string name;
	public string description;
	public string displayName;
	public string icon;	
	public string projectile;
	public int power;
	public int amount;
	public int limit;
}