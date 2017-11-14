using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager{
	public List<Weapon> weapons;
	public List<Item> item;
	public int unionContracts; 
	public int healthContainers;

	public void Load(){
		
	}
}

public class Weapon{
	public string name;
	public string description;
	public Image icon;
	public string projectile;
	public string ammoType;
	public int power;
}

public static class Weapons{
	public static Dictionary<string, Image> Images = new Dictionary<string, Image> ();
}
	

public class Item{
	public string name;
	public Image icon;
	public string description;
}