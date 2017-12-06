using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager{

	static public InventoryManager instance;
	public ItemPanel panel;
	public Dictionary<string, Weapon> weapons;
	public Dictionary<string, Ammunition> ammunition;
	public Dictionary<string, InventoryGrenade> grenades;
	public HashSet<Item> items;
	public int unionContracts; 
	public int healthContainers;
	public Weapon currentWeapon;
	public InventoryGrenade currentGrenade;
	public Item currentItem;
	public List<string> weaponIndex;
	public List<string> grenadeIndex;
	int currentIndex;
	int currentGrenadeIndex;
	int currentItemIndex;

	public static InventoryManager GetInstance(){
		if (instance != null) {
			return instance;
		} else {
			instance = new InventoryManager ();
			return instance;
		}
	}

	public InventoryManager(){
		weapons = new Dictionary<string, Weapon> ();
		ammunition = new Dictionary<string, Ammunition> ();
		grenades = new Dictionary<string, InventoryGrenade> ();
		weaponIndex = new List<string> ();
		grenadeIndex = new List<string> ();
		items = new HashSet<Item> ();
		panel = GameObject.FindObjectOfType<ItemPanel> ();
		panel.gameObject.SetActive (false);
	}

	public bool HasWeapon(){
		return weaponIndex.Count > 0f;
	}

	public void AddWeapon(Weapon weapon){
		if (!weapons.ContainsKey (weapon.name)) {
			weapons.Add (weapon.name, weapon);
			panel.name.text = weapon.displayName;
			panel.description.text = weapon.description;
			panel.Show (weapon.name);
			weaponIndex.Add (weapon.name);
			currentIndex = weaponIndex.Count - 1;
			currentWeapon = weapons [weaponIndex [currentIndex]];
		}
	}

	public void AddItem(Item item){
		if (!items.Contains (item)) {
			panel.name.text = item.displayName;
			panel.description.text = item.description;
			panel.Show (item.name);
		}
		items.Add (item);
		currentItemIndex = items.Count - 1;
		currentItem = items.ToList() [currentItemIndex];
	}

	public void NextItem(){
		currentItemIndex++;
		if (currentItemIndex > items.Count - 1) {
			currentItemIndex = 0;
		}
		currentItem = items.ToList() [currentItemIndex];
	}

	public void PrevItem(){
		currentItemIndex++;
		if (currentItemIndex < 0) {
			currentItemIndex = items.Count - 1;
		}
		currentItem = items.ToList() [currentItemIndex];
	}

	public void RemoveItem(Item item){
		items.Remove (item);
		currentItemIndex = items.Count - 1;
		currentItem = items.ToList() [currentItemIndex];
	}

	public void AddGrenade(InventoryGrenade grenade){
		if (!grenades.ContainsKey (grenade.name)) {
			grenades.Add (grenade.name, grenade);
			grenadeIndex.Add (grenade.name);
			panel.name.text = grenade.displayName;
			panel.description.text = grenade.description;
			panel.Show (grenade.name);
			currentGrenadeIndex = grenadeIndex.Count - 1;
			currentGrenade = grenades [grenadeIndex [currentGrenadeIndex]];
			Debug.Log ("GRENADES");
			Debug.Log (grenades.Count);
		} else {
			if (grenades[grenade.name].limit < grenades[grenade.name].amount + grenade.amount) {
				grenades [grenade.name].amount = grenades [grenade.name].limit;
			} else {
				grenades [grenade.name].amount += grenade.amount;
			}
		}
	}

	public int GetCurrentGrenadeAmount(){
		return currentGrenade.amount;
	}

	public void NextGrenade(){
		currentGrenadeIndex++;
		if (currentGrenadeIndex > grenadeIndex.Count - 1) {
			currentGrenadeIndex = 0;
		}
		currentGrenade = grenades [grenadeIndex [currentGrenadeIndex]];
	}

	public void PrevGrenade(){
		currentGrenadeIndex--;
		if (currentGrenadeIndex < 0) {
			currentGrenadeIndex = grenadeIndex.Count - 1;
		}
		currentGrenade = grenades [grenadeIndex [currentGrenadeIndex]];
	}

	public void AddHealthContainer(){
		healthContainers++;
	}

	public void AddUnionContracts(){
		unionContracts++;
	}

	public void SetCurrentWeapon(Weapon weapon){
		currentIndex = weaponIndex.FindIndex (x => x.StartsWith(weapon.name));
		currentWeapon = weapons [weaponIndex [currentIndex]];
	}

	public void NextWeapon(){
		currentIndex++;
		if (currentIndex > weaponIndex.Count - 1) {
			currentIndex = 0;
		}
		currentWeapon = weapons [weaponIndex [currentIndex]];
	}

	public void PrevWeapon(){
		currentIndex--;
		if (currentIndex < 0) {
			currentIndex = weaponIndex.Count - 1;
		}
		currentWeapon = weapons [weaponIndex [currentIndex]];
	}

	public void AddAmmunition(Ammunition ammo){
		if (weapons.ContainsKey (ammo.weaponName)) {
			if (ammunition.ContainsKey (ammo.weaponName)) {
				if (ammunition [ammo.weaponName].limit < ammunition [ammo.weaponName].amount + ammo.amount) {
					ammunition [ammo.weaponName].amount = ammunition [ammo.weaponName].limit;
				} else {
					ammunition [ammo.weaponName].amount += ammo.amount;
				}
			} else {
				ammunition.Add (ammo.weaponName, ammo);
			}
		}
	}

	public int GetCurrentAmmunitionAmount(){
		return ammunition [currentWeapon.name].amount;
	}

	public bool HasAmmunition(){
		return ammunition [currentWeapon.name].amount > 0;
	}

	public bool HasGrenades(){
		return currentGrenade.amount > 0;
	}

	public void SubtractAmmunition(string weaponName){
		ammunition [weaponName].amount--;
	}

	public void SubtractAmmunition(){
		Debug.Log (currentWeapon.name);
		ammunition [currentWeapon.name].amount--;
	}

	public void SubtractGrenade(){
		currentGrenade.amount--;
	}

	public int GetAmmunitionAmount(string weaponName){
		return ammunition [weaponName].amount;
	}
}