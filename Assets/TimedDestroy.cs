using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour {

	// Use this for initialization
	public float timeToLive;
	void Start () {
		Destroy (this.gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
