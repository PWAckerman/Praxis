using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthBar : MonoBehaviour {
	private PlayerController player;
	private Transform hPosition;
	private List<HealthBarIndividual> HealthBars = new List<HealthBarIndividual>();
	private float spacing = .2f;
	private float lastHealth;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		for (var i = 0; i < player.healthCapacity; i++) {
			Debug.Log (player.health);
			if (i < player.health) {
				Debug.Log (i);
				Debug.Log ("ON");
				HealthBarIndividual healthBar = new HealthBarIndividual (HealthBarIndividual.State.ON);
				healthBar.transform.parent = this.transform;
				healthBar.transform.localScale = new Vector3 (2, 2, 1);
				healthBar.transform.localPosition = new Vector3 ((-1.81f + 0.3f) + (i * spacing), healthBar.transform.position.y,  1);
				HealthBars.Add (healthBar);
			} else {
				Debug.Log ("OFF");
				HealthBarIndividual healthBar = new HealthBarIndividual (HealthBarIndividual.State.OFF);
				healthBar.transform.parent = this.transform;
				healthBar.transform.localScale = new Vector3 (2, 2, 1);
				healthBar.transform.localPosition = new Vector3 ((-1.81f + 0.3f) + (i * spacing), healthBar.transform.position.y,  1);
				HealthBars.Add (healthBar);
			}
		}
		Debug.Log (player.healthCapacity);
	}
	
	// Update is called once per frame
	void Update () {
		int health = player.health;
		if (lastHealth != health) {
			ResetHealth (health);
		}
		lastHealth = player.health;
	}

	void ResetHealth(int val){
		int i = -1;
		foreach(HealthBarIndividual bar in HealthBars){
			i++;
			HealthBarIndividual.State state = i < val ? HealthBarIndividual.State.ON : HealthBarIndividual.State.OFF;
			bar.RenderState (state);
		}
	}
		
}
