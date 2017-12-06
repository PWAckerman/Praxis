using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranqProjectile : MonoBehaviour, IProjectile {

	public Rigidbody2D rb;
	ConstantForce2D force;
	GameObject parent;
	AudioManager am;
	SpriteRenderer rd;
	public bool collided;
	public int angleIncrement;
	public float strength;
	int aim;
	float projectileSpeed = 50;
	public float timeToLive;
	public Sprite projectile;

	public void Start(){
		angleIncrement = 45;
		force = GetComponent<ConstantForce2D> ();
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		timeToLive = 3f;
		this.gameObject.layer = 8;
		rb = GetComponent<Rigidbody2D> () as Rigidbody2D;
		am.Play ("shoot1");
	}

	public void Update(){
//		if (gameObject.tag == "PlayerProjectile") {
//			Physics2D.IgnoreLayerCollision (8, 9, true);
//		}
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
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		rb.AddForce (f * projectileSpeed, ForceMode2D.Impulse);
//		force.relativeForce = f * projectileSpeed;
		Destroy(this.gameObject, timeToLive);
	}

	public void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.layer == 15 && gameObject.tag == "PlayerProjectile") {
			if (collision.gameObject.GetComponent<ITranquilizeable> () != null ) {
				if (!collision.gameObject.GetComponent<ITranquilizeable> ().IsTranquilized ()) {
					rb.isKinematic = true;
					rb.velocity = new Vector2 (0, 0);
					collision.gameObject.GetComponent<ITranquilizeable> ().Tranquilize (strength);
					transform.parent = collision.gameObject.transform;
				}
			}
			Destroy (this.gameObject, 0.5f);
		} else {
			rb.isKinematic = true;
			rb.velocity = new Vector2 (0, 0);
			Destroy (this.gameObject);
		}
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
