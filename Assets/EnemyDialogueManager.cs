using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDialogueManager : MonoBehaviour {

	// Use this for initialization
	TextMesh txt;
	MeshRenderer mr;
	Coroutine cr;
	public string alert;
	public string abandon;
	public string greeting;
	public string reveal;
	public string freakingOut;
	public string confused;
	public string tranquilized;
	public float displayTime;
	[SerializeField]
	string currentText;

	void Start () {
		mr = GetComponent<MeshRenderer> ();
		txt = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentText = txt.text;
		Readjust ();
	}

	void Readjust(){
		if (transform.parent.localScale.x < 0) {
			Vector3 scl = transform.localScale;
			scl.x = -Mathf.Abs (scl.x);
			transform.localScale = scl;
		} else if(transform.parent.localScale.x > 0 & transform.localScale.x < 0){
			Vector3 scl = transform.localScale;
			scl.x = Mathf.Abs (scl.x);
			transform.localScale = scl;
		}
	}

	public void ShowAlert(){
		if (txt.text != alert) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", alert);
		}
	}

	public void ShowWake(){
		if (txt.text != "Wake up!") {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", "Wake up!");
		}
	}

	public void Clear(){
		if (txt.text != null) {
			StopDisplay ();
		}
	}

	public void ShowTranquilized(){
		if (txt.text != tranquilized) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", tranquilized);
		}
	}

	public void ShowAbandon(){
		if (txt.text != alert) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", abandon);
		}
	}

	public void ShowGreeting(){
		if (txt.text != greeting) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", greeting);
		}
	}

	public void ShowConfused(){
		if (txt.text != confused) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", confused);
		}
	}

	public void ShowReveal(){
		if (txt.text != reveal) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", reveal);
		}
	}

	public void ShowFreakOut(){
		if (txt.text != freakingOut) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", freakingOut);
		}
	}

	void StopDisplay(){
		if (cr != null) {
			cr = StartCoroutine ("ShowText", alert);
		}
	}

	IEnumerator ShowText(string tex){
		txt.text = tex;
		mr.enabled = true;
		yield return new WaitForSeconds (displayTime);
		mr.enabled = false;
		txt.text = "";
		yield return new WaitForSeconds (0.1f);
	}
}
