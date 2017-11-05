using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefinementMenu : MonoBehaviour {

	public GameObject cost;
	public ResourceManager rm;
	public InfoMenuManager im;

	void Start () {
		im = InfoMenuManager.getInstance ();
		rm = ResourceManager.getResourceManager ();
		cost.GetComponent<Text> ().text = rm.GetRefineryCost().ToString() + " G";
	}

	// Update is called once per frame
	void Update () {
		cost.GetComponent<Text> ().text = rm.GetRefineryCost().ToString() + " G";
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			rm.PurchaseRefinery();
		}
	}
}
