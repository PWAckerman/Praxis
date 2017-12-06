using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDialogueManager : MonoBehaviour {

	// Use this for initialization
	TextMesh txt;
	MeshRenderer mr;
	Coroutine cr;
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

	public void TriggerText(string tx){
		if (txt.text != tx) {
			StopDisplay ();
			cr = StartCoroutine ("ShowText", tx);
		}
	}

	void StopDisplay(){
		if (cr != null) {
			cr = StartCoroutine ("ShowText", "");
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
