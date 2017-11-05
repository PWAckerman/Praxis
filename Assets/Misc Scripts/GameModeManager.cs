using System.Collections;
using System.Collections.Generic;


public class GameModeManager{

	public enum Mode{
		PAUSED,
		RUNNING
	}

	public Mode currentMode;
	static GameModeManager instance;
	// Use this for initialization
	public GameModeManager(){
		currentMode = Mode.RUNNING;
	}

	public static GameModeManager getInstance(){
		if(instance != null){
			return instance;
		} else {
			instance = new GameModeManager ();
			return instance;
		}
	}

	public void setCurrentMode(Mode mode){
		currentMode = mode;
	}

	public bool isPaused(){
		return currentMode == Mode.PAUSED;
	}

	public bool isRunning(){
		return currentMode == Mode.RUNNING;
	}
}
