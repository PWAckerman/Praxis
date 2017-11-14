using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEmitter : MonoBehaviour, ISwitchable {

	// Use this for initialization
	public GameObject fire;
	public int emissionLimit;
	public float emissionSpeed;
	public bool on {get; set;}
	Coroutine emission;

	void Start () {
		on = false;
	}

	// Update is called once per frame
	void Update () {

	}

	public void TurnOff(){
		on = false;
		StopCoroutine (emission);
	}

	public void TurnOn(){
		on = true;
		emission = StartCoroutine ("EmitFire");
	}

	IEnumerator EmitFire(){
		var x = emissionLimit;
		while(x > 0){
			yield return new WaitForSeconds(emissionSpeed);
			Instantiate(fire,transform.position, Quaternion.identity);
			x--;
		}
	}
}
