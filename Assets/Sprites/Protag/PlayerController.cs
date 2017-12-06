using UnityEngine;
using UnityEngine.UI;
 
using System.Collections;
using System.Linq;

public enum PlayerMode{
	PLAYER,
	ELEVATOR,
	LADDER,
	PICKUP,
	DRIVING,
	DRONE,
	LOCKER
}

public class PlayerController : MonoBehaviour, IBurnable, IElectrocutable, IDamageable {

	public Rigidbody2D myRB;
	public SpriteRenderer myRenderer;
	public ResourceManager rm;
	public Animator myAnim;
	public BoxCollider2D box;
	public CircleCollider2D footing;
	public ProjectileManager projManager;
	public GameModeManager gm;
	public GameObject ui;
	public GameObject oxygenUi;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public float maxSpeed;
	public float initialSpeed;
	public float turning;
	public float jumpPower;
	public float fireRate = 0.5f;
	public float nextFire = 0f;
	public int aimingDirection;
	public int healthCapacity = 8;
	public int health;
	public float oxygenCapacity = 5f;
	public float oxygen;
	public AudioSource asrc;
	public AudioClip pickupSnd;
	public AudioClip jumpingSnd;
	public AudioClip landingSnd;
	public bool wasPressedLastUpdate = false;
	public bool grounded;
	public bool platformed;
	public bool walled;
	public bool lava;
	public bool aiming;
	public bool dead;
	public LayerMask groundLayer;
	public LayerMask platformLayer;
	public Transform groundCheck;
	public LayerMask wallLayer;
	public LayerMask lavaLayer;
	public PlayerMode currentMode;
	public Transform locker;
	public GameObject resources;
	public GameObject money;
	public GameObject projectileOriginator;
	public GameObject drone;
	public IPickupable pickedUp;
	public GameObject pickUpPublic;
	[SerializeField]
	float timeScale;


	bool facingRight = true;
	bool crouching = false;
	bool firstFadeOut;
	bool burning;
	bool underWater = false;
	float groundCheckRadius = 0.2f;
	float fadeOut;

	InventoryManager im;

	ProjectileOriginator po;
	AudioManager am;
	GameObject myDrone;
	GameObject ladder;
	Coroutine water;


	// Use this for initialization
	void Start () {
		im = InventoryManager.GetInstance ();
		po = projectileOriginator.GetComponent<ProjectileOriginator> ();
		myRB = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer> ();
		myAnim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider2D> ();
		footing = GetComponent<CircleCollider2D> ();
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager>();
		projManager = new ProjectileManager ();
		rm = ResourceManager.getResourceManager ();
		gm = GameModeManager.getInstance ();
		initialSpeed = maxSpeed;
		asrc = GetComponent<AudioSource> ();
		pickupSnd = asrc.clip;
		jumpingSnd = Resources.Load<AudioClip>("jumping");
		landingSnd = Resources.Load<AudioClip>("landing");
	}

	public void EnterWater(){
		if (!underWater) {
			maxSpeed = initialSpeed - 5f;
			myRB.gravityScale = 1f;
			underWater = true;
			oxygenUi.GetComponent<FadeAll> ().currentMode = FadeAll.FadeMode.FADEIN;
			water = StartCoroutine ("LoseOxygen");
		}
	}

	public void ExitWater(){
		StopCoroutine (water);
		maxSpeed = initialSpeed;
		myRB.gravityScale = 4f;
		underWater = false;
		oxygen = oxygenCapacity;
		oxygenUi.GetComponent<FadeAll> ().currentMode = FadeAll.FadeMode.FADEOUT;
	}

	IEnumerator LoseOxygen(){
		while (underWater) {
			if (oxygen > 0f) {
				oxygen--;
			} else if (oxygen == 0f) {
				Damage ();
			}
			yield return new WaitForSeconds (2f);
		}
	}

	float CalculateAngle(float stickAngle){
		if (stickAngle == 0f && !facingRight) {
			return 0f;
		}
		if (stickAngle == 0f && facingRight) {
			return 0f;
		}
		if (stickAngle == 180f && !facingRight) {
			return 0f;
		}
		if (stickAngle == 180f && facingRight) {
			return 0f;
		}
		if(stickAngle > 180f && facingRight){
			return stickAngle - 360f;
		}
		if(stickAngle <= -90f && !facingRight){
			return -(180f + stickAngle);
		}
		if(stickAngle < 180f && facingRight){
			return stickAngle;
		}
		if(stickAngle < 180f && !facingRight){
			return 180f - stickAngle;
		}
		return stickAngle;
	}
	
	// Update is called once per frame
	void Update () {
		if (pickedUp != null) {
			pickUpPublic = pickedUp.GetGameObject ();
		} else {
			pickUpPublic = null;
		}
		if (Time.timeScale > 0) {
			if (currentMode == PlayerMode.LOCKER) {

			}
			if (currentMode == PlayerMode.ELEVATOR) {
				if (myDrone && !myDrone.GetComponent<ReconnaissanceDrone> ().GetComponent<Animator> ().GetBool ("dying")) {
					myDrone.GetComponent<ReconnaissanceDrone> ().Die ();
				}
				if (pickedUp != null) {
					if (pickedUp.GetGameObject ().GetComponent<IEnemy> () == null) {
						Throw ();
					}
				}
			}
			if (currentMode == PlayerMode.PLAYER) {
				float move = Input.GetAxis ("Horizontal");
				float crouch = Input.GetAxis ("Vertical");
				float stickAngle = Mathf.Round (Mathf.Atan2 (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal")) * Mathf.Rad2Deg / 45f) * 45f;
				myAnim.SetFloat ("angle", CalculateAngle(stickAngle));
				bool fire = Input.GetKey (KeyCode.Joystick1Button19) || Input.GetKey (KeyCode.K);
				aiming = Input.GetAxis ("Aiming") > 0 || Input.GetKey (KeyCode.H);
				bool pistol = Input.GetKey (KeyCode.Joystick1Button1);
				bool running = Input.GetKey (KeyCode.Joystick1Button18) || Input.GetKey (KeyCode.L);
				bool pulling = Input.GetKey (KeyCode.Joystick1Button13) || Input.GetKey (KeyCode.F);
				bool jumping = Input.GetKeyDown (KeyCode.Joystick1Button16) || Input.GetKey (KeyCode.J);
				float notMoving = (Mathf.Abs (myRB.velocity.x) + Mathf.Abs (myRB.velocity.y));

				bool wasPressedThisUpdate = pistol;
				InventoryCycle ();
				Running (running);
				if (!firstFadeOut) {
					fadeOut = Time.time + 10f;
					firstFadeOut = true;
				}
				if ((Time.time > fadeOut)) {
					FadeChildren ();
					money.GetComponent<Text> ().CrossFadeAlpha (0, .5f, true);
					if (!underWater) {
						oxygenUi.GetComponent<FadeAll> ().currentMode = FadeAll.FadeMode.FADEOUT;
					}
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
				myAnim.SetBool ("aiming", im.HasWeapon() ? aiming : false);
				myAnim.SetBool ("running", running);
				myAnim.SetBool ("pulling", pulling);
				myAnim.SetFloat ("notMoving 0", notMoving);

				if (aiming && pickedUp == null) {
			
					myAnim.SetInteger ("aimingDirection", Mathf.RoundToInt (crouch));
					if (fire) {
						Fire (stickAngle);
					}
					if (Input.GetKey (KeyCode.C) || Input.GetKey (KeyCode.Joystick1Button17)) {
						ThrowGrenade (stickAngle);
					}
				}

				if (pickedUp != null && Input.GetKeyDown (KeyCode.Joystick1Button17)) {
					Throw ();
				}
			}
			if (currentMode == PlayerMode.DRONE) {
				Debug.Log ("SWITCH MODES");
				if (Input.GetKeyDown (KeyCode.Joystick1Button13)) {
					currentMode = PlayerMode.PLAYER;
					gameObject.tag = "Player";
				}
			} else if ((Input.GetKeyDown (KeyCode.Joystick1Button12) || Input.GetKeyDown (KeyCode.U)) && currentMode == PlayerMode.PLAYER) {
				DeployDrone ();
			}
			if (health <= 0 && !dead) {
				Die ();
			}
			if (currentMode == PlayerMode.LADDER) {
				Debug.Log ("ladder");
				float climbSpeed = maxSpeed * Time.deltaTime;
				if (Input.GetAxis ("Vertical") > 0) {
					Debug.Log ("ladder up");
					transform.Translate (Vector3.up * 0.2f);
					if (transform.position.y > ladder.GetComponent<Collider2D> ().bounds.max.y || transform.position.y < ladder.GetComponent<Collider2D> ().bounds.min.y) {
						NotClimbing ();
					}
				} else if (Input.GetAxis ("Vertical") < 0) {
					Debug.Log ("ladder down");
					transform.Translate (-Vector3.up * 0.2f);
					if (transform.position.y > ladder.GetComponent<Collider2D> ().bounds.max.y || transform.position.y < ladder.GetComponent<Collider2D> ().bounds.min.y) {
						NotClimbing ();
					}
				}
			}
			if (currentMode == PlayerMode.PICKUP) {
				currentMode = PlayerMode.PLAYER;
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
		myRB.velocity = new Vector2 (0f, 0f);
		myRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;	
		myRB.gravityScale = 0;
		myAnim.SetBool ("climbing", true);
		transform.position = new Vector3 (ladder.transform.position.x, transform.position.y > ladder.transform.position.y ? transform.position.y - 1f : transform.position.y + 1f, transform.position.z);
	}	

	void Pickup(GameObject obj){
		if (currentMode == PlayerMode.PLAYER) {
			Rigidbody2D picked = obj.GetComponent<Rigidbody2D> ();
			pickedUp = obj.GetComponent<IPickupable>();
			picked.transform.localRotation = Quaternion.identity;
			picked.velocity = new Vector2 (0f,0f);
			picked.bodyType = RigidbodyType2D.Kinematic;
			picked.constraints = RigidbodyConstraints2D.FreezeAll;
			obj.transform.parent = transform;
			obj.transform.localPosition = new Vector3 (0f, 0.52f, 0f);
			currentMode = PlayerMode.PICKUP;
		}
	}
		

	public void Throw(){
		myAnim.SetBool ("throwing", true);
		pickedUp.Throw (facingRight, 10f);
		pickedUp = null;
	}

	public void NotThrowing(){
		myAnim.SetBool ("throwing", false);
	}

	void NotClimbing(){
		ladder = null;
		myAnim.SetBool ("climbing", false);
		currentMode = PlayerMode.PLAYER;
		myRB.constraints = RigidbodyConstraints2D.FreezeRotation;
		myRB.isKinematic = false;
		myRB.gravityScale = 4f;
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
		if (im.HasAmmunition ()) {
			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				aimingDirection = Mathf.RoundToInt (Input.GetAxis ("Vertical"));
				if (stickAngle == 0 && !facingRight) {
					stickAngle = 180f;
				}
				po.PositionByAngle ((int)myAnim.GetFloat("angle"), crouching);
				if (!facingRight) {
					po.Flip ();
				}
				im.SubtractAmmunition ();
				projManager.Fire (facingRight, Mathf.RoundToInt (Input.GetAxis ("Vertical")), (int)stickAngle, po.GetPosition (), MapProjectileMode());
			}
		}
	}

	ProjectileManager.Mode MapProjectileMode(){
		if(im.currentWeapon.name == "tranquilizer"){
			return ProjectileManager.Mode.TRANQUILIZER;
		}
		if (im.currentWeapon.name == "pistol") {
			return ProjectileManager.Mode.PROJECTILE1;
		} else {
			return ProjectileManager.Mode.PROJECTILE1;
		}
	}

	ProjectileManager.Mode MapGrenadeMode(){
		if(im.currentGrenade.name == "grenade"){
			return ProjectileManager.Mode.GRENADE;
		}
		if (im.currentGrenade.name == "chaff") {
			return ProjectileManager.Mode.CHAFF_GRENADE;
		} else {
			return ProjectileManager.Mode.GRENADE;
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
		oxygenUi.GetComponentsInChildren<Image> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (1, 0.5f, true);
		});
		resources.GetComponentsInChildren<Image> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (1, 0.5f, true);
		});
		resources.GetComponentsInChildren<Text> ().ToList ().ForEach (img => {
			img.CrossFadeAlpha (1, 0.5f, true);
		});
	}

	public void ThrowGrenade(float stickAngle){
		if(im.HasGrenades()){
			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				if (stickAngle == 0 && !facingRight) {
					stickAngle = 180f;
				}
				po.PositionByAngle ((int)stickAngle, crouching);
				projManager.Fire (facingRight, Mathf.RoundToInt (Input.GetAxis ("Vertical")), (int)stickAngle, po.GetPosition (), MapGrenadeMode());
				im.SubtractGrenade ();
			}
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

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.GetComponent<IPickupable> () != null && Input.GetKeyDown(KeyCode.Joystick1Button17) && pickedUp == null) {
			coll.gameObject.GetComponent <IPickupable> ().Pickup (gameObject);
		}
		if (coll.gameObject.GetComponent<IStashTarget> () != null && Input.GetKeyDown (KeyCode.Joystick1Button17) && pickedUp != null){
			if(pickedUp.GetGameObject ().GetComponent<IStashable> () != null) {
				coll.gameObject.GetComponent<IStashTarget> ().StashInside(pickedUp.GetGameObject ().GetComponent<IStashable>());
				pickedUp.GetGameObject ().transform.parent = null;
				pickedUp.GetGameObject ().SetActive (false);
				pickedUp = null;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.GetComponent<WeaponPickup> () != null) {
			im.AddWeapon (coll.gameObject.GetComponent<WeaponPickup>().weapon);
			Destroy (coll.gameObject, 0.1f);
		}
		if (coll.gameObject.GetComponent<ItemPickup> () != null) {
			im.AddItem (coll.gameObject.GetComponent<ItemPickup>().item);
			Destroy (coll.gameObject.transform.parent.gameObject, 0.1f);
		}
		if (coll.gameObject.GetComponent<Ammunition> () != null) {
			im.AddAmmunition (coll.gameObject.GetComponent<Ammunition> ());
			Destroy (coll.gameObject.transform.parent.gameObject, 0.1f);
		}
		if (coll.gameObject.GetComponent<GrenadePickup> () != null) {
			Debug.Log ("Grenade Pickup");
			im.AddGrenade (coll.gameObject.GetComponent<GrenadePickup> ().grenade);
			Destroy (coll.gameObject.transform.parent.gameObject, 0.1f);
		}
	}

	void Crouch(float crouch){
		if (crouch < 0 && crouching == false && pickedUp == null) {
			crouching = true;
			box.enabled = false;
			Vector3 pos = projectileOriginator.transform.position;
			projectileOriginator.transform.position = new Vector3 (pos.x, pos.y - 0.7f, pos.z);
		} else if  (crouch >= 0 && crouching == true){
			crouching = false;
			box.enabled = true;
			Vector3 pos = projectileOriginator.transform.position;
			projectileOriginator.transform.position = new Vector3 (pos.x, pos.y + 0.7f, pos.z);
		}
	}

	void InventoryCycle(){
		if(Input.GetKeyDown(KeyCode.Joystick1Button7)){
			im.PrevWeapon ();
		}
		if(Input.GetKeyDown(KeyCode.Joystick1Button8)){
			im.NextWeapon ();
		}
		if(Input.GetKeyDown(KeyCode.Joystick1Button5)){
			im.NextGrenade ();
		}
		if(Input.GetKeyDown(KeyCode.Joystick1Button6)){
			im.PrevGrenade ();
		}
	}

	void Running(bool running){
		if (running) {
			maxSpeed = 15f;
			if (underWater) {
				maxSpeed = 15f - 8f;
			}
		} else {
			maxSpeed = initialSpeed;
			if (underWater) {
				maxSpeed = initialSpeed - 5f;
			}
		}
	}

	void Jump(){
		if (pickedUp == null && !aiming) {
			asrc.clip = jumpingSnd;
			asrc.Play ();
			myAnim.SetBool ("grounded", false);
			myRB.velocity = new Vector2 (myRB.velocity.x, 0);
			myRB.AddForce (new Vector2 (Mathf.Clamp (myRB.velocity.x * 5f, 1f, 30f), jumpPower), ForceMode2D.Impulse);
			grounded = false;
		}
	}

	void Die(){
		myAnim.SetBool ("dead", true);
		am.PlayWithLocalAudioSource (asrc, "playerDie");
		box.enabled = false;
		dead = true;
		gameObject.tag = "DeadPlayer";
		Time.timeScale = 0.5f;
		if (pickedUp != null) {
			Throw ();
		}
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
