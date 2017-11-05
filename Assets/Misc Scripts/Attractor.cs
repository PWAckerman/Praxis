using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {

	float pullSpeedFactor = 1.00f; // Higher values make objects home in faster
	private PlayerController player;
	public Rigidbody2D rb;
	public Transform transform;
	public int radius = 10;
	public Collider2D colliders;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		rb = GetComponent<Rigidbody2D> ();
		transform = gameObject.GetComponent<Transform> ();
	}

	// Update is called once per frame
	void Update () {
		colliders = Physics2D.OverlapCircle (transform.position, radius);
		if (Input.GetKey (KeyCode.Joystick1Button14) && colliders.gameObject.layer == 9) {
			Attract ();
		}
	}

	public void Attract(){
		Vector3 diff = player.transform.position - this.transform.position;
		if (!player.dead) {
			rb.constraints = RigidbodyConstraints2D.None;
			rb.mass = 0.1f;
			this.rb.AddForce (diff * pullSpeedFactor, ForceMode2D.Force);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){

	}
}
