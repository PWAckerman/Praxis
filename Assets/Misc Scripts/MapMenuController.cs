﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuController : MonoBehaviour {
	public ResourceManager rm;
	public PauseMenuManager pm;
	// Use this for initialization
	void Start () {
		rm = ResourceManager.getResourceManager();
		pm = PauseMenuManager.getInstance ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Joystick1Button4) && pm.currentMenu == PauseMenuManager.Menu.MAP){
			pm.getPrevState ();
		}
		if(Input.GetKeyDown(KeyCode.Joystick1Button5) && pm.currentMenu == PauseMenuManager.Menu.MAP){
			pm.getNextState ();
		}
	}
}
