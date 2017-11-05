using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerMenu : MonoBehaviour {

	public GameObject cost;
	public ResourceManager rm;
	public InfoMenuManager im;
	public string[] miners;
	public Dictionary<string, GameObject> minersDict;
	public int currentMiner;

	void Start () {
		im = InfoMenuManager.getInstance ();
		rm = ResourceManager.getResourceManager ();
		miners = new string[]{"A","B","C","D"};
		minersDict = new Dictionary<string, GameObject>{
			{"A", GameObject.FindGameObjectWithTag("MinerAIcon")},
			{"B", GameObject.FindGameObjectWithTag("MinerBIcon")},
			{"C", GameObject.FindGameObjectWithTag("MinerCIcon")},
			{"D", GameObject.FindGameObjectWithTag("MinerDIcon")},
		};
		currentMiner = 0;
		cost.GetComponent<Text> ().text = rm.GetMinerCost (miners[currentMiner]).ToString() + " G";
	}

	// Update is called once per frame
	void Update () {
		cost.GetComponent<Text> ().text = rm.GetMinerCost (miners[currentMiner]).ToString();
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			rm.PurchaseMiner(miners[currentMiner]);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			minersDict [miners [currentMiner]].GetComponent<Image> ().color = Color.gray;
			getPrevMiner ();
			minersDict [miners [currentMiner]].GetComponent<Image> ().color = Color.white;
			Input.ResetInputAxes ();
		} else if (Input.GetAxis ("Horizontal") > 0) {
			minersDict [miners [currentMiner]].GetComponent<Image> ().color = Color.gray;
			getNextMiner ();
			minersDict [miners [currentMiner]].GetComponent<Image> ().color = Color.white;
			Input.ResetInputAxes ();
		}
	}

	void getNextMiner(){
		if (currentMiner < miners.Length - 1) {
			currentMiner++;
		} else {
			currentMiner = 0;
		}
	}

	void getPrevMiner(){
		if (currentMiner > 0) {
			currentMiner--;
		} else {
			currentMiner = miners.Length - 1;
		}
	}


}
