using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMenu : MonoBehaviour {

	public GameObject cost;
	public GameObject output;
	public GameObject needs;
	public ResourceManager rm;
	public InfoMenuManager im;

	void Start () {
		im = InfoMenuManager.getInstance ();
		rm = ResourceManager.getResourceManager ();
		cost.GetComponent<Text> ().text = rm.GetCostOfGenerator().ToString() + " G";
		needs.GetComponent<Text> ().text = rm.CalculatePower ().ToString ();
		output.GetComponent<Text> ().text = (rm.GENERATORS * (rm.POWER_EFFICIENCY * 1000)).ToString();
	}

	// Update is called once per frame
	void Update () {
		cost.GetComponent<Text> ().text = rm.GetCostOfGenerator().ToString() + " G";
		needs.GetComponent<Text> ().text = rm.CalculatePower ().ToString ();
		output.GetComponent<Text> ().text = (rm.GENERATORS * (rm.POWER_EFFICIENCY * 1000)).ToString();
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			rm.PurchaseGenerator ();
		}
	}
}
