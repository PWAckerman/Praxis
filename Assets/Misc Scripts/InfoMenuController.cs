using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuController : MonoBehaviour {
	public ResourceManager rm;
	public PauseMenuManager pm;
	public GameModeManager gm;
	public InfoMenuManager im;
	public GameObject[] panels;
	public GameObject currentPanel;
	public GameObject[] submenus;
	public Dictionary<InfoMenuManager.Panel, GameObject> submenusDict;
	public int currentPanelIndex;
	public Text resourceALabel;
	public Text resourceBLabel;
	public Text resourceCLabel;
	public Text resourceDLabel;
	public Text resourceGLabel;
	public Text refinedALabel;
	public Text refinedBLabel;
	public Text refinedCLabel;
	public Text refinedDLabel;
	public Text minerALabel;
	public Text minerBLabel;
	public Text minerCLabel;
	public Text minerDLabel;
	public Text inventory;
	public Text generators;
	public Text generatorE;
	public Text marketing;
	public Text demand;
	public Text price;
	public Text factories;
	public Text factoryE;
	public Text factoryY;
	public Text refineries;
	public Text refineryE;
	public Text refineryY;
	public Text distribution;
	public Text workforce;
	public Text crypto;
	public Text nodes;
	public Text cryptoDifficulty;
	// Use this for initialization
	void Start () {
		rm = ResourceManager.getResourceManager();
		pm = PauseMenuManager.getInstance ();
		gm = GameModeManager.getInstance ();
		im = InfoMenuManager.getInstance ();
		panels = new GameObject[] {
			(GameObject)GameObject.FindGameObjectWithTag ("RawPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("MinersPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("RefinedPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("CryptoPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("InventoryPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("ManufacturingPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("EnergyPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("MarketingPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("DistributionPanel"),
			(GameObject)GameObject.FindGameObjectWithTag ("RefinementPanel")
		};
		submenusDict = new Dictionary<InfoMenuManager.Panel, GameObject>{
			{InfoMenuManager.Panel.RAW, submenus[0]},
			{InfoMenuManager.Panel.MINERS, submenus[1]},
			{InfoMenuManager.Panel.REFINED, submenus[2]},
			{InfoMenuManager.Panel.CRYPTO, submenus[3]},
			{InfoMenuManager.Panel.INVENTORY, submenus[4]},
			{InfoMenuManager.Panel.MANUFACTURING, submenus[5]},
			{InfoMenuManager.Panel.ENERGY, submenus[6]},
			{InfoMenuManager.Panel.MARKETING, submenus[7]},
			{InfoMenuManager.Panel.DISTRIBUTION, submenus[8]},
			{InfoMenuManager.Panel.REFINERIES, submenus[9]},
		};
		HidePanelMenus ();
		panels.ToList().ForEach(obj => obj.GetComponent<Image>().color = Color.grey);
		currentPanel = panels [currentPanelIndex];
		currentPanel.GetComponent<Image> ().color = Color.white;
		resourceALabel = GameObject.FindGameObjectWithTag ("ALabel").GetComponent<Text> ();
		resourceBLabel = GameObject.FindGameObjectWithTag ("BLabel").GetComponent<Text> ();
		resourceCLabel = GameObject.FindGameObjectWithTag ("CLabel").GetComponent<Text> ();
		resourceDLabel = GameObject.FindGameObjectWithTag ("DLabel").GetComponent<Text> ();
		resourceGLabel = GameObject.FindGameObjectWithTag ("GoldLabel").GetComponent<Text> ();
		refinedALabel = GameObject.FindGameObjectWithTag ("RefinedA").GetComponent<Text> ();
		refinedBLabel = GameObject.FindGameObjectWithTag ("RefinedB").GetComponent<Text> ();
		refinedCLabel = GameObject.FindGameObjectWithTag ("RefinedC").GetComponent<Text> ();
		refinedDLabel = GameObject.FindGameObjectWithTag ("RefinedD").GetComponent<Text> ();
		minerALabel = GameObject.FindGameObjectWithTag ("MinerA").GetComponent<Text> ();
		minerBLabel = GameObject.FindGameObjectWithTag ("MinerB").GetComponent<Text> ();
		minerCLabel = GameObject.FindGameObjectWithTag ("MinerC").GetComponent<Text> ();
		minerDLabel = GameObject.FindGameObjectWithTag ("MinerD").GetComponent<Text> ();
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Text> ();
		generators = GameObject.FindGameObjectWithTag ("Generators").GetComponent<Text> ();
		generatorE = GameObject.FindGameObjectWithTag ("GeneratorE").GetComponent<Text> ();
		marketing = GameObject.FindGameObjectWithTag ("Marketing").GetComponent<Text> ();
		demand = GameObject.FindGameObjectWithTag ("Demand").GetComponent<Text> ();
		price = GameObject.FindGameObjectWithTag ("Price").GetComponent<Text> ();
		factories = GameObject.FindGameObjectWithTag ("Factories").GetComponent<Text> ();
		factoryE = GameObject.FindGameObjectWithTag ("FactoryE").GetComponent<Text> ();
		factoryY = GameObject.FindGameObjectWithTag ("FactoryY").GetComponent<Text> ();
		refineries = GameObject.FindGameObjectWithTag ("Refineries").GetComponent<Text> ();
		refineryE = GameObject.FindGameObjectWithTag ("RefineryE").GetComponent<Text> ();
		refineryY = GameObject.FindGameObjectWithTag ("RefineryY").GetComponent<Text> ();
		distribution = GameObject.FindGameObjectWithTag ("Distribution").GetComponent<Text> ();
		workforce = GameObject.FindGameObjectWithTag ("Workforce").GetComponent<Text> ();
		crypto = GameObject.FindGameObjectWithTag ("Crypto").GetComponent<Text> ();
		nodes = GameObject.FindGameObjectWithTag ("CryptoNodes").GetComponent<Text> ();
		cryptoDifficulty = GameObject.FindGameObjectWithTag ("CryptoDifficulty").GetComponent<Text> ();
		resourceALabel.text = rm.ResourceA.ToString();
		resourceBLabel.text = rm.ResourceB.ToString();
		resourceCLabel.text = rm.ResourceC.ToString();
		resourceDLabel.text = rm.ResourceD.ToString();
		resourceGLabel.text = rm.Gold.ToString();
		refinedALabel.text = rm.RefinedA.ToString();
		refinedBLabel.text = rm.RefinedB.ToString();
		refinedCLabel.text = rm.RefinedC.ToString();
		refinedDLabel.text = rm.RefinedD.ToString();
		minerALabel.text = rm.MinerA.ToString();
		minerBLabel.text = rm.MinerB.ToString();
		minerCLabel.text = rm.MinerC.ToString();
		minerDLabel.text = rm.MinerD.ToString();
		inventory.text = rm.WIDGET_INVENTORY.ToString ();
		generators.text = rm.GENERATORS.ToString ();
		generatorE.text = rm.POWER_EFFICIENCY.ToString ();
		marketing.text = rm.MARKETING_LEVEL.ToString ();
		demand.text = rm.DEMAND.ToString ();
		price.text = rm.BASE_PRICE.ToString ();
		factories.text = rm.FACTORIES.ToString ();
		factoryY.text = rm.PRODUCTION_BASE_YIELD.ToString();
		factoryE.text = rm.PRODUCTION_EFFICIENCY.ToString();
		refineries.text = rm.REFINERIES.ToString ();
		refineryE.text = rm.REFINERY_EFFICIENCY.ToString ();
		refineryY.text = rm.REFINE_BASE_YIELD.ToString ();
		distribution.text = rm.DISTRIBUTION_FORCE.ToString ();
		workforce.text = rm.WORKFORCE.ToString ();
		crypto.text = rm.CRYPTO.ToString ();
		nodes.text = rm.CRYPTO_NODES.ToString ();
		cryptoDifficulty.text = rm.CRYPTO_DIFFICULTY.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
			if (Input.GetKeyDown (KeyCode.Joystick1Button4) && pm.currentMenu == PauseMenuManager.Menu.INFO) {
				Debug.Log ("We're skipping INFO for some reason");
				pm.getPrevState ();
			}
			if (Input.GetKeyDown (KeyCode.Joystick1Button5) && pm.currentMenu == PauseMenuManager.Menu.INFO) {
				Debug.Log ("We're skipping INFO for some reason");
				pm.getNextState ();
			}
			PanelSelection ();
		    ProcessAction ();
			resourceALabel.text = rm.ResourceA.ToString ();
			resourceBLabel.text = rm.ResourceB.ToString ();
			resourceCLabel.text = rm.ResourceC.ToString ();
			resourceDLabel.text = rm.ResourceD.ToString ();
			resourceGLabel.text = rm.Gold.ToString ();
			refinedALabel.text = rm.RefinedA.ToString ();
			refinedBLabel.text = rm.RefinedB.ToString ();
			refinedCLabel.text = rm.RefinedC.ToString ();
			refinedDLabel.text = rm.RefinedD.ToString ();
			minerALabel.text = rm.MinerA.ToString ();
			minerBLabel.text = rm.MinerB.ToString ();
			minerCLabel.text = rm.MinerC.ToString ();
			minerDLabel.text = rm.MinerD.ToString ();
			inventory.text = rm.WIDGET_INVENTORY.ToString ();
			generators.text = rm.GENERATORS.ToString ();
			generatorE.text = rm.POWER_EFFICIENCY.ToString ();
			marketing.text = rm.MARKETING_LEVEL.ToString ();
			demand.text = rm.DEMAND.ToString ();
			price.text = rm.BASE_PRICE.ToString ();
			factories.text = rm.FACTORIES.ToString ();
			factoryY.text = rm.PRODUCTION_BASE_YIELD.ToString ();
			factoryE.text = rm.PRODUCTION_EFFICIENCY.ToString ();
			refineries.text = rm.REFINERIES.ToString ();
			refineryE.text = rm.REFINERY_EFFICIENCY.ToString ();
			refineryY.text = rm.REFINE_BASE_YIELD.ToString ();
			distribution.text = rm.DISTRIBUTION_FORCE.ToString ();
			workforce.text = rm.WORKFORCE.ToString ();
			crypto.text = rm.CRYPTO.ToString ();
			nodes.text = rm.CRYPTO_NODES.ToString ();
			cryptoDifficulty.text = rm.CRYPTO_DIFFICULTY.ToString ();
	}

	void ProcessAction(){
		if (gm.isPaused () && im.currentPanel == InfoMenuManager.Panel.NONE) {
			if(Input.GetKeyDown(KeyCode.Joystick1Button1)){
				switch (currentPanel.tag) {
				case "RawPanel":
					im.setCurrentMode (InfoMenuManager.Panel.RAW);
					break;
				case "RefinedPanel":
					im.setCurrentMode (InfoMenuManager.Panel.REFINED);
					break;
				case "MinersPanel":
					im.setCurrentMode (InfoMenuManager.Panel.MINERS);
					break;
				case "EnergyPanel":
					im.setCurrentMode (InfoMenuManager.Panel.ENERGY);
					break;
				case "CryptoPanel":
					im.setCurrentMode (InfoMenuManager.Panel.CRYPTO);
					break;
				case "ManufacturingPanel":
					im.setCurrentMode (InfoMenuManager.Panel.MANUFACTURING);
					break;
				case "MarketingPanel":
					im.setCurrentMode (InfoMenuManager.Panel.MARKETING);
					break;
				case "DistributionPanel":
					im.setCurrentMode (InfoMenuManager.Panel.DISTRIBUTION);
					break;
				case "RefinementPanel":
					im.setCurrentMode (InfoMenuManager.Panel.REFINERIES);
					break;
				default:
					break;
				}
				EnableCurrentMenu ();	
			}
		}
		if (!gm.isPaused ()) {
			im.setCurrentMode (InfoMenuManager.Panel.NONE);
			HidePanelMenus ();
		}
	}

	void HidePanelMenus(){
		submenus.ToList<GameObject>().ForEach (menu => menu.SetActive(false));
	}

	void EnableCurrentMenu(){
		HidePanelMenus ();
		if (im.currentPanel != InfoMenuManager.Panel.NONE) {
			Debug.Log (submenusDict[im.currentPanel]);
			submenusDict [im.currentPanel].SetActive (true);
		}
	}

	void PanelSelection(){
		Debug.Log ("PanelSelection");
		Debug.Log (gm.currentMode);
		if (gm.isPaused() && im.isSubmenuOpen()) {
			Debug.Log ("PanelSelection:PAUSED");
			if (Input.GetAxis ("Vertical") < 0) {
				getNextPanel ();
				Input.ResetInputAxes ();
			} else if (Input.GetAxis ("Vertical") > 0) {
				getPrevPanel ();
				Input.ResetInputAxes ();
			}
			if (Input.GetAxis ("Horizontal") < 0) {
				getAdjacentPanelBackward ();
				Input.ResetInputAxes ();
			} else if (Input.GetAxis ("Horizontal") > 0) {
				getAdjacentPanelForward ();
				Input.ResetInputAxes ();
			}
		}
		if (gm.isPaused () && !im.isSubmenuOpen ()) {
			if (Input.GetKeyDown (KeyCode.Joystick1Button0)) {
				im.setCurrentMode (InfoMenuManager.Panel.NONE);
				HidePanelMenus ();
			}
		}
	}

	void getNextIndex(){
		if (currentPanelIndex < panels.Length - 1) {
			currentPanelIndex++;
		} else {
			currentPanelIndex = 0;
		}
	}

	void getPrevIndex(){
		if (currentPanelIndex > 0) {
			currentPanelIndex--;
		} else {
			currentPanelIndex = panels.Length - 1;
		}
	}

	void getNextPanel(){
		getNextIndex ();
		currentPanel.GetComponent<Image>().color = Color.grey;
		currentPanel = panels [currentPanelIndex];
		currentPanel.GetComponent<Image> ().color = Color.white;
	}

	void getAdjacentPanelForward(){
		getNextIndex ();
		getNextIndex ();
		currentPanel.GetComponent<Image>().color = Color.grey;
		currentPanel = panels [currentPanelIndex];
		currentPanel.GetComponent<Image> ().color = Color.white;
	}

	void getAdjacentPanelBackward(){
		getPrevIndex ();
		getPrevIndex ();
		currentPanel.GetComponent<Image>().color = Color.grey;
		currentPanel = panels [currentPanelIndex];
		currentPanel.GetComponent<Image> ().color = Color.white;
	}

	void getPrevPanel(){
		getPrevIndex ();
		currentPanel.GetComponent<Image>().color = Color.grey;
		currentPanel = panels [currentPanelIndex];
		currentPanel.GetComponent<Image> ().color = Color.white;
	}
}
