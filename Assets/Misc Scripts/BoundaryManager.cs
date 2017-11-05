﻿using UnityEngine;
using System.Collections;

public class BoundaryManager : MonoBehaviour {
	private BoxCollider2D managerBox;
	private Transform player;
	public GameObject boundary;
	// Use this for initialization
	void Start () {
		managerBox = GetComponent<BoxCollider2D> ();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		ManageBoundary ();
	}

	void ManageBoundary () {
		Vector3 position = player.position;
		Vector3 boundsMin = managerBox.bounds.min;
		Vector3 boundsMax = managerBox.bounds.max;
		if(boundsMin.x < position.x && position.x < boundsMax.x &&
			boundsMin.y < position.y && position.y < boundsMax.y){
			boundary.SetActive (true);
		} else {
			boundary.SetActive (false);
		}
	}
}