using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcove : MonoBehaviour, IStashTarget {

	// Use this for initialization
	public BoxCollider2D trigger;
	public GameObject buttonDisplay;
	GameObject player;
	GameObject instance;
	IStashable stashed;
	bool playerIsPresent;
	bool playerIsInside;
	bool blocked;
	bool debounce = true;
	public GameObject pointer;
	GameObject markerInstance;
	GameObject currentPointer;
	AudioManager am;

	void Start () {
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerIsInside = false;
		playerIsPresent = false;
		blocked = false;
	}

	public void StashInside(IStashable go){
		if (stashed == null) {
			stashed = go;
			debounce = false;
			markerInstance = Instantiate (go.marker, new Vector3 (transform.position.x, transform.position.y + 2.7f, transform.position.z), Quaternion.identity);
			go.Stash ();
			StartCoroutine ("StashDebounce");
		}
	}

	void Release(){
		Destroy(markerInstance);
		Debug.Log ("DESTROY MARKER");
		debounce = false;
		stashed.Release (transform);
		stashed = null;
		StartCoroutine ("StashDebounce");
	}

	// Update is called once per frame
	void Update () {
		if(stashed != null && debounce){
			if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown (KeyCode.Joystick1Button17)) && playerIsPresent && !playerIsInside && player.GetComponent<PlayerController> ().currentMode == PlayerMode.PLAYER && stashed != null && debounce) {
				Release ();
			}
		} else if (stashed == null && debounce) {
			if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown (KeyCode.Joystick1Button17)) && playerIsPresent && !playerIsInside && player.GetComponent<PlayerController> ().currentMode == PlayerMode.PLAYER && !blocked) {
				if (player.GetComponent<PlayerController> ().pickedUp != null) {
					player.GetComponent<PlayerController> ().Throw ();
				}
				player.GetComponent<PlayerController> ().currentMode = PlayerMode.LOCKER;
				player.GetComponent<PlayerController> ().locker = this.transform;
				player.transform.localScale = new Vector3 (0f, 0f, 0f);
				player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
				player.GetComponent<BoxCollider2D> ().enabled = false;
				player.GetComponent<CircleCollider2D> ().enabled = false;
				playerIsInside = true;
				if (instance != null) {
					Destroy (instance);
					instance = null;
				}
				currentPointer = Instantiate (pointer, new Vector3 (transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
			} else if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown (KeyCode.Joystick1Button17)) && playerIsInside) {
				player.GetComponent<PlayerController> ().currentMode = PlayerMode.PLAYER;
				player.GetComponent<PlayerController> ().locker = null;
				player.transform.localScale = new Vector3 (8f, 8f, 8f);
				player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
				player.transform.position = transform.position;
				player.GetComponent<BoxCollider2D> ().enabled = true;
				player.GetComponent<CircleCollider2D> ().enabled = true;
				playerIsInside = false;
				if (instance == null) {
					instance = Instantiate (buttonDisplay, new Vector3 (transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
				}
				Destroy (currentPointer);
			}
		}
	}

	IEnumerator StashDebounce(){
		yield return new WaitForSeconds(2f);
		debounce = true;
		yield return new WaitForSeconds(2f);
	}

	void OnDestroy(){
		if (playerIsInside) {
			player.GetComponent<PlayerController> ().currentMode = PlayerMode.PLAYER;
			player.GetComponent<PlayerController> ().locker = null;
			player.transform.localScale = new Vector3 (8f, 8f, 8f);
			player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
			player.transform.position = transform.position;
			player.GetComponent<BoxCollider2D> ().enabled = true;
			player.GetComponent<CircleCollider2D> ().enabled = true;
			Destroy (currentPointer);
			Destroy (instance);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player") {
			playerIsPresent = true;
			if (instance == null) {
				instance = Instantiate (buttonDisplay, new Vector3 (transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
			}
		} else {
			blocked = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			playerIsPresent = false;
			if (instance != null) {
				Destroy (instance);
				instance = null;
			}
		} else {
			blocked = false;
		}
	}
}
