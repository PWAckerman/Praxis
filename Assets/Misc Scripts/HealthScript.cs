﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthScript : MonoBehaviour {
	private Transform player;
	private Image HImage;
	private List<Image> HealthBars = new List<Image>();
	// Use this for initialization
	void Start () {
		HImage = GetComponent<Image> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}