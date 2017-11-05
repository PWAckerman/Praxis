using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchPanelController : MonoBehaviour {

	// Use this for initialization

	public List<ResearchObject> ros;
	public GameObject prefab;

	void Start () {
		ros = ResearchObject.LoadAllFromFile ();
		Debug.Log (ros.Count);
		ros.ForEach (ro => {
			GameObject panel = (GameObject)Instantiate (prefab);
			panel.transform.parent = this.transform;
			panel.GetComponent<ResearchPanel> ().SetResearchObject (ro);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
