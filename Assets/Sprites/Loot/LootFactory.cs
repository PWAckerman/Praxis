using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public static class EnumExtensions
{
	public static Enum GetRandomEnumValue(this Type t)
	{
		return Enum.GetValues(t)          // get values from Type provided
			.OfType<Enum>()               // casts to Enum
			.OrderBy(e => Guid.NewGuid()) // mess with order of results
			.FirstOrDefault();            // take first item in result
	}
}

public enum LootTypes
{
	A,
	B,
	C,
	D
};

public class LootFactory {


	// Use this for initialization
	public static Loot GetLoot(LootTypes knd, MonoBehaviour par){
		return new Loot (knd, par);
	}

	public static Loot GetRandomLoot(MonoBehaviour par, GameObject go){
		LootTypes lootT = (LootTypes)typeof(LootTypes).GetRandomEnumValue ();
		GameObject go2 = new GameObject ();
		Loot loot = go2.AddComponent<Loot>();
		loot.Init (lootT, par);
		return loot;
	}
	// Update is called once per frame
	public static Loot[] GetAssortedRandomLoot(int amount, MonoBehaviour par, GameObject go ){
		Loot[] lootCollection = new Loot[amount];
		for (var i = 0; i < amount; i++) {
			lootCollection [i] = GetRandomLoot (par,go);
		}
		return lootCollection;
	}
}
