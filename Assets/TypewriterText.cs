using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterText : MonoBehaviour, IGoable {

	// Use this for initialization
	public string IntendedText;
	public float delay;
	public float afterDelay;
	public Text uiText;
	public TextAnchor anchor;
	public bool interruptable;
	SceneController sceneController;
	Coroutine routine;
	bool isRunning;

	void Start () {
		uiText = GetComponent<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (interruptable && !isRunning && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button1))) {
			this.uiText.enabled = false;
			Next ();
		}
		if (interruptable && isRunning && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button1))) {
			StopCoroutine (routine);
			uiText.text = IntendedText;
			isRunning = false;
		}
	}

	public void Go(SceneController scene){
		sceneController = scene;
		routine = StartCoroutine ("TypeOut");
	}

	void Next(){
		sceneController.Next ();
	}

	public IEnumerator TypeOut(){
		isRunning = true;
		int i = 0;
		int end = IntendedText.Length;
		while (i < end) {
			uiText.text = uiText.text + IntendedText.ToCharArray () [i];
			i++;
			yield return new WaitForSeconds(delay);
		}
		yield return new WaitForSeconds(afterDelay);
		this.uiText.enabled = false;
		Next ();
		yield return new WaitForSeconds(delay);
	}	
}
