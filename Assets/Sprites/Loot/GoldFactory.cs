using UnityEngine;
 
using System;
using System.Collections;
using System.Linq;

public class GoldFactory {


	public static Gold GetRandomGold(MonoBehaviour par, GameObject go){
		GameObject go2 = new GameObject ();
		Gold gold = go2.AddComponent<Gold>();
		gold.Init (par);
		return gold;
	}

	public static Gold[] GetAssortedGold(int amount, MonoBehaviour par, GameObject go ){
		Gold[] goldCollection = new Gold[amount];
		for (var i = 0; i < amount; i++) {
			goldCollection [i] = GetRandomGold (par, go);
		}
		return goldCollection;
	}
}
