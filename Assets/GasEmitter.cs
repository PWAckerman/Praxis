using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEmitter : MonoBehaviour, ISwitchable {

	// Use this for initialization
	public GameObject gas;
	public int emissionLimit;
	public float emissionSpeed;
	public bool on {get; set;}
	Coroutine emission;

	void Start () {
		on = true;
		emission = StartCoroutine ("EmitGas");
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
		emission = StartCoroutine ("EmitGas");
	}

	IEnumerator EmitGas(){
		var x = emissionLimit;
		while(x > 0){
			yield return new WaitForSeconds(emissionSpeed);
			Instantiate(gas,transform.position, Quaternion.identity);
			x--;
		}
	}
}
