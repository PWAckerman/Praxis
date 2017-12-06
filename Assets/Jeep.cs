using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JeepMode {
	ACCELERATING,
	BRAKING,
	DRIVING
}

public class Jeep : MonoBehaviour, IDamageable, IBurnable, ISortable {

	public GameObject driver;
	GameObject player;
	public GameObject pointer;
	public GameObject buttonDisplay;
	public GameObject explosion;
	public GameObject fire;
	public GameObject dust;
	public HingeJoint2D backWheel;
	public HingeJoint2D frontWheel;
	public int maxHitPoints;
	public int hitPoints;
	public float maxSpeed;
	public float currentSpeed = 0;
	public bool on;
	public bool playerIsPresent;
	public bool enterable;
	public bool burning;
	public Animator myAnim;
	public GasEmitter emitter;

	GameObject instance;
	GameObject currentPointer;
	public JeepMode currentMode;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		if (on) {
			StartMotor ();
		}
		emitter = GetComponent<GasEmitter> ();
		hitPoints = maxHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		enterable = IsEnterable ();
		GetInOrOut ();
		Drive ();
		if (currentMode == JeepMode.ACCELERATING && driver == player) {
			myAnim.SetBool ("backup", false);
		} else if (currentMode == JeepMode.BRAKING && driver == player) {
			myAnim.SetBool ("driving", false);
			myAnim.SetBool ("backup", true);
		} else if (currentMode == JeepMode.DRIVING && driver == player) {
			myAnim.SetBool ("driving", true);
			myAnim.SetBool ("backup", false);
		} else if (driver != player) {
			myAnim.SetBool ("driving", false);
			myAnim.SetBool ("backup", false);
		}
	}

	public void Damage(int amount = 1){
		if (hitPoints > 0) {
			hitPoints -= amount;
			StartCoroutine ("Flash");
		}
		if (hitPoints < (maxHitPoints / 2)) {
			emitter.TurnOn ();
		}
		if (hitPoints <= 0) {
			Die ();
		}
	}

	public void FixSort(){
		gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 5;
		backWheel.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 6;
		frontWheel.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 6;
	}

	public void Burn(){
		if (!isBurning ()) {
			burning = true;
			Damage ();
			StartCoroutine ("StopBurning");
		}
	}

	public bool isBurning(){
		return burning;
	}

	IEnumerator StopBurning(){
		yield return new WaitForSeconds(3f);
		burning = false;
	}

	public void Die(){
		backWheel.gameObject.transform.parent = null;
		frontWheel.gameObject.transform.parent = null;
		frontWheel.gameObject.GetComponent<Rigidbody2D> ().mass = 10f;
		backWheel.gameObject.GetComponent<Rigidbody2D> ().mass = 10f;
		if (driver == player) {
			myAnim.SetBool ("player", false);
			player.GetComponent<PlayerController> ().currentMode = PlayerMode.PLAYER;
			player.GetComponent<PlayerController> ().locker = null;
			player.transform.localScale = new Vector3 (8f, 8f, 8f);
			gameObject.tag = "Untagged";
			player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
			player.transform.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z);
			player.transform.parent = null;
			player.GetComponent<BoxCollider2D> ().enabled = true;
			player.GetComponent<CircleCollider2D> ().enabled = true;
		}
		driver = null;
		Destroy (currentPointer);
		Instantiate (explosion, transform.position, Quaternion.identity);
		Instantiate (dust, transform.position, Quaternion.identity);
		Instantiate (fire, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
	}

	void Drive(){
		if (on) {
			StartMotor ();
		}
		if (driver != null) {
			Debug.Log ("Accelerate or Brake");
			if (Input.GetAxis ("Braking") > 0 || Input.GetKey (KeyCode.H)) {
				currentMode = JeepMode.ACCELERATING;
			} else if (Input.GetAxis ("Aiming") > 0 || Input.GetKey (KeyCode.J)) {
				currentMode = JeepMode.BRAKING;
			} else {
				currentMode = JeepMode.DRIVING;
			}
		}
		switch (currentMode) {
			case JeepMode.DRIVING:
				Deccelerate ();
				break;
			case JeepMode.ACCELERATING:
				Accelerate ();
				break;
			case JeepMode.BRAKING:
				Brake ();
				break;
			}
	}

	public IEnumerator Flash() {
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 0f, 255f);
		yield return new WaitForSeconds(0.1f);
	}

	void SetCurrentSpeed(){
		JointMotor2D motor1 = backWheel.motor;
		JointMotor2D motor2 = frontWheel.motor;
		motor1.motorSpeed = currentSpeed;
		motor2.motorSpeed = currentSpeed;
		backWheel.motor = motor1;
		frontWheel.motor = motor2;
	}

	bool IsEnterable(){
		if (transform.localRotation.eulerAngles.z >= 290f || transform.localRotation.eulerAngles.z < 70f) {
			return true;
		} else {
			StopMotor ();
			currentMode = JeepMode.DRIVING;
			return false;
		}
	}

	void GetInOrOut(){
		Debug.Log ("GET IN");
		if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown(KeyCode.Joystick1Button17)) && playerIsPresent && driver == null && enterable) {
			if (player.GetComponent<PlayerController> ().pickedUp != null) {
				player.GetComponent<PlayerController> ().Throw ();
			}
			myAnim.SetBool ("player", true);
			on = true;
			player.GetComponent<PlayerController>().currentMode = PlayerMode.DRIVING;
			player.GetComponent<PlayerController> ().locker = this.transform;
			gameObject.tag = "PlayerDriving";
			player.transform.localScale = new Vector3 (0f, 0f, 0f);
			player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
			player.GetComponent<BoxCollider2D> ().enabled = false;
			player.GetComponent<CircleCollider2D> ().enabled = false;
			driver = player;
			StartMotor ();
			if (instance != null) {
				Destroy (instance);
				instance = null;
			}
			currentPointer = Instantiate (pointer, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
			currentPointer.transform.parent = transform;
		} else if ((Input.GetKeyDown (KeyCode.K) || Input.GetKeyDown(KeyCode.Joystick1Button17)) && driver != null || !enterable && driver != null ) {
			myAnim.SetBool ("player", false);
			player.GetComponent<PlayerController>().currentMode = PlayerMode.PLAYER;
			player.GetComponent<PlayerController> ().locker = null;
			player.transform.localScale = new Vector3 (8f, 8f, 8f);
			gameObject.tag = "Untagged";
			player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
			player.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
			player.transform.parent = null;
			player.GetComponent<BoxCollider2D> ().enabled = true;
			player.GetComponent<CircleCollider2D> ().enabled = true;
			driver = null;
			if (instance == null) {
				instance = Instantiate (buttonDisplay, new Vector3 (transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
				instance.transform.parent = transform;
			}
			Destroy (currentPointer);
		}
	}

	void Accelerate(){
		if (currentSpeed < maxSpeed) {
			currentSpeed += 10f;
			SetCurrentSpeed ();
		} else {
			SetCurrentSpeed ();
		}
	}

	void Brake(){
		if (currentSpeed > 0) {
			currentSpeed -= 40f;
			SetCurrentSpeed ();
		} else {
			SetCurrentSpeed ();
		}
		if (currentSpeed <= 0 && currentSpeed > -maxSpeed) {
			currentSpeed -= 40f;
			SetCurrentSpeed ();
		} else {
			SetCurrentSpeed ();
		}
	}

	void Deccelerate(){
		if (currentSpeed > 0) {
			currentSpeed -= 10f;
			SetCurrentSpeed ();
		} else {
			SetCurrentSpeed ();
		}
		if (currentSpeed < 0) {
			currentSpeed += 10f;
			SetCurrentSpeed ();
		} else {
			SetCurrentSpeed ();
		}
	}

	public void StartMotor(){
		backWheel.useMotor = true;
		frontWheel.useMotor = true;
		JointMotor2D motor1 = backWheel.motor;
		JointMotor2D motor2 = frontWheel.motor;
		motor1.motorSpeed = 0f;
		motor2.motorSpeed = 0f;
		backWheel.motor = motor1;
		frontWheel.motor = motor2;
	}

	public void StopMotor(){
		backWheel.useMotor = false;
		frontWheel.useMotor = false;
		JointMotor2D motor1 = backWheel.motor;
		JointMotor2D motor2 = frontWheel.motor;
		motor1.motorSpeed = 0f;
		motor2.motorSpeed = 0f;
		backWheel.motor = motor1;
		frontWheel.motor = motor2;
		currentSpeed = 0f;
		SetCurrentSpeed ();
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player" && !playerIsPresent && driver == null && enterable) {
			playerIsPresent = true;
			if (instance == null) {
				instance = Instantiate (buttonDisplay, new Vector3 (transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
				instance.transform.parent = transform;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 5f){
			Damage (5);
		}
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 20f){
			coll.otherCollider.gameObject.SendMessage ("Damage", 5);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			playerIsPresent = false;
			if (instance != null) {
				Destroy (instance);
				instance = null;
			}
		}
	}
}
