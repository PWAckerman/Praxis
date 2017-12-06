using UnityEngine;
 
using System.Collections;
using System.Timers;
using System.Linq;

public class Grenade: MonoBehaviour, IProjectile, IExplodeable, IBurnable{

	public Rigidbody2D rb;
	ConstantForce2D force;
	GameObject parent;
	AudioManager am;
	Animator myAnim;
	SpriteRenderer rd;
	bool burning;
	public bool collided;
	public GameObject explosion;
	public GameObject dust;
	public int angleIncrement;
	int aim;
	float projectileSpeed = 20;
	public float timeToLive;
	public Sprite projectile;

	// Use this for initialization

	public void Start(){
		Debug.Log ("GRENADE INITIATIED");
		myAnim = GetComponent<Animator> ();
		angleIncrement = 45;
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		timeToLive = 20f;
		this.gameObject.layer = 8;
		rb = GetComponent<Rigidbody2D> () as Rigidbody2D;
		am.Play ("shoot1");
		burning = false;
	}

	public void Update(){
		//		bool wall = Physics2D.OverlapCircle (transform.position, 0.5f, 10);
		//		bool floor = Physics2D.OverlapCircle (transform.position, 0.5f, 13);
		//		bool platform = Physics2D.OverlapCircle (transform.position, 0.5f, 12);
		//		Debug.Log ("PROJECTILE COLLISION?");
		//		Debug.Log (wall);
		//		Debug.Log (floor);
		//		Debug.Log (platform);
		//		if (wall || floor || platform) {
		//			Destroy(this.gameObject);
		//		}
	}

	public void Burn(){
		burning = true;
		Explode ();
	}

	public bool isBurning(){
		return burning;
	}

	public void Explode(){
		if (this.gameObject != null) {
			Destroy (this.gameObject);
		}
	}

	public void Fire(bool direction, int aim, int angle){
		//		transform.position = new Vector3(parent.transform.position.x + (direction ? -2 : 2), parent.transform.position.y + 1,  parent.transform.position.z);
		rd = GetComponent<SpriteRenderer>();
		rd.sprite = projectile;
		Debug.Log ("GRENADE ANGLE");
		Debug.Log (angle);
		Vector3 f = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;
		rb.AddForce (new Vector2 (0, 40), ForceMode2D.Impulse);
		rb.AddForce (f * projectileSpeed, ForceMode2D.Impulse);
//		myAnim = GetComponent<Animator> ();
		StartCountdown();
		Destroy(this.gameObject, timeToLive);
	}

	public void OnTriggerEnter2D(Collider2D coll){
//		Debug.Log ("COLLISION");
//		if (collision.gameObject.tag == "Drone" && gameObject.tag == "PlayerProjectile") {
//			collision.gameObject.GetComponent<DroneController> ().Damage (1);
//		}
		if(coll.gameObject.tag == "Lava"){
			Destroy (this.gameObject);
		}
		if(coll.gameObject.tag == "GrenadeExplosion"){
			Destroy (this.gameObject);
		}
//		StartCountdown ();
//		myAnim.SetBool ("exploding", true);
	}

	public void OnDestroy(){
		am.Play ("destroyed");
		Instantiate (explosion, transform.position, Quaternion.identity);
		Instantiate (dust, transform.position, Quaternion.identity);
		Debug.Log ("DESTROYED");
	}

	public void StartCountdown(){
		Debug.Log ("COUNTDOWN");
		StartCoroutine ("Countdown");
	}

	public IEnumerator Countdown(){
		Debug.Log ("COUNTDOWN2");
		yield return new WaitForSeconds(timeToLive * 0.5f);
		StartCoroutine("Flashing");
		yield return new WaitForSeconds (timeToLive * 0.2f);
		StartCoroutine ("FlashingFast");
		yield return new WaitForSeconds (timeToLive * 0.1f);
	}

	public IEnumerator Flashing() {
		while (true) {
			Debug.Log ("FLASHING:");
			am.Play ("countdown");
			GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 0f, 100f);
			yield return new WaitForSeconds (0.5f);
			GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
			yield return new WaitForSeconds (0.5f);
		}
	}

	public IEnumerator FlashingFast() {
		while (true) {
			Debug.Log ("FAST FLASHING:");
			am.Play ("countdown");
			GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 0f, 100f);
			yield return new WaitForSeconds (0.1f);
			GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
			yield return new WaitForSeconds (0.1f);
		}
	}

	public void Die(){
		Destroy (this.gameObject);
	}

}
