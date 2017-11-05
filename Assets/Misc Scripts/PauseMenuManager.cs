using UnityEngine;

public class PauseMenuManager{

	public enum Menu{
		INFO,
		RESEARCH,
		MAP,
		INVENTORY,
		SYSTEM
	}

	public Menu currentMenu;
	int currentState = 0;
	float responsiveness = 0.2f;
	static PauseMenuManager instance;
	public Menu[] possibleStates;
	public float nextPress;
	// Use this for initialization
	public PauseMenuManager(){
		possibleStates = new Menu[]{ Menu.INFO, Menu.RESEARCH, Menu.MAP, Menu.INVENTORY, Menu.SYSTEM };
		currentMenu = possibleStates [currentState];
	}


	public static PauseMenuManager getInstance(){
		if(instance != null){
			return instance;
		} else {
			instance = new PauseMenuManager();
			return instance;
		}
	}

	public void setCurrentMode(Menu menu){
		currentMenu = menu;
	}

	public void getNextState(){
		Debug.Log ("nextpress");
		Debug.Log (Time.realtimeSinceStartup);
		if (Time.realtimeSinceStartup > nextPress) {
			getNextIndex ();
			currentMenu = possibleStates [currentState];
			nextPress = Time.realtimeSinceStartup + responsiveness;
		}
	}

	public void getPrevState(){
		Debug.Log ("nextpress");
		Debug.Log (Time.realtimeSinceStartup);
		if (Time.realtimeSinceStartup > nextPress) {
			Debug.Log ("nextpress");
			getPrevIndex ();
			currentMenu = possibleStates [currentState];
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
