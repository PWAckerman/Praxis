using UnityEngine;
 
using System.Collections;
using System.Timers;
using System.Linq;

public class Projectil1 : MonoBehaviour, IProjectile{

	public Rigidbody2D rb;
	ConstantForce2D force;
	GameObject parent;
	AudioManager am;
	Animator myAnim;
	SpriteRenderer rd;
	public bool collided;
	public int angleIncrement;
	int aim;
	float projectileSpeed = 40;
	public float timeToLive;
	public Sprite projectile;

	// Use this for initialization

	public void Start(){
		myAnim = GetComponent<Animator> ();
		angleIncrement = 45;
		force = GetComponent<ConstantForce2D> ();
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		timeToLive = 3f;
		this.gameObject.layer = 8;
		rb = GetComponent<Rigidbody2D> () as Rigidbody2D;
		am.Play ("shoot1");
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

	public void Fire(bool direction, int aim, int angle){
		//		transform.position = new Vector3(parent.transform.position.x + (direction ? -2 : 2), parent.transform.position.y + 1,  parent.transform.position.z);
		rd = GetComponent<SpriteRenderer>();
		rd.sprite = projectile;
		Debug.Log ("AIM");
		Debug.Log (aim);
		Debug.Log ((angleIncrement * (direction ? aim : -aim)));
		Vector3 f = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;
		rb.AddForce (f * projectileSpeed, ForceMode2D.Impulse);
		force.relativeForce = f * projectileSpeed;
		myAnim = GetComponent<Animator> ();
		Destroy(this.gameObject, timeToLive);
	}

	public void OnCollisionEnter2D(Collision2D collision){
		Debug.Log ("COLLISION");
		if (collision.gameObject.tag == "Drone" && gameObject.tag == "PlayerProjectile") {
			collision.gameObject.GetComponent<IDamageable> ().Damage (1);
			Destroy (this.gameObject);
		} else if (collision.gameObject.name == "ReconDrone") {
//			collision.gameObject.SendMessage ("Damage", 1);
			collision.gameObject.GetComponent<IDamageable> ().Damage (1);
			Destroy (this.gameObject);
		} else if (collision.gameObject.layer == 15 && gameObject.tag == "PlayerProjectile") {
			collision.gameObject.GetComponent<IDamageable> ().Damage (1);
			Destroy (this.gameObject);
		} else if (collision.gameObject.tag == "ElectricPole" && gameObject.tag == "PlayerProjectile") {
			collision.gameObject.GetComponent<IDamageable> ().Damage (1);
			Destroy (this.gameObject);
		}
		if (collision.gameObject.tag == "Grenade") {
			collision.gameObject.GetComponent<Grenade>().Die ();
			Destroy (this.gameObject);
		}
		Debug.Log (collision.gameObject.name);
		myAnim.SetBool ("exploding", true);
		rb.isKinematic = true;
		rb.velocity = new Vector2 (0, 0);
//		Destroy (this.gameObject);
	}

	public void Die(){
		gameObject.AddComponent<PointEffector2D> ();
		PointEffector2D pe = gameObject.GetComponent<PointEffector2D> ();
		GetComponent<CircleCollider2D> ().usedByEffector = true;
		pe.forceMagnitude = 100f;
		pe.forceVariation = 50f;
		pe.forceSource = EffectorSelection2D.Collider;
		pe.distanceScale = 1;
		Destroy (this.gameObject, 0.1f);
	}
		
}
