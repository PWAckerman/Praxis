using UnityEngine;
using UnityEditor;
using System.Collections;

public class Bomb : MonoBehaviour {

	float pullSpeedFactor = 0.01f; // Higher values make objects home in faster
	private PlayerController player;
	public Rigidbody2D rb;
	public SpriteRenderer renderer;
	public CircleCollider2D collider;
	public CircleCollider2D triggerCollider;
	public Transform transform;
	public Sprite sprite;
	public int radius = 10;
	public Collider2D colliders;
	// Use this for initialization
	void Start () {
		sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Obstacles/Bomb.png");
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		renderer = gameObject.AddComponent<SpriteRenderer> ();
		rb = gameObject.AddComponent<Rigidbody2D> () as Rigidbody2D;
		collider = gameObject.AddComponent<CircleCollider2D> () as CircleCollider2D;
		triggerCollider = gameObject.AddComponent<CircleCollider2D> () as CircleCollider2D;
		rb.mass = 0f;
		rb.constraints = RigidbodyConstraints2D.FreezePositionY;
		collider.radius = sprite.bounds.size.x / 2;
		triggerCollider.radius = collider.radius;
		triggerCollider.isTrigger = true;
		transform = gameObject.GetComponent<Transform> ();
		renderer.sprite = sprite;
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
			this.rb.mass = 0.1f;
			rb.constraints = RigidbodyConstraints2D.None;
			this.rb.AddForce (diff, ForceMode2D.Force);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.gameObject.layer == 9) {
			Debug.Log ("it was a player");
			player.Damage ();
			Destroy (this.gameObject);
		}
		if (coll.collider.gameObject.layer == 8) {
			Debug.Log ("it was a projectile");
			Destroy (coll.collider.gameObject);
			Destroy (this.gameObject);
		}

	}
}
