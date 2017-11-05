using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.Joystick1Button5)) {
			SceneManager.LoadScene (1, LoadSceneMode.Single);
			Debug.Log (SceneManager.sceneCount);
		}
	}
}
