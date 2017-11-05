using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NewBehaviourScript2 : MonoBehaviour, IPointerDownHandler {

	// Use this for initialization
	Simulator sim;
	PlayerController player;

	void Start () {
		this.sim = new Simulator(1);
		this.player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.sim.Update();
	}

	public void OnPointerDown(PointerEventData ped) {
        print ("Pressed");
		this.sim.Start();
		this.player.Damage ();
    }  
}
