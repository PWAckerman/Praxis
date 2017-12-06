using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum HelicopterMode {
	ASCENDING,
	DESCENDING,
	ENEMY,
	DRIVING
}

public class Helicopter : MonoBehaviour, IDamageable, IBurnable {

	public GameObject driver;
	public GameObject player;
	public GameObject pointer;
	public GameObject buttonDisplay;
	public GameObject explosion;
	public GameObject fire;
	public GameObject dust;
	public GameObject door;
	public GameObject[] cargo;
	public AlertManager alertManager;
	public int capacity;
	public int maxHitPoints;
	public int hitPoints;
	public int patrolDistance;
	public float maxSpeed;
	public float currentSpeed = 0;
	public bool on;
	public bool playerIsPresent;
	public bool enterable;
	public bool burning;
	public Animator myAnim;
	public GasEmitter emitter;
	bool paused;
	GameObject instance;
	GameObject currentPointer;
	Coroutine cargoDump;
	public HelicopterMode currentMode;
	public EnemyMode enemyMode;
	public Vector3 target;
	public float distanceToOrigin;
	Vector3 initialPosition;

	void Start () {
		emitter = GetComponent<GasEmitter> ();
		hitPoints = maxHitPoints;
		initialPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		enterable = IsEnterable ();
		if (currentMode != HelicopterMode.ENEMY) {
			GetInOrOut ();
			Drive ();
		} else {
			switch (enemyMode) {
			case EnemyMode.ALERTED:
				target = new Vector3 (alertManager.lastSeenPosition.x, alertManager.lastSeenPosition.y, transform.position.z);
				break;
			case EnemyMode.FLEEING:

				break;
			case EnemyMode.TRANQUILIZED:
				break;
			case EnemyMode.CONFUSED:
				enemyMode = EnemyMode.PATROLLING;
				break;
			case EnemyMode.FUCKED:
				target = transform.position;
				break;
			case EnemyMode.PURSUING:
				target = new Vector3 (alertManager.lastSeenPosition.x, alertManager.lastSeenPosition.y, transform.position.z);
				Pursue ();
				break;
			case EnemyMode.PATROLLING:
				Patrol ();
				break;
			case EnemyMode.SURPRISED:
				break;
			case EnemyMode.STASHED:
				break;
			}

			switch (alertManager.currentMode) {
				case AlertMode.HIGH_ALERT:
					enemyMode = EnemyMode.PURSUING;
					break;
				case AlertMode.PATROLLING:
					if (enemyMode != EnemyMode.FLEEING && enemyMode != EnemyMode.SURPRISED && enemyMode != EnemyMode.FLEEING && enemyMode != EnemyMode.FUCKED && enemyMode != EnemyMode.TRANQUILIZED) {
//						ForgetTarget ();
						target = initialPosition;
						enemyMode = EnemyMode.PATROLLING;
						CloseDoor ();
					}
					if (enemyMode == EnemyMode.FUCKED) {
//						ForgetTarget ();
					}
					break;
				}
			}
	}

	public void OpenDoor(){
		Debug.Log ("OpenDoor");
		door.GetComponent<Animator> ().SetBool ("open", true);
		if (cargoDump == null) {
			cargoDump = StartCoroutine ("InstantiateCargo");
		}
	}

	public void CloseDoor(){
		Debug.Log ("CloseDoor");
		door.GetComponent<Animator> ().SetBool ("open", false);
		cargoDump = null;
	}

	public void ForgetTarget(){
			enemyMode = EnemyMode.PATROLLING;
			target = new Vector3 (initialPosition.x - patrolDistance, transform.position.y, transform.position.z);
	}

	public void Pursue(){
		FaceTarget ();
		float distance = Vector3.Distance (target, transform.position);
		Debug.Log ("distance");
		Debug.Log (distance);

		if (distance > 100f) {
			ForgetTarget ();
			target = initialPosition;
//			enemyMode = EnemyMode.PATROLLING;
		}
		if (Mathf.Abs (distance) > 20f || target == initialPosition) {
			if (target == initialPosition) {
				MoveTowards (0.5f);
			} else {
				MoveTowards ();
			}
		} else if (Mathf.Abs (distance) <= 20f && Mathf.Abs (distance) > 1f) {
			OpenDoor ();
			MoveTowards ();
		} else if (Mathf.Abs (distance) <= 1) {
		
		}
	}

	public void Patrol(){
		distanceToOrigin = Mathf.Abs(Vector3.Distance (initialPosition, transform.position));
		float distance = Mathf.Abs(Vector3.Distance (transform.position, target));
		if (distance < 1f) {
			StartCoroutine ("Pause");
		}
		if (distanceToOrigin < 1f) {
			target = new Vector3 (initialPosition.x - patrolDistance, initialPosition.y, initialPosition.z);
		} else if (distanceToOrigin > (patrolDistance - 5)) {
			target = initialPosition;
		}
		if (!paused) {
			FaceTarget ();
			MoveTowards ();
		}
	}

	IEnumerator Pause(){
		paused = true;
		yield return new WaitForSeconds(2f);
		paused = false;
		yield return new WaitForSeconds(0.1f);

	}

	public void FaceTarget(){
		if (target.x < transform.position.x) {
			FlipLeft ();
		}
		if (target.x >= transform.position.x) {
			FlipRight ();
		}
	}

	void MoveTowards(float speed = 0.2f){
		if (Time.timeScale > 0) {
			if (Mathf.Abs (transform.position.y - target.y) > 10f || target == initialPosition) {
				transform.position = Vector3.MoveTowards (transform.position, target, speed);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (target.x, transform.position.y, target.z), speed);
			}
		}
		//		rb.MovePosition(target.position);
	}

	void FlipLeft(){
		Vector3 scl = transform.localScale;
		transform.localScale = new Vector3 (-Mathf.Abs(scl.x), scl.y, scl.z);
	}

	void FlipRight(){
		Vector3 scl = transform.localScale;
		transform.localScale = new Vector3 (Mathf.Abs(scl.x), scl.y, scl.z);
	}
//		if (currentMode == JeepMode.ACCELERATING && driver == player) {
//			myAnim.SetBool ("backup", false);
//		} else if (currentMode == JeepMode.BRAKING && driver == player) {
//			myAnim.SetBool ("driving", false);
//			myAnim.SetBool ("backup", true);
//		} else if (currentMode == JeepMode.DRIVING && driver == player) {
//			myAnim.SetBool ("driving", true);
//			myAnim.SetBool ("backup", false);
//		} else if (driver != player) {
//			myAnim.SetBool ("driving", false);
//			myAnim.SetBool ("backup", false);
//		}

	public void Damage(int amount = 1){
		if (hitPoints > 0) {
			hitPoints -= amount;
			StartCoroutine ("Flash");
		}
		if (hitPoints < (maxHitPoints / 2)) {
			emitter.TurnOn ();
		}
		if (hitPoints <= 0) {
			StartCoroutine ("FlashDie");
		}
	}

	public void Burn(){
		if (!isBurning ()) {
			burning = true;
			Damage ();
			StartCoroutine ("StopBurning");
		}
	}

	IEnumerator InstantiateCargo(){
		var count = 0;
		while (count < cargo.ToList().Count) {
			Instantiate (cargo [count], door.transform.position, Quaternion.identity);
			count++;
			yield return new WaitForSeconds(1f);
		}
		cargo = new GameObject[0];
	}

	public bool isBurning(){
		return burning;
	}

	IEnumerator StopBurning(){
		yield return new WaitForSeconds(3f);
		burning = false;
	}

	public void Die(){
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
		if (driver != null) {
			Debug.Log ("Accelerate or Brake");
			if (Input.GetAxis ("Braking") > 0 || Input.GetKey (KeyCode.H)) {
				currentMode = HelicopterMode.ASCENDING;
			} else if (Input.GetAxis ("Aiming") > 0 || Input.GetKey (KeyCode.J)) {
				currentMode = HelicopterMode.DESCENDING;
			} else {
				currentMode = HelicopterMode.DRIVING;
			}
		}
		switch (currentMode) {
		case HelicopterMode.DRIVING:
			Deccelerate ();
			break;
		case HelicopterMode.ASCENDING:
			Ascend ();
			break;
		case HelicopterMode.DESCENDING:
			Descend ();
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

	public IEnumerator FlashDie() {
		GetComponent<Rigidbody2D> ().gravityScale = 1f;
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
		Die ();
		yield return new WaitForSeconds(0.1f);
	}

	bool IsEnterable(){
		if (transform.localRotation.eulerAngles.z >= 290f || transform.localRotation.eulerAngles.z < 70f && currentMode != HelicopterMode.ENEMY) {
			return true;
		} else {
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
			player.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
			player.transform.parent = null;
			player.GetComponent<BoxCollider2D> ().enabled = true;
			player.GetComponent<CircleCollider2D> ().enabled = true;
			driver = null;
			if (instance == null) {
				instance = Instantiate (buttonDisplay, new Vector3 (transform.position.x + 3f, transform.position.y + 4.5f, transform.position.z), Quaternion.identity);
				instance.transform.parent = transform;
			}
			Destroy (currentPointer);
		}
	}

	void Ascend(){
		float move = Input.GetAxis ("Horizontal");
		if(GetComponent<Rigidbody2D> ().velocity.y < 1000f){
			GetComponent<Rigidbody2D> ().AddForce ( new Vector2(move * maxSpeed, 500f), ForceMode2D.Force);
		}
	}

	void Descend(){
		float move = Input.GetAxis ("Horizontal");
		if (GetComponent<Rigidbody2D> ().velocity.y > -1000f) {
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (move * maxSpeed, -500f), ForceMode2D.Force);
		}
	}

	void Deccelerate(){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 0f);
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player" && !playerIsPresent && driver == null && enterable) {
			playerIsPresent = true;
			if (instance == null) {
				instance = Instantiate (buttonDisplay, new Vector3 (transform.position.x + 3f, transform.position.y + 4.5f, transform.position.z), Quaternion.identity);
				instance.transform.parent = transform;
			}
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
