using UnityEngine;

public class InfoMenuManager{

	public enum Panel{
		NONE,
		RAW,
		MINERS,
		REFINED,
		CRYPTO,
		INVENTORY,
		MANUFACTURING,
		ENERGY,
		MARKETING,
		DISTRIBUTION,
		REFINERIES
	}

	public Panel currentPanel;
	int currentState = 0;
	float responsiveness = 0.2f;
	static InfoMenuManager instance;
	public Panel[] possibleStates;
	public float nextPress;
	// Use this for initialization
	public InfoMenuManager(){
		possibleStates = new Panel[]{ Panel.NONE, Panel.RAW, Panel.MINERS, Panel.REFINED, Panel.CRYPTO, Panel.INVENTORY, Panel.MANUFACTURING, Panel.ENERGY, Panel.MARKETING, Panel.DISTRIBUTION, Panel.REFINERIES  };
		currentPanel = possibleStates [currentState];
	}


	public static InfoMenuManager getInstance(){
		if(instance != null){
			return instance;
		} else {
			instance = new InfoMenuManager();
			return instance;
		}
	}

	public void setCurrentMode(Panel panel){
		currentPanel = panel;
	}

	public bool isSubmenuOpen(){
		return currentPanel == Panel.NONE;
	}

	public void getNextState(){
		if (Time.realtimeSinceStartup > nextPress) {
			getNextIndex ();
			currentPanel = possibleStates [currentState];
			nextPress = Time.realtimeSinceStartup + responsiveness;
		}
	}

	public void getPrevState(){
		if (Time.realtimeSinceStartup > nextPress) {
			getPrevIndex ();
			currentPanel = possibleStates [currentState];
			nextPress = Time.realtimeSinceStartup + responsiveness;
		}
	}

	void getNextIndex(){
		if (currentState < possibleStates.Length - 1) {
			currentState++;
		} else {
			currentState = 0;
		}
	}

	void getPrevIndex(){
		if (currentState > 0) {
			currentState--;
		} else {
			currentState = possibleStates.Length - 1;
		}
	}

}
