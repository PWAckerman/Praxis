using UnityEngine;
 
using System.Collections;
using System.Timers;
using System.Linq;
using System;

public class Gold : MonoBehaviour {

	public GameObject go = new GameObject();
	float pullSpeedFactor = 0.1f; // Higher values make objects home in faster
	private PlayerController player;
	public Rigidbody2D rb;
	public GameObject Collected;
	public int radius = 10;
	public Collider2D colliders;
	public SpriteRenderer renderer;
	public CircleCollider2D collider;
	public CircleCollider2D triggerCollider;
	public Transform transform;
	public Sprite sprite;
	public MonoBehaviour par;
	public int count = 0;
	public bool moving = false;

	public Gold(MonoBehaviour parent){

	}

	public void Init(MonoBehaviour parent){
		sprite = Resources.Load<Sprite>("Coin");
		par = parent;
	}


	// Use this for initialization
	void Start () {
		int[] rnd = { 1, 2, 3, 4, 5 };
		int rndSelect = rnd.OrderBy (e => Guid.NewGuid ()).FirstOrDefault ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		renderer = gameObject.AddComponent<SpriteRenderer> ();
		rb = gameObject.AddComponent<Rigidbody2D> () as Rigidbody2D;
//		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		collider = gameObject.AddComponent<CircleCollider2D> () as CircleCollider2D;
		collider.radius = sprite.bounds.size.x / 2;
		Collected = Resources.Load<GameObject>("Collected");
		transform = gameObject.GetComponent<Transform> ();
		transform.position = new Vector3 (par.GetComponent<Transform>().position.x + (rndSelect * 0.2f), par.GetComponent<Transform>().position.y + 1, 1f);
		transform.localScale = new Vector3 (8,8,1);
		renderer.sprite = sprite;
		renderer.sortingOrder = 6;
		enabled = true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		colliders = Physics2D.OverlapCircle (transform.position, radius);
		if (Input.GetKey (KeyCode.Joystick1Button14) && colliders.gameObject.layer == 9) {
			Collect ();
		}
	}

	public void Collect(){
		Vector3 diff = player.transform.position - this.transform.position;
		this.rb.mass = 0.1f;
		if (!player.dead) {
			this.rb.AddForce (diff, ForceMode2D.Force);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.gameObject.layer == 9) {
			Debug.Log ("it was a player");
			Destroy(Instantiate<GameObject>(Collected, transform.position, Quaternion.identity), 0.25f);
			Destroy (this.gameObject);
			player.CollectGold();
		}

	}
}
