using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.UI;

public class ResearchPanel : MonoBehaviour {

	public GameObject go;
	public GameObject text;
	public GameObject icon;
	public bool isActive;
	public Image border;
	public ResearchObject resObj;
	public RectTransform trans;
	public CanvasRenderer cr;
	public LayoutElement le;
	public HorizontalLayoutGroup hlogrp;

	// Use this for initialization
	public void SetResearchObject(ResearchObject obj){
		
	}
}
