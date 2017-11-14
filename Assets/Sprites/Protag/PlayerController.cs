using UnityEngine;
using UnityEngine.UI;
 
using System.Collections;
using System.Linq;

public enum PlayerMode{
	PLAYER,
	ELEVATOR,
	LADDER,
	DRONE,
	LOCKER
}

public class PlayerController : MonoBehaviour, IBurnable, IElectrocutable {

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
	public GameObject projectileOriginator;
	ProjectileOriginator po;
	AudioManager am;
	public PlayerMode currentMode;
	public GameObject drone;
	public Transform locker;
	GameObject myDrone;
	GameObject ladder;
	bool burning;
	// Use this for initialization
	void Start () {
		po = projectileOriginator.GetComponent<ProjectileOriginator> ();
		myRB = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer> ();
		myAnim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider2D> ();
		footing = GetComponent<CircleCollider2D> ();
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager>();
		projManager = new ProjectileManager ();
		rm = ResourceManager.getResourceManager ();
		initialSpeed = maxSpeed;
		asrc = GetComponent<AudioSource> ();
		pickupSnd = asrc.clip;
		jumpingSnd = Resources.Load<AudioClip>("jumping");
		landingSnd = Resources.Load<AudioClip>("landing");
	}
	
	// Update is called once per frame
	void Update () {
		if (currentMode == PlayerMode.LOCKER) {
			
		}
		if (currentMode == PlayerMode.ELEVATOR) {
			if (myDrone && !myDrone.GetComponent<ReconnaissanceDrone> ().GetComponent<Animator>().GetBool("dying")){
				myDrone.GetComponent<ReconnaissanceDrone> ().Die ();
			}
		}
		if (currentMode == PlayerMode.PLAYER) {
			float move = Input.GetAxis ("Horizontal");
			float crouch = Input.GetAxis ("Vertical");
			float stickAngle = Mathf.Round (Mathf.Atan2 (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal")) * Mathf.Rad2Deg / 45f) * 45f;
			bool fire = Input.GetKey (KeyCode.Joystick1Button3) || Input.GetKey (KeyCode.K);
			aiming = Input.GetKey (KeyCode.Joystick1Button15) || Input.GetKey (KeyCode.H);
			bool pistol = Input.GetKey (KeyCode.Joystick1Button1);
			bool running = Input.GetKey (KeyCode.Joystick1Button2) || Input.GetKey (KeyCode.L);
			bool pulling = Input.GetKey (KeyCode.Joystick1Button14) || Input.GetKey (KeyCode.F);
			bool jumping = Input.GetKeyDown (KeyCode.Joystick1Button0) || Input.GetKey (KeyCode.J);
			float notMoving = (Mathf.Abs (myRB.velocity.x) + Mathf.Abs (myRB.velocity.y));

			bool wasPressedThisUpdate = pistol;

			Running (running);

			if (!firstFadeOut) {
				fadeOut = Time.time + 10f;
				firstFadeOut = true;
			}
			if ((Time.time > fadeOut)) {
				FadeChildren ();
				money.GetComponent<Text> ().CrossFadeAlpha (0, .5f, true);
			}
	
			if (grounded && (jumping) && !dead) {
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
				myRB.AddForce (new Vector2 (Mathf.Clamp (-myRB.velocity.x * 5f, 1f, 30f), jumpPower * Input.GetAxis ("Fire1")), ForceMode2D.Impulse);
				myRB.AddForce (new Vector2 (-jumpPower * Input.GetAxis ("Fire1") * transform.forward.x, 0f), ForceMode2D.Impulse);
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
					Fire (stickAngle);
				}
				if (Input.GetKey (KeyCode.C) || Input.GetKey (KeyCode.Joystick1Button1)) {
					ThrowGrenade (stickAngle);
				}
			}
		}
		if (currentMode == PlayerMode.DRONE) {
			Debug.Log ("SWITCH MODES");
			if(Input.GetKeyDown(KeyCode.Joystick1Button13)){
				currentMode = PlayerMode.PLAYER;
				gameObject.tag = "Player";
			}
		} else if((Input.GetKeyDown(KeyCode.Joystick1Button13) || Input.GetKeyDown(KeyCode.U)) && currentMode == PlayerMode.PLAYER){
			DeployDrone ();
		}
		if (health <= 0 && !dead) {
			Die ();
		}
		if (currentMode == PlayerMode.LADDER) {
			Debug.Log ("ladder");
			float climbSpeed = maxSpeed * Time.deltaTime;
			if(Input.GetAxis("Vertical") > 0)
			{
				Debug.Log ("ladder up");
				transform.Translate(Vector3.up * 0.2f);
				if (transform.position.y > ladder.GetComponent<Collider2D> ().bounds.max.y || transform.position.y < ladder.GetComponent<Collider2D> ().bounds.min.y) {
					NotClimbing ();
				}
			}
			else if(Input.GetAxis("Vertical") < 0)
			{
				Debug.Log ("ladder down");
				transform.Translate(-Vector3.up * 0.2f);
				if (transform.position.y > ladder.GetComponent<Collider2D> ().bounds.max.y || transform.position.y < ladder.GetComponent<Collider2D> ().bounds.min.y) {
					NotClimbing ();
				}
			}
		}
		rm.Run ();
		UpdateHud ();
	}

	public void Electrocute(){
		Damage ();
	}

	void Flip () {
		facingRight = !facingRight;
		myRenderer.flipX = !myRenderer.flipX;
		turning = 30f;
		po.Flip ();
	}

	void Climbing(GameObject obj){
		ladder = obj;
		currentMode = PlayerMode.LADDER;
		myRB.isKinematic = true;
		myAnim.SetBool ("climbing", true);
		transform.position = new Vector3 (ladder.transform.position.x, transform.position.y, transform.position.z);
	}

	void NotClimbing(){
		ladder = null;
		myAnim.SetBool ("climbing", false);
		currentMode = PlayerMode.PLAYER;
		myRB.isKinematic = false;
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

	void DeployDrone(){
		//INSTANTIATE DRONE PREFAB;
		if (myDrone == null) {
			myDrone = Instantiate (drone, new Vector3 (transform.position.x, transform.position.y + 3f, transform.position.z), Quaternion.identity);
			myDrone.GetComponent<ReconnaissanceDrone>().player = this;
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraFollows> ().secondaryTarget = myDrone.transform;
		}
		currentMode = PlayerMode.DRONE;
		gameObject.tag = "Untagged";
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

	void Fire(float stickAngle) {
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			aimingDirection = Mathf.RoundToInt (Input.GetAxis ("Vertical"));
			if (stickAngle == 0 && !facingRight) {
				stickAngle = 180f;
			}
			po.PositionByAngle ((int)stickAngle, crouching);
			projManager.Fire (facingRight, Mathf.RoundToInt(Input.GetAxis ("Vertical")),(int)stickAngle, po.GetPosition(), ProjectileManager.Mode.PROJECTILE1);
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

	public void ThrowGrenade(float stickAngle){
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if (stickAngle == 0 && !facingRight) {
				stickAngle = 180f;
			}
			po.PositionByAngle ((int)stickAngle, crouching);
			projManager.Fire (facingRight, Mathf.RoundToInt (Input.GetAxis ("Vertical")), (int)stickAngle, po.GetPosition (), ProjectileManager.Mode.GRENADE);
		}
	}

	public void Damage(int amount = 1) {
		health -= 1;
		am.Play ("damage");
		StartCoroutine ("Flash");
	}

	public void CollectResource(LootTypes lootT){
		if (lootT != LootTypes.HEALTH) {
			UnFadeChildren ();
			asrc.clip = pickupSnd;
			asrc.Play ();
			fadeOut = Time.time + 10f;
			rm.increaseResource (lootT);
		} else {
			asrc.clip = pickupSnd;
			asrc.Play ();
			if (health < healthCapacity) {
				health++;
			}
		}
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

		if (coll.gameObject.tag == "Drone") {
			Damage ();
		}

		if (coll.gameObject.tag == "DroneProjectile") {
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
		if (crouch < 0 && crouching == false) {
			crouching = true;
			box.enabled = false;
		} else if  (crouch >= 0 && crouching == true){
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
		am.PlayWithLocalAudioSource (asrc, "playerDie");
		box.enabled = false;
		dead = true;
		gameObject.tag = "DeadPlayer";
		Time.timeScale = 0.5f;
	}

	void EndDie(){
		Debug.Log ("End Die!");
		Time.timeScale = 1;
		Debug.Log (Time.timeScale);
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
