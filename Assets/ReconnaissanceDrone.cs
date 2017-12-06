using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconnaissanceDrone : MonoBehaviour, IBurnable {
	public int maxSpeed;
	public PlayerController player;
	public Rigidbody2D myRB;
	public AudioManager am;
	public bool burning;
	// Use this for initialization
	void Start () {
		maxSpeed = 10;
		name = "ReconDrone";
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		am.Play("droneAppear");
		new WaitForSeconds (2f);
		am.LoopWithLocalAudioSource (GetComponent<AudioSource> (), "droneHovering");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.currentMode == PlayerMode.DRONE) {
			gameObject.tag = "Player";
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			myRB.velocity = new Vector2 (moveHorizontal * maxSpeed, moveVertical * maxSpeed);
			if (moveHorizontal > 0) {
				Flip (true);
			} else if (moveHorizontal < 0) {
				Flip (false);
			}
			if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
				Die ();
			}
		} else {
			gameObject.tag = "Untagged";
			myRB.velocity = new Vector2 (0, 0);
		}
	}

	public void Die(){
		GetComponent<Animator> ().SetBool ("dying", true);
		am.Play("droneDisappear");
	}

	public void Damage(int d){
		if (!GetComponent<Animator> ().GetBool ("dying")) {
			GetComponent<Animator> ().SetBool ("dying", true);
			am.Play ("droneDisappear");
		}
	}

	public void Burn(){
		Damage (1);
	}

	public bool isBurning(){
		return burning;
	}

	void DestroyMe(){
		Destroy (this.gameObject);
		player.gameObject.tag = "Player";
		player.currentMode = PlayerMode.PLAYER;
	}

	void Flip(bool right){
		GetComponent<SpriteRenderer> ().flipX = right;
	}
}
