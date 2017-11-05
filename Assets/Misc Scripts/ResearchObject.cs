using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ResearchObjectRepresentation{
	public string name;
	public string icon;
	public string description;
	public string action;

	public ResearchObjectRepresentation(string n, string i, string desc, string act){
		name = n;
		icon = i;
		description = desc;
		action = act;
	}

	public static ResearchObjectRepresentation CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<ResearchObjectRepresentation>(jsonString);
	}
}

[Serializable]
public class ResearchObjects{
	public List<ResearchObjectRepresentation> ros;

	public static ResearchObjects CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<ResearchObjects>(jsonString);
	}
}


public class ResearchObject{

	public enum ResearchIcon{
		GOLD,
		FACTORY,
		ENERGY,
		EQUIPMENT,
		MINING,
		ABILITY,
		TRANSPORTATION,
		WORKFORCE,
		ENEMY
	}
		
	public static Dictionary<string, ResearchIcon> researchMap = new Dictionary<string, ResearchIcon>{
		{"GOLD", ResearchIcon.GOLD},
		{"FACTORY", ResearchIcon.FACTORY},
		{"ENERGY", ResearchIcon.ENERGY},
		{"EQUIPMENT", ResearchIcon.EQUIPMENT},
		{"MINING", ResearchIcon.MINING},
		{"ABILITY", ResearchIcon.ABILITY},
		{"TRANSPORTATION", ResearchIcon.TRANSPORTATION},
		{"WORKFORCE", ResearchIcon.WORKFORCE},
		{"ENEMY", ResearchIcon.ENEMY}
	};

	public static Dictionary<ResearchIcon, string> assetMap = new Dictionary<ResearchIcon, string>{
		{ResearchIcon.GOLD, "Assets/Sprites/Loot/Coin"},
		{ResearchIcon.FACTORY, "Assets/Sprites/Loot/Factory"},
		{ResearchIcon.ENERGY, "Assets/Sprites/Loot/Generator"},
		{ResearchIcon.EQUIPMENT, "Assets/Sprites/Loot/InventoryBox"},
		{ResearchIcon.MINING, "Assets/Sprites/Loot/MinerA"},
		{ResearchIcon.ABILITY, "Assets/Sprites/Loot/Workforce"},
		{ResearchIcon.TRANSPORTATION, "Assets/Sprites/Loot/Distribution"},
		{ResearchIcon.WORKFORCE, "Assets/Sprites/Loot/Workforce"},
		{ResearchIcon.ENEMY, "Assets/Sprites/Loot/Factory"}
	};

	public static Dictionary<ResearchIcon, string> reverseMap = new Dictionary<ResearchIcon, string>{
		{ResearchIcon.GOLD, "GOLD"},
		{ResearchIcon.FACTORY, "FACTORY"},
		{ResearchIcon.ENERGY, "ENERGY"},
		{ResearchIcon.EQUIPMENT, "EQUIPMENT"},
		{ResearchIcon.MINING, "MINING"},
		{ResearchIcon.ABILITY, "ABILITY"},
		{ResearchIcon.TRANSPORTATION, "TRANSPORTATION"},
		{ResearchIcon.WORKFORCE, "WORKFORCE"},
		{ResearchIcon.ENEMY, "ENEMY"}
	};

	public string name;
	public ResearchIcon icon;
	public string description;
	public string action;

	public static ResearchObject FromJson(string rep){
		ResearchObjectRepresentation rp = (ResearchObjectRepresentation)JsonUtility.FromJson (rep, typeof(ResearchObjectRepresentation));
		return new ResearchObject(rp.name, researchMap[rp.icon], rp.description, "act");
	}

	public static List<ResearchObject> FromJsonArray(string rep){
		ResearchObjects lst = JsonUtility.FromJson <ResearchObjects>(rep);
		List<ResearchObjectRepresentation> reps = lst.ros;
		List<ResearchObject> objs = new List<ResearchObject> ();
		reps.ToList<ResearchObjectRepresentation>().ForEach(rp => {
			objs.Add(new ResearchObject(rp.name, researchMap[rp.icon], rp.description, "act"));
		});
		return objs;
	}

	public static List<ResearchObject> LoadAllFromFile(){
		StreamReader reader = new StreamReader("Assets/ResearchList.json"); 
		string file = reader.ReadToEnd();
		reader.Close();
		List<ResearchObjectRepresentation> reps = ResearchObjects.CreateFromJSON (file).ros;
		List<ResearchObject> objs = new List<ResearchObject> ();
		reps.ForEach(rp => {
			objs.Add(new ResearchObject(rp.name, researchMap[rp.icon], rp.description, "act"));
		});
		return objs;
	}

	public ResearchObject(string n, ResearchIcon i, string desc, string act){
		name = n;
		icon = i;
		description = desc;
		action = act;
	}

	public string ToJson(){
		return JsonUtility.ToJson (new ResearchObjectRepresentation (name, reverseMap[icon], description, "act"));
	}
		
}
