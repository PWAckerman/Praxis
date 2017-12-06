using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour{
	public string name;
	public string description;
	public string displayName;
	public string icon;
	public string projectile;
	public string ammoType;
	public int power;

	public Weapon weapon;

	void Start(){
		weapon = new Weapon ();
		weapon.name = name;
		weapon.displayName = displayName;
		weapon.description = description;
		weapon.icon = icon;
		weapon.projectile = projectile;
		weapon.ammoType = ammoType;
		weapon.power = power;
	}
}