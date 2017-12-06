using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public enum EnemyMode{
	ALERTED,
	CONFUSED,
	FUCKED,
	PATROLLING,
	PURSUING,
	SURPRISED,
	FLEEING,
	FIRING,
	TRANQUILIZED,
	FUZZED,
	STASHED
}

public class MinerController : MonoBehaviour, IEnemy, ISurpriseable, IDamageable, IElectrocutable, IBurnable, ITranquilizeable, IPickupable, IStashable {

	// Use this for initialization
	public Vector3 target;
	public Vector3 lastSeenPosition;
	public Vector3 lastPosition;
	public GameObject exclamation;
	public GameObject question;
	public GameObject zzzz;
	public AudioManager am;
	public AlertManager alertManager;
	public EnemyDialogueManager dm;
	public CapsuleCollider2D originalCollider;
	public CapsuleCollider2D sleepCollider;
	public GameObject publicMarker;
	public GameObject marker {get; set;}
	public GameObject droppable;
	public bool unarmed;
	public bool facingLeft;
	public float patrolDistance;
	public float FiringDelay;
	public int maxHitPoints;
	public int hitPoints;
	Coroutine deferred;
	[SerializeField]
	public EnemyMode currentMode {get; set;}
	public EnemyMode realMode;
	[SerializeField]
	bool paused;
	bool dying;
	bool burning;
	bool forgotten;
	bool isPickedUp;
	[SerializeField]
	float distanceToTarget;
	string collided;
	float timer;
	int sortingOrder;
	Animator myAnim;
	Rigidbody2D rb;
	public Vector3 initialPosition;
	Coroutine tranqRoutine;
	GameObject instance;
	Coroutine flipCycle;
	public GameObject projectileOriginator;
	ProjectileManager projManager;
	ProjectileOriginator po;

	void Start () {
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		alertManager = GameObject.Find ("ComboManager").GetComponent<AlertManager> ();
		myAnim = GetComponent<Animator> ();
		po = projectileOriginator.GetComponent<ProjectileOriginator> ();
		projManager = new ProjectileManager ();
		forgotten = true;
//		flipCycle = StartCoroutine ("FlipCycle");
		facingLeft = false;
		target = new Vector3 (transform.position.x - patrolDistance, transform.position.y, transform.position.z);
		currentMode = realMode;
		initialPosition = transform.position;
		hitPoints = maxHitPoints;
		rb = GetComponent<Rigidbody2D> ();
		paused = false;
		LoadCollider (originalCollider);
		sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder;
		marker = publicMarker;
		if (!unarmed) {
			myAnim.SetBool ("armed", true);
		}
	}

	public void LoadCollider(CapsuleCollider2D loading){
		CapsuleCollider2D component = GetComponent<CapsuleCollider2D> ();
		component.offset = loading.offset;
		component.direction = loading.direction;
		component.size = loading.size;
	}
		
		

	public void ChangeMode(EnemyMode mode){
		currentMode = mode;
		realMode = mode;
	}

	public GameObject GetGameObject(){
		return gameObject;
	}

	public void Tranquilize(float strength){
		currentMode = EnemyMode.TRANQUILIZED;
//		dm.ShowTranquilized ();
		if (tranqRoutine == null) {
			tranqRoutine = StartCoroutine ("Sleep", strength);
		} else {
			StopCoroutine (tranqRoutine);
			tranqRoutine = StartCoroutine ("Sleep", strength);
		}
	}

	public bool IsTranquilized(){
		return currentMode == EnemyMode.TRANQUILIZED;
	}

	public void Pickup(GameObject picker){
		if (IsTranquilized ()) {
			picker.SendMessage ("Pickup", this.gameObject);
			isPickedUp = true;
			GetComponent<SpriteRenderer> ().sortingOrder = picker.GetComponent<SpriteRenderer> ().sortingOrder;
		}
	}

	public void Stash(){
		isPickedUp = false;
		transform.localScale = new Vector3 (0f, 0f, 0f);
		rb.gravityScale = 0f;
		rb.bodyType = RigidbodyType2D.Dynamic;
		ChangeMode (EnemyMode.STASHED);
	}

	public void Release(Transform trans){
		gameObject.SetActive (true);
		transform.parent = null;
		transform.position = trans.position;
		transform.localScale = new Vector3 (8f, 8f, 8f);
		rb.gravityScale = 3f;
		myAnim.SetBool ("sleeping", false);
		LoadCollider (originalCollider);
		Destroy (instance);
		dm.Clear ();
		sleepCollider.enabled = false;
		ChangeMode (EnemyMode.CONFUSED);
	}

	public void Throw (bool direction, float force){
		Debug.Log ("THROW");
		transform.parent = null;
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		rb.velocity = new Vector2 ( direction ? force : -force, force);
		GetComponent<SpriteRenderer> ().sortingOrder = sortingOrder;
		isPickedUp = false;
	}

	IEnumerator Sleep(float length){
		myAnim.SetBool ("sleeping", true);
		dm.Clear ();
		instance = Instantiate (zzzz, new Vector3 (transform.position.x, transform.position.y), Quaternion.identity);
		instance.transform.parent = transform;
		LoadCollider (sleepCollider);
		yield return new WaitForSeconds (length);
		myAnim.SetBool ("sleeping", false);
		LoadCollider (originalCollider);
		Destroy (instance);
		sleepCollider.enabled = false;
		currentMode = EnemyMode.CONFUSED;
		yield return new WaitForSeconds (0f);
	}

	public void Wake(){
		StopCoroutine (tranqRoutine);
		myAnim.SetBool ("sleeping", false);
		LoadCollider (originalCollider);
		Destroy (instance);
		sleepCollider.enabled = false;
		currentMode = EnemyMode.CONFUSED;
	}

	public void ForgetTarget(){
		if (instance == null || instance.name != "QuestionMark") {
			dm.ShowAbandon ();
			Destroy (instance);
			instance = Instantiate (question, new Vector3 (transform.position.x, transform.position.y + 2.5f), Quaternion.identity);
			instance.transform.parent = transform;
			Destroy (instance, 5f);
		}
		forgotten = true;
		currentMode = EnemyMode.PATROLLING;
		target = new Vector3 (initialPosition.x - patrolDistance, transform.position.y, transform.position.z);
	}

	IEnumerator Pause(){
		paused = true;
		yield return new WaitForSeconds(2f);
		paused = false;
		yield return new WaitForSeconds(0.1f);
	}

	IEnumerator Defer(){
		paused = true;
		yield return new WaitForSeconds(3f);
		paused = false;
		yield return new WaitForSeconds(0.1f);
	}

	public void StartDefer(GameObject def){
		if (def.GetComponent<MinerController> () != null) {
			if (facingLeft && def.GetComponent<MinerController> ().facingLeft && transform.position.x > def.transform.position.x) {
				deferred = StartCoroutine ("Defer");
			}
			if (!facingLeft && !def.GetComponent<MinerController> ().facingLeft && transform.position.x < def.transform.position.x) {
				deferred = StartCoroutine ("Defer");
			}
		}
	}

	void MoveTowards(float spd = 0.2f){
		if (Time.timeScale > 0f) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (target.x, transform.position.y, transform.position.z), spd);
		}
		//		rb.MovePosition(target.position);
	}

	public void Patrol(){
		float distanceToOrigin = Vector3.Distance (new Vector3(initialPosition.x, transform.position.y, transform.position.z), transform.position);
		float distance = Vector3.Distance (transform.position, target);
		if (!paused &&distance < 1f) {
			StartCoroutine ("Pause");
		}
		if (distanceToOrigin < 1f) {
			target = new Vector3 (initialPosition.x - patrolDistance, initialPosition.y, initialPosition.z);

		} else if (distanceToOrigin > (patrolDistance - 1)) {
			target = new Vector3(initialPosition.x, transform.position.y, transform.position.z);
		}
		if (!paused) {
			FaceTarget ();
			MoveTowards ();
		}
	}

	public void NoticeTarget(){
		if (instance == null || instance.name != "Exclamation") {
			dm.ShowAlert ();
			forgotten = false;
			Destroy (instance);
			am.Play ("alert");
			instance = Instantiate (exclamation, new Vector3 (transform.position.x, transform.position.y + 2.5f), Quaternion.identity);
			instance.transform.parent = transform;
			Destroy (instance, 5f);
			currentMode = EnemyMode.PURSUING;
//			CancelFlipCycle ();
		}
	}

	public void SetTarget(Vector3 targ){
		target = targ;
	}

	public void FaceTarget(){
		if (target.x < transform.position.x) {
			FlipLeft ();
		}
		if (target.x >= transform.position.x) {
			FlipRight ();
		}
	}

	public void FaceTarget(Vector3 target){
		if (target.x < transform.position.x) {
			FlipLeft ();
		}
		if (target.x >= transform.position.x) {
			FlipRight ();
		}
	}

	public void Electrocute(){
		Damage (1);
	}

	public void Burn(){
		burning = true;
		Damage (1);
	}

	public int GetCurrentHitPoints(){
		return hitPoints;
	}

	public int GetMaxHitPoints(){
		return maxHitPoints;
	}

	public bool isBurning(){
		return burning;
	}

	public void Damage(int hit){
		am.Play("hit");
		hitPoints -= hit;
		if (currentMode == EnemyMode.TRANQUILIZED) {
			myAnim.SetBool ("sleeping", false);
			LoadCollider (originalCollider);
			currentMode = EnemyMode.ALERTED;
		}
		if (currentMode != EnemyMode.ALERTED) {
			Flip ();
		}
		if (unarmed && currentMode != EnemyMode.FLEEING) {
			currentMode = EnemyMode.FLEEING;
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			Vector3 point = player.transform.position;
			if (transform.position.x < point.x) {
				target = new Vector3 (transform.position.x - 15f, transform.position.y, transform.position.z);
			} else if (transform.position.x > point.x) {
				target = new Vector3 (transform.position.x + 15f, transform.position.y, transform.position.z);
			}
		}
		if (hitPoints <= 0) {
			if (!dying) {
				StartCoroutine ("FlashDie");
			}
		}
		StartCoroutine ("Flash");
	}

	public void Fire(){
		float angle = Vector3.Angle (target, transform.position);
		projManager.Fire (facingLeft, 1, facingLeft ? 180 : 0, po.GetPosition(), ProjectileManager.Mode.ENEMY);
	}

	public void Pursue(){
		if (Time.time - timer > FiringDelay && target != initialPosition && !unarmed) {
			timer = Time.time;
			Fire ();
		}
		if (transform.position.x < target.x) {
			FlipRight ();
		} else {
			FlipLeft ();
		}
		float distance = Vector3.Distance (target, transform.position);
		if (distanceToTarget <= 3f) {
			myAnim.SetBool ("walking", false);
		} else if (distanceToTarget > 3f){
			myAnim.SetBool ("walking", true);
		}
		if (Mathf.Abs (distanceToTarget) > 3f || target == new Vector3(initialPosition.x, transform.position.y, transform.position.z)) {
			if (!paused) {
				if (target == new Vector3 (initialPosition.x, transform.position.y, transform.position.z)) {
					MoveTowards (0.5f);
				} else {
					MoveTowards ();
				}
			}
		}
	}

	public void Drop(){
		Instantiate (droppable, new Vector3(transform.position.x - (facingLeft ? -2 : 2), transform.position.y, transform.localPosition.y), Quaternion.identity);
	}

	public void BeSurprised(){
		if (currentMode != EnemyMode.FLEEING) {
			currentMode = EnemyMode.SURPRISED;
			dm.ShowReveal ();
			Debug.Log ("Surprise!");
			if (flipCycle != null) {
//			CancelFlipCycle ();
			}
			if (!unarmed) {
				unarmed = true;
				myAnim.SetBool ("armed", false);
				Drop ();
			}
			if (instance == null || instance.name != "Exclamation") {
				Destroy (instance);
				am.Play ("alert");
				instance = Instantiate (exclamation, new Vector3 (transform.position.x, transform.position.y + 2.5f), Quaternion.identity);
				instance.transform.parent = transform;
				Destroy (instance, 5f);
			}
		}
	}

	public void Flee(){
		dm.ShowFreakOut ();
		if (transform.position.x < target.x) {
			FlipRight ();
		} else {
			FlipLeft ();
		}
		if (target.x < transform.position.x) {
			target = new Vector3 (target.x - 1f, target.y, target.z);
		} else if(target.x > transform.position.x){
			target = new Vector3 (target.x + 1f, target.y, target.z);
		}
		MoveTowards (0.5f);
	}

	public void Flee2(){
		if (transform.position.x < target.x) {
			FlipRight ();
		} else {
			FlipLeft ();
		}
		float distance = Vector3.Distance (target, transform.position);
		if (distance > 2f) {
			MoveTowards (0.5f);
		}
	}


	void Flip(){
		Vector3 scl = transform.localScale;
		transform.localScale = new Vector3 (-scl.x, scl.y, scl.z);
		facingLeft = !facingLeft;
	}

	void FlipLeft(){
		Vector3 scl = transform.localScale;
		transform.localScale = new Vector3 (-Mathf.Abs(scl.x), scl.y, scl.z);
		facingLeft = true;
	}

	void FlipRight(){
		Vector3 scl = transform.localScale;
		transform.localScale = new Vector3 (Mathf.Abs(scl.x), scl.y, scl.z);
		facingLeft = false;
	}

	void CancelFlipCycle(){
		Debug.Log ("CancelFlipCycle");
		StopCoroutine (flipCycle);
		flipCycle = null;
	}

	IEnumerator FlipCycle(){
		while (true) {
			Debug.Log ("Flip!");
			yield return new WaitForSeconds(8f);
			Flip ();
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
		burning = false;
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
//		Drop ();
		GameObject.Find ("ComboManager").SendMessage ("InitiateCombo", this.gameObject);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
//		Instantiate (explosion, transform.position, transform.rotation);
		Destroy (this.gameObject);
		yield return new WaitForSeconds(0.1f);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (currentMode != EnemyMode.TRANQUILIZED) {
			collided = coll.gameObject.name;
//			if (coll.gameObject.layer == 15) {
//				if (facingLeft && coll.gameObject.transform.position.x > transform.position.x) {
//					deferred = StartCoroutine ("Defer");
//				}
//				if (!facingLeft && coll.gameObject.transform.position.x < transform.position.x) {
//					deferred = StartCoroutine ("Defer");
//				}
//			}
//			if (coll.gameObject.layer == 13 && currentMode != EnemyMode.FLEEING) {
//				target = initialPosition;
//			} else if (coll.gameObject.layer == 13 && currentMode == EnemyMode.FLEEING) {
//				currentMode = EnemyMode.FUCKED;
//			}
			if (coll.gameObject.tag == "Crate" && currentMode == EnemyMode.FLEEING) {
				currentMode = EnemyMode.FUCKED;
			} else if (coll.gameObject.tag == "Crate" && currentMode != EnemyMode.FLEEING) {
				if (transform.position.x < coll.gameObject.transform.position.x) {
					StartCoroutine ("Pause");
					target = new Vector3 (transform.position.x - 15f, transform.position.y, transform.position.z);
				}
				if (transform.position.x > coll.gameObject.transform.position.x) {
					StartCoroutine ("Pause");
					target = new Vector3 (transform.position.x + 15f, transform.position.y, transform.position.z);
				}
			}
			if (coll.gameObject.layer == 13 && currentMode == EnemyMode.FLEEING) {
				currentMode = EnemyMode.FUCKED;
			} else if (coll.gameObject.layer == 13 && currentMode != EnemyMode.FLEEING) {
				if (transform.position.x < coll.gameObject.transform.position.x) {
					StartCoroutine ("Pause");
					target = new Vector3 (transform.position.x - 15f, transform.position.y, transform.position.z);
				}
				if (transform.position.x > coll.gameObject.transform.position.x) {
					StartCoroutine ("Pause");
					target = new Vector3 (transform.position.x + 15f, transform.position.y, transform.position.z);
				}
			}
			if (coll.gameObject.name == "Player" && currentMode != EnemyMode.FLEEING && currentMode != EnemyMode.FUCKED) {
				GameObject player = GameObject.FindGameObjectWithTag ("Player");
				target = player.transform.position;
				if (player.GetComponent<PlayerController> ().currentMode != PlayerMode.LOCKER && player.GetComponent<PlayerController> ().currentMode != PlayerMode.ELEVATOR) {
					alertManager.lastSeenPosition = player.transform.position;
					alertManager.HighAlert ();
					if (currentMode != EnemyMode.ALERTED && currentMode != EnemyMode.PURSUING) {
						currentMode = EnemyMode.ALERTED;
					}
					if (player.GetComponent<PlayerController> ().aiming && unarmed) {
						currentMode = EnemyMode.FLEEING;
						Vector3 point = player.transform.position;
						if (transform.position.x < point.x) {
							target = new Vector3 (transform.position.x - 15f, transform.position.y, transform.position.z);
						} else if (transform.position.x > point.x) {
							target = new Vector3 (transform.position.x + 15f, transform.position.y, transform.position.z);
						}
					}
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll){
//		if (coll.gameObject.layer == 15) {
//				if (deferred != null) {
//					StopCoroutine (deferred);
//					deferred = null;
//				}
//		}
	}

	void OnTriggerStay2D(Collider2D coll){
//		if (coll.gameObject.layer == 15) {
//			if (coll.gameObject.layer == 15) {
//				if (facingLeft && coll.gameObject.transform.position.x < transform.position.x) {
//					if (deferred != null) {
//						StopCoroutine (deferred);
//						deferred = null;
//					}
//					deferred = StartCoroutine ("Defer");
//				}
//				if (!facingLeft && coll.gameObject.transform.position.x > transform.position.x) {
//					if (deferred != null) {
//						StopCoroutine (deferred);
//						deferred = null;
//					}
//					deferred = StartCoroutine ("Defer");
//				}
//			}
//		}
	}

	void OnCollisionStay2D(Collision2D coll){
		if(currentMode != EnemyMode.TRANQUILIZED){
			collided = coll.gameObject.name;

			if (coll.gameObject.layer == 13 && currentMode != EnemyMode.FLEEING) {
				target = new Vector3(initialPosition.x, transform.position.y, transform.position.z);
			} else if (coll.gameObject.layer == 13 && currentMode == EnemyMode.FLEEING) {
				currentMode = EnemyMode.FUCKED;
			}
			if (coll.gameObject.tag == "Crate" && currentMode == EnemyMode.FLEEING) {
				currentMode = EnemyMode.FUCKED;
			} else if(coll.gameObject.tag == "Crate" && currentMode != EnemyMode.FLEEING){
				if (transform.position.x < coll.gameObject.transform.position.x) {
					target = new Vector3 (transform.position.x - 15f, transform.position.y, transform.position.z);
				}
				if (transform.position.x > coll.gameObject.transform.position.x) {
					target = new Vector3 (transform.position.x + 15f, transform.position.y, transform.position.z);
				}
			}
			if (coll.gameObject.name == "Player" && currentMode != EnemyMode.FLEEING && currentMode != EnemyMode.FUCKED) {
				GameObject player = GameObject.FindGameObjectWithTag ("Player");
				target = player.transform.position;
				if (player.GetComponent<PlayerController> ().currentMode != PlayerMode.LOCKER && player.GetComponent<PlayerController> ().currentMode != PlayerMode.ELEVATOR) {
					alertManager.lastSeenPosition = player.transform.position;
					alertManager.HighAlert ();
					if (currentMode != EnemyMode.ALERTED && currentMode != EnemyMode.PURSUING) {
						currentMode = EnemyMode.ALERTED;
					}
					if (player.GetComponent<PlayerController> ().aiming && unarmed) {
						currentMode = EnemyMode.FLEEING;
						Vector3 point = player.transform.position;
						if (transform.position.x < point.x) {
							target = new Vector3 (transform.position.x - 15f, transform.position.y, transform.position.z);
						} else if (transform.position.x > point.x) {
							target = new Vector3 (transform.position.x + 15f, transform.position.y, transform.position.z);
						}
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		realMode = currentMode;
		lastSeenPosition = alertManager.lastSeenPosition;
		myAnim.SetFloat ("velocity", Mathf.Abs(rb.velocity.y));
		distanceToTarget = Mathf.Abs (transform.position.x - target.x);
		if (lastPosition.x != transform.position.x) {
			myAnim.SetBool ("walking", true);
		} else {
			myAnim.SetBool ("walking", false);
		}
		lastPosition = transform.position;
		if(isPickedUp && currentMode != EnemyMode.TRANQUILIZED){
			transform.parent.gameObject.SendMessage ("Throw");
		}
		if (currentMode != EnemyMode.SURPRISED) {
			myAnim.SetBool ("surprised", false);
		}
		if (currentMode != EnemyMode.FUCKED) {
			myAnim.SetBool ("fucked", false);
		}
		switch (currentMode) {
		case EnemyMode.ALERTED:
			target = new Vector3 (lastSeenPosition.x, transform.position.y, transform.position.z);
			NoticeTarget ();
			break;
		case EnemyMode.FLEEING:
			Flee ();
			break;
		case EnemyMode.TRANQUILIZED:
			break;
		case EnemyMode.CONFUSED:
			dm.ShowConfused ();
			currentMode = EnemyMode.PATROLLING;
			break;
		case EnemyMode.FUCKED:
//			Flee2();
			myAnim.SetBool ("fucked", true);
			if (alertManager.currentMode == AlertMode.HIGH_ALERT) {
				alertManager.Alerted ();
			}
			target = transform.position;
			break;
		case EnemyMode.PURSUING:
			target = new Vector3 (lastSeenPosition.x, transform.position.y, transform.position.z);
			Pursue ();
			break;
		case EnemyMode.PATROLLING:
			if (flipCycle == null) {
//				flipCycle = StartCoroutine ("FlipCycle");
//				CancelFlipCycle ();
			}
			Patrol ();
			break;
		case EnemyMode.SURPRISED:
			myAnim.SetBool ("surprised", true);
			break;
		case EnemyMode.STASHED:
			break;
		}

		switch (alertManager.currentMode) {
		case AlertMode.ALERTED:
			if (currentMode != EnemyMode.FLEEING && currentMode != EnemyMode.SURPRISED && currentMode != EnemyMode.FLEEING && currentMode != EnemyMode.FUCKED && currentMode != EnemyMode.TRANQUILIZED) {
				currentMode = EnemyMode.PURSUING;
				target = alertManager.lastSeenPosition;
				forgotten = false;
			}
			break;
		case AlertMode.PATROLLING:

			if (currentMode != EnemyMode.FLEEING && currentMode != EnemyMode.SURPRISED && currentMode != EnemyMode.FLEEING && currentMode != EnemyMode.FUCKED && currentMode != EnemyMode.TRANQUILIZED) {
				if (!forgotten) {
					ForgetTarget ();
				}
			}
			if (currentMode == EnemyMode.FUCKED) {
				ForgetTarget ();
			}
			break;
		}
	}
}
