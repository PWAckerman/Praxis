using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefinedSubMenu : MonoBehaviour {

	public GameObject number;
	public ResourceManager rm;
	public InfoMenuManager im;

	void Start () {
		im = InfoMenuManager.getInstance ();
		rm = ResourceManager.getResourceManager ();
		number.GetComponent<Text> ().text = rm.GetNeededManResources ().ToString() + " EA";
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("Refine");
		number.GetComponent<Text> ().text = rm.GetNeededManResources ().ToString() + " EA";
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			Debug.Log ("Keydown");
			rm.ManuallyManufacture ();
		} else {
			Debug.Log ("No keydown");
		}
	}

	void Refine(){

	}
}
