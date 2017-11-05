using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawSubmenu : MonoBehaviour {

	public GameObject number;
	public ResourceManager rm;
	public InfoMenuManager im;

	void Start () {
		im = InfoMenuManager.getInstance ();
		rm = ResourceManager.getResourceManager ();
		number.GetComponent<Text> ().text = rm.GetNeededResources ().ToString() + " EA";
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Refine");
		number.GetComponent<Text> ().text = rm.GetNeededResources ().ToString() + " EA";
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			Debug.Log ("Keydown");
			rm.ManuallyRefine ();
		} else {
			Debug.Log ("No keydown");
		}
	}

	void Refine(){

	}
}
