using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ElevatorMode{
	PLAYER,
	NOPLAYER
}

public class Elevator : MonoBehaviour {

	// Use this for initialization
	public PlayerController player;
	public bool open;
	public Animator anim;
	public GameObject target;
	public bool playerIsPresent;
	public bool playerIsInside;
	public List<BoxCollider2D> enclosure;
	public ElevatorMode currentMode;
	public AudioManager am;
	public bool transition;

	void Start () {
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		open = false;
		playerIsInside = false;
		enclosure = GetComponentsInChildren<BoxCollider2D> ()
			.Where<BoxCollider2D> (el => !el.isTrigger)
			.ToList ();
		enclosure.ForEach (el => el.enabled = false);
	}
	
	// Update is called once per frame
	void Update () {
		if (playerIsPresent && Input.GetKeyDown(KeyCode.Joystick1Button17) && open) {
			open = false;
		} else if (playerIsPresent && Input.GetKeyDown(KeyCode.Joystick1Button17) && !open) {
			open = true;
			transition = false;
			playerIsInside = false;
		}
		if (currentMode == ElevatorMode.PLAYER && Input.GetAxis ("Vertical") < 0 && playerIsPresent && playerIsInside && !open && !transition) {
			//MOVE TO TARGET
			new WaitForSeconds(4f);
			am.Play ("elevatorMove");
			player.transform.position = target.transform.position;
			target.GetComponent<Elevator>().currentMode = ElevatorMode.PLAYER;
			target.GetComponent<Elevator> ().transition = true;
			target.GetComponent<Elevator> ().player = player;
			target.GetComponent<Elevator> ().enclosure.ForEach (el => el.enabled = true);
			target.GetComponent<Elevator> ().GetComponent<SpriteRenderer> ().sortingOrder = 5;
			currentMode = ElevatorMode.NOPLAYER;
			transition = true;
			playerIsInside = false;
			
		}
		anim.SetBool ("open", open);
	}

	void DoorOpenComplete(){
		GetComponent<SpriteRenderer> ().sortingOrder = 2;
		enclosure.ForEach (el => el.enabled = false);
		player.currentMode = PlayerMode.PLAYER;
		am.Play ("doorOpen");
	}

	void DoorCloseStart(){
		if (playerIsPresent) {
			GetComponent<SpriteRenderer> ().sortingOrder = 5;
			enclosure.ForEach (el => el.enabled = true);
			player.currentMode = PlayerMode.ELEVATOR;
			currentMode = ElevatorMode.PLAYER;
		}
		if (!playerIsPresent) {
			GetComponent<SpriteRenderer> ().sortingOrder = 2;
			enclosure.ForEach (el => el.enabled = false);
			currentMode = ElevatorMode.NOPLAYER;
		}
	}

	void DoorCloseComplete(){
		am.Play ("doorClose");
		if (playerIsPresent) {
			playerIsInside = true;
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			player = coll.gameObject.GetComponent<PlayerController> ();
			playerIsPresent = true;
			playerIsInside = false;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			playerIsPresent = false;
			playerIsInside = false;
			enclosure.ForEach (el => el.enabled = false);
			GetComponent<SpriteRenderer> ().sortingOrder = 2;
			open = false;
		}
	}
}
