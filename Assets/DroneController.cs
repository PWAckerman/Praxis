using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class DroneController : MonoBehaviour, IDamageable {

	enum Mode{
		PATROLLING,
		PURSUING,
		FIRING,
		FUZZED
	}
	// Use this for initialization
	Transform transform;
	Animator myAnim;
	Rigidbody2D rb;
	Vector3 target;
	AudioManager am;
	ProjectileManager projManager;
	Vector3 initialPosition;
	Mode currentMode;
	public float patrolDistance;
	ProjectileOriginator po;
	public Loot[] loot;
	public int dropRate;
	public GameObject altDrop;
	public float perceptionRange;
	public float FiringDelay;
	public float timer;
	public float initialScale;
	int hitPoints;
	public int maxHitPoints;
	public bool onTheWay;
	bool activated;
	bool dying;
	public GameObject explosion;
	public GameObject exclamation;
	public GameObject question;
	public GameObject instance;
	public GameObject projectileOriginator;


	void Start () {
		po = projectileOriginator.GetComponent<ProjectileOriginator> ();
		transform = GetComponent<Transform> ();
		initialPosition = transform.position;
		myAnim = GetComponent<Animator> ();
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		rb = GetComponent<Rigidbody2D> ();
		hitPoints = maxHitPoints;
		activated = false;
		projManager = new ProjectileManager ();
		timer = Time.time;
		currentMode = Mode.PATROLLING;
		initialScale = rb.gravityScale;
	}

	// Update is called once per frame
	void Update () {
		switch (currentMode) {
			case Mode.PATROLLING:
				Patrol ();
				break;
			case Mode.FUZZED:
				Fuzz ();
				break;
			case Mode.PURSUING:
				Pursue ();
				break;
		}
	}

	public void Fuzz(){
		rb.gravityScale = 3;
	}

	public void Pursue(){
		rb.gravityScale = initialScale;
		myAnim.SetBool ("alerted", true);
		if (target == null) {
			currentMode = Mode.PATROLLING;
		}
		if (!onTheWay) {
			onTheWay = true;
		}
		if (Time.time - timer > FiringDelay && target != initialPosition) {
			timer = Time.time;
			Fire ();
		}
		if (transform.position.x < target.x) {
			GetComponent<SpriteRenderer> ().flipX = true;
		} else {
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		float distance = Vector3.Distance (target, transform.position);
		Debug.Log ("distance");
		Debug.Log (distance);

		if (distance > 20f) {
			ForgetTarget ();
			target = initialPosition;
			onTheWay = false;
			currentMode = Mode.PATROLLING;
		}
		if (Mathf.Abs (distance) > 5f || target == initialPosition) {
			if (target == initialPosition) {
				MoveTowards (0.5f);
			} else {
				MoveTowards ();
			}

		}
	}

	public void Fire(){
		bool facingRight = GetComponent<SpriteRenderer> ().flipX;
		float angle = Vector3.Angle (target, transform.position);
		projManager.Fire (facingRight, 1, facingRight ? -(int)angle : 180 + (int)angle, po.GetPosition(), ProjectileManager.Mode.ENEMY);
	}

	void MoveTowards(float speed = 0.2f){
		transform.position = Vector3.MoveTowards(transform.position, target, speed);
//		rb.MovePosition(target.position);
	}

	void Patrol(){
		rb.gravityScale = initialScale;
		myAnim.SetBool ("alerted", false);
		float distanceToOrigin = Vector3.Distance (initialPosition, transform.position);
		if (distanceToOrigin < 1) {
			target = new Vector3 (initialPosition.x - patrolDistance, initialPosition.y, initialPosition.z);
		}
		if (distanceToOrigin > (patrolDistance - 1)) {
			target = initialPosition;
		}
		float distance = Vector3.Distance (transform.position, target);
		if (transform.position.x < target.x) {
			GetComponent<SpriteRenderer> ().flipX = true;
		} else if(distance > 1) {
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		MoveTowards ();
	}

	void FixedUpdate(){	

		//Altitude
//		rb.AddForce(0, Input.GetAxis("Vertical") * 5, 0);
	}

	void ResumePatrol(){
		currentMode = Mode.PATROLLING;
	}

	public void Damage(int hit){
		am.Play("hit");
		hitPoints -= hit;
		if (hitPoints <= 0) {
			if (!dying) {
				activated = false;
				StartCoroutine ("FlashDie");
			}
		}
		StartCoroutine ("Flash");
	}

	public void Die(){
		activated = false;
		Destroy (this.gameObject);
	}

	public void ForgetTarget(){
		if (instance != null) {
			Destroy (instance);
		}
		instance = Instantiate (question, new Vector3 (transform.position.x, transform.position.y + 1f), Quaternion.identity);
		instance.transform.parent = transform;
		Destroy (instance, 5f);
	}

	public void NoticeTarget(){
		if (instance != null) {
			Destroy (instance);
		}
		am.Play ("alert");
		instance = Instantiate (exclamation, new Vector3 (transform.position.x, transform.position.y + 1f), Quaternion.identity);
		instance.transform.parent = transform;
	}

	public void Drop(){
		int rand = Random.Range (0, 4);
			loot = LootFactory.GetAssortedRandomLoot (dropRate, this, new GameObject ());
		if(rand > 0) {
			Instantiate (altDrop, transform.position, Quaternion.identity);		
		}
	}
		
	public IEnumerator Flash() {
		rb.gravityScale = 2;
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
		rb.gravityScale = initialScale;
		yield return new WaitForSeconds(0.1f);
	}

	public IEnumerator FlashDie(){
		dying = true;
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
		am.Play("destroyed");
		Drop ();
		GameObject.Find ("ComboManager").SendMessage ("InitiateCombo", this.gameObject);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		Instantiate (explosion, transform.position, transform.rotation);
		Destroy (this.gameObject);
		yield return new WaitForSeconds(0.1f);
	}

	public void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && currentMode != Mode.FUZZED && other.gameObject.GetComponent<PlayerController>().currentMode != PlayerMode.ELEVATOR && other.gameObject.GetComponent<PlayerController>().currentMode != PlayerMode.LOCKER) {
			target = other.transform.position;
			currentMode = Mode.PURSUING;
			NoticeTarget ();
			am.LoopWithLocalAudioSource (GetComponent<AudioSource> (), "droneHovering");
		}
		if (other.gameObject.tag == "Grenade" && currentMode != Mode.FUZZED) {
			target = other.transform.position;
			currentMode = Mode.PURSUING;
			NoticeTarget ();
			am.LoopWithLocalAudioSource (GetComponent<AudioSource> (), "droneHovering");
		}
		if (other.gameObject.tag == "ChaffExplosion") {
			currentMode = Mode.FUZZED;
		}
	}

	public void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().currentMode != PlayerMode.ELEVATOR && other.gameObject.GetComponent<PlayerController>().currentMode != PlayerMode.LOCKER) {
			target = other.transform.position;
		}
	}

	public void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player" && currentMode != Mode.FUZZED) {
			activated = false;
			if (instance != null) {
				Destroy (instance);
			}
			instance = Instantiate (question, new Vector3 (transform.position.x, transform.position.y + 1f), Quaternion.identity);
			instance.transform.parent = transform;
			Destroy (instance, 5f);
			currentMode = Mode.PATROLLING;
		}
		if (other.gameObject.tag == "ChaffExplosion") {
			activated = false;
			if (instance != null) {
				Destroy (instance);
			}
			currentMode = Mode.PATROLLING;
			instance = Instantiate (question, new Vector3 (transform.position.x, transform.position.y + 1f), Quaternion.identity);
			instance.transform.parent = transform;
		}
	}
}
