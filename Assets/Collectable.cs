using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	// Use this for initialization

	float pullSpeedFactor = 0.1f; // Higher values make objects home in faster
	private PlayerController player;
	public Rigidbody2D rb;
	AudioManager am;
	public int radius = 10;
	public Collider2D colliders;
	public GameObject Collected;
	public LootTypes kind;
	public SpriteRenderer renderer;
	public Collider2D collider;
	public Transform transform;
	public LootTypes knd;
	public int count = 0;
	public bool moving = false;

	void Start () {
		transform = gameObject.GetComponent<Transform> ();
		am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		rb = gameObject.GetComponent<Rigidbody2D> () as Rigidbody2D;
		collider = gameObject.GetComponent<Collider2D> () as Collider2D;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		colliders = Physics2D.OverlapCircle (transform.position, radius);
		if ((Input.GetKey (KeyCode.Joystick1Button14) || Input.GetKey(KeyCode.F)) && colliders.gameObject.layer == 9) {
			Collect ();
		}
	}

	public void Collect(){
		Vector3 diff = player.transform.position - transform.position;
		rb.mass = 0.1f;
		if (!player.dead && !player.aiming) {
			rb.AddForce (diff, ForceMode2D.Force);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.gameObject.layer == 9) {
			Debug.Log ("it was a player");
			Destroy(Instantiate<GameObject>(Collected, transform.position, Quaternion.identity), 0.25f);
			Destroy (gameObject);
			player.CollectResource(knd);
		}
		if (coll.collider.gameObject.layer == 10) {
			am.Play ("drop");
		}
	}
}
