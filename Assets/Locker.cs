using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour {

	// Use this for initialization
	public BoxCollider2D trigger;
	public Sprite openSprite;
	public Sprite closedSprite;
	GameObject player;
	bool playerIsPresent;
	bool playerIsInside;
	public bool isOpen;
	public GameObject pointer;
	GameObject currentPointer;
	AudioManager am;

	void Start () {
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerIsInside = false;
		playerIsPresent = false;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown(KeyCode.Joystick1Button17)) && playerIsPresent && !playerIsInside && !isOpen) {
			Open ();
		} else if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown(KeyCode.Joystick1Button17)) && playerIsPresent && !playerIsInside && isOpen) {
			player.GetComponent<PlayerController>().currentMode = PlayerMode.LOCKER;
			player.GetComponent<PlayerController> ().locker = this.transform;
			player.transform.localScale = new Vector3 (0f, 0f, 0f);
			playerIsInside = true;
			currentPointer = Instantiate (pointer, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
			Close ();
		} else if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown(KeyCode.Joystick1Button17)) && playerIsInside && !isOpen) {
			player.GetComponent<PlayerController>().currentMode = PlayerMode.PLAYER;
			player.GetComponent<PlayerController> ().locker = null;
			player.transform.localScale = new Vector3 (8f, 8f, 8f);
			player.transform.position = transform.position;
			playerIsInside = false;
			Destroy (currentPointer);
			Open ();
		}
	}

	void Open(){
		isOpen = true;
		am.Play ("doorOpen");
		GetComponent<SpriteRenderer> ().sprite = openSprite;
	}

	void Close(){
		isOpen = false;
		am.Play ("doorClose");
		GetComponent<SpriteRenderer> ().sprite = closedSprite;
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.tag == "Player") {
			playerIsPresent = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			playerIsPresent = false;
			if (isOpen) {
				Close ();
			}
		}
	}
}
