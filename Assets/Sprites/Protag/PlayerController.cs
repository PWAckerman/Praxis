using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Linq;



public class PlayerController : MonoBehaviour {

	public Rigidbody2D myRB;
	public SpriteRenderer myRenderer;
	public ResourceManager rm;
	public Animator myAnim;
	public BoxCollider2D box;
	public CircleCollider2D footing;
	public ProjectileManager projManager;
	public GameObject ui;
	public int aimingDirection;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public float maxSpeed;
	public float initialSpeed;
	public int healthCapacity = 8;
	public int health;
	public AudioSource asrc;
	public AudioClip pickupSnd;
	public AudioClip jumpingSnd;
	public AudioClip landingSnd;
	bool facingRight = true;
	bool crouching = false;
	public bool wasPressedLastUpdate = false;
	public bool grounded;
	public bool platformed;
	public bool walled;
	public bool lava;
	public bool aiming;
	public bool dead;
	public float turning;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public LayerMask platformLayer;
	public Transform groundCheck;
	public LayerMask wallLayer;
	public LayerMask lavaLayer;
	public float jumpPower;
	public float fireRate = 0.5f;
	public float nextFire = 0f;
	float fadeOut;
	bool firstFadeOut;
	public GameObject resources;
	public GameObject money;
	// Use this for initialization
	void Start () {
		myRB = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer> ();
		myAnim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider2D> ();
		footing = GetComponent<CircleCollider2D> ();
		projManager = new ProjectileManager (this);
		rm = ResourceManager.getResourceManager ();
		initialSpeed = maxSpeed;
		asrc = GetComponent<AudioSource> ();
		pickupSnd = asrc.clip;
		jumpingSnd = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/jumping.wav");
		landingSnd = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/landing.wav");
	}
	
	// Update is called once per frame
	void Update () {
		float move = Input.GetAxis ("Horizontal");
		float crouch = Input.GetAxis ("Vertical");
		bool fire = Input.GetKey (KeyCode.Joystick1Button3);
		aiming = Input.GetKey (KeyCode.Joystick1Button15);
		bool pistol = Input.GetKey (KeyCode.Joystick1Button1);
		bool running = Input.GetKey (KeyCode.Joystick1Button2);
		bool pulling = Input.GetKey (KeyCode.Joystick1Button14);
		bool jumping = Input.GetKeyDown (KeyCode.Joystick1Button0);
		float notMoving = (Mathf.Abs (myRB.velocity.x) + Mathf.Abs (myRB.velocity.y));

		bool wasPressedThisUpdate = pistol;

		Running (running);

		if (!firstFadeOut) {
			fadeOut = Time.time + 10f;
			firstFadeOut = true;
		}
		if((Time.time > fadeOut)){
			FadeChildren ();
			money.GetComponent<Text>().CrossFadeAlpha(0, .5f, true);
		}
	
		if(grounded && (jumping) && !dead){
			Jump ();
		}
		if (turning > 0) {
			myAnim.SetBool ("turning", true);
			turning--;
		} else {
			myAnim.SetBool ("turning", false);
		}

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);
		platformed = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, platformLayer);
		walled = Physics2D.OverlapCircle (transform.position, groundCheckRadius + 1, wallLayer);
		lava = Physics2D.OverlapCircle (transform.position, groundCheckRadius, lavaLayer);
		if (lava) {
			Damage ();
		}
		if (platformed) {
			grounded = true;
		}

		if (walled && !grounded && (jumping)) {
			Debug.Log (walled);
//			Jump ();
//			myRB.AddForce (-transform.right * 200, ForceMode2D.Impulse);
			myRB.AddForce (new Vector2 (Mathf.Clamp(-myRB.velocity.x * 5f, 1f, 30f), jumpPower * Input.GetAxis ("Fire1")), ForceMode2D.Impulse);
			myRB.AddForce(new Vector2(-jumpPower * Input.GetAxis ("Fire1") * transform.forward.x, 0f), ForceMode2D.Impulse);
			Flip ();
		}

		myAnim.SetBool ("grounded", grounded);


		Crouch (crouch);
		if (move > 0 && !facingRight && !dead) {
			Flip ();
		} else if (move < 0 && facingRight && !dead) {
			Flip ();
		}

		wasPressedLastUpdate = wasPressedThisUpdate;

		if (aiming == false && crouching == false && dead == false && pulling == false) {
			myRB.velocity = new Vector2 (move * maxSpeed, myRB.velocity.y);
		}

		myAnim.SetFloat ("moveSpeed", Mathf.Abs (move));
		myAnim.SetBool ("crouching", crouching);
		myAnim.SetBool ("aiming", aiming);
		myAnim.SetBool ("running", running);
		myAnim.SetBool ("pulling", pulling);
		myAnim.SetFloat ("notMoving 0", notMoving);

		if (aiming) {
			
			myAnim.SetInteger ("aimingDirection", Mathf.RoundToInt (crouch));
			if (fire) {
				Fire ();
			}
		}
		if (health <= 0) {
			Die ();
		}
		UpdateHud ();
		rm.Run ();
	}

	void Flip () {
		facingRight = !facingRight;
		myRenderer.flipX = !myRenderer.flipX;
		turning = 30f;
	}

	void UpdateHud(){
		Text txtA = GameObject.FindGameObjectWithTag ("ResourceA").GetComponent<Text> ();
		Text txtB = GameObject.FindGameObjectWithTag ("ResourceB").GetComponent<Text> ();
		Text txtC = GameObject.FindGameObjectWithTag ("ResourceC").GetComponent<Text> ();
		Text txtD = GameObject.FindGameObjectWithTag ("ResourceD").GetComponent<Text> ();
		Text txtGold = GameObject.FindGameObjectWithTag ("MyGold").GetComponent<Text> ();
		txtA.text = rm.ResourceA.ToString();
		txtB.text = rm.ResourceB.ToString();
		txtC.text = rm.ResourceC.ToString();
		txtD.text = rm.ResourceD.ToString();
		txtGold.text = rm.Gold.ToString ();
		txtGold.text = "S" + txtGold.text.PadLeft (10, '0');
	}

	void Fire() {
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			aimingDirection = Mathf.RoundToInt (Input.GetAxis ("Vertical"));
			projManager.Fire (facingRight, Mathf.RoundToInt(Input.GetAxis ("Vertical")),Mathf.RoundToInt(Input.GetAxis ("Vertical")));
		}
	}

	void FadeChildren(){
		resources.GetComponentsInChildren<Image> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (0, 0.5f, true);
		});
		resources.GetComponentsInChildren<Text> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (0, 0.5f, true);
		});
	}

	void UnFadeChildren(){
		resources.GetComponentsInChildren<Image> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (1, 0.5f, true);
		});
		resources.GetComponentsInChildren<Text> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (1, 0.5f, true);
		});
	}

	public void Damage() {
		health -= 1;
		StartCoroutine ("Flash");
	}

	public void CollectResource(LootTypes lootT){
		UnFadeChildren ();
		asrc.clip = pickupSnd;
		asrc.Play ();
		fadeOut = Time.time + 10f;
		rm.increaseResource (lootT);
	}

	public void CollectGold(){
		money.GetComponent<Text>().CrossFadeAlpha(1, .5f, true);
		asrc.clip = pickupSnd;
		asrc.Play ();
		fadeOut = Time.time + 10f;
		rm.increaseGold ();
	}

	void OnCollisionEnter2D(Collision2D coll){
		Debug.Log ("RELATIVE VELOCITY");


		if (coll.gameObject.tag == "Platform" && platformed) {
			transform.parent = coll.gameObject.transform;	
		}

		if (coll.gameObject.tag == "Lava") {
			Damage ();
		}

		if (walled && !grounded && Input.GetKeyDown (KeyCode.Joystick1Button0)) {
			Debug.Log ("WALL COLLISION JUMP");
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		transform.parent = null;
	}

	void Crouch(float crouch){
		if (crouch < 0) {
			crouching = true;
			box.enabled = false;
		} else if  (crouch >= 0){
			crouching = false;
			box.enabled = true;
		}
	}

	void Running(bool running){
		if (running) {
			maxSpeed = 15f;
		} else {
			maxSpeed = initialSpeed;
		}
	}

	void Jump(){
		asrc.clip = jumpingSnd;
		asrc.Play ();
		myAnim.SetBool ("grounded", false);
		myRB.velocity = new Vector2 (myRB.velocity.x, 0);
		myRB.AddForce (new Vector2 (Mathf.Clamp(myRB.velocity.x * 5f, 1f, 30f), jumpPower * Input.GetAxis ("Fire1")), ForceMode2D.Impulse);
		grounded = false;
	}

	void Die(){
		myAnim.SetBool ("dead", true);
		box.enabled = false;
		dead = true;
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
		
}
