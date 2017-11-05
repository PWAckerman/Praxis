using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Timers;
using System.Linq;

public class Projectil1 : MonoBehaviour, IProjectile{

	public Rigidbody2D rb;
	CircleCollider2D collider;
	ConstantForce2D force;
	SpriteRenderer renderer;
	GameObject parent;
	Transform transform;
	public bool collided;
	int aim;
	float projectileSpeed = 40;
	public float timeToLive;
	public Sprite[] sprites;
	public Sprite projectile;

	// Use this for initialization

	public void Start(){
		timeToLive = 3f;
		sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/Projectiles/Projectile1.png").OfType<Sprite>().ToArray();
		this.gameObject.layer = 8;
		renderer = GetComponent<SpriteRenderer> () as SpriteRenderer;
		rb = GetComponent<Rigidbody2D> () as Rigidbody2D;
		collider = GetComponent<CircleCollider2D> () as CircleCollider2D;
		transform = GetComponent<Transform> ();
		force = GetComponent<ConstantForce2D> ();
		collider.radius = projectile.bounds.size.x / 2;
		parent = GameObject.FindGameObjectWithTag ("Player");
		bool direction = parent.GetComponent<SpriteRenderer> ().flipX;
		transform.parent = parent.transform;
		transform.position = new Vector3(parent.transform.position.x + (direction ? -2 : 2), parent.transform.position.y + 1,  parent.transform.position.z);
		renderer.sprite = projectile;
		int level = 1;
		aim= parent.GetComponent<PlayerController>().aimingDirection;
		Vector3 f = Quaternion.AngleAxis(0  + (20 * (direction ? -aim : aim)), Vector3.forward) * (direction ? Vector2.left : Vector2.right);
		rb.AddForce (f * projectileSpeed, ForceMode2D.Impulse);
		force.relativeForce = f * projectileSpeed;
		UnityEngine.Object.Destroy(this.gameObject, timeToLive);
	}

	public void Update(){
		bool wall = Physics2D.OverlapCircle (transform.position, 2f, 10);
		bool floor = Physics2D.OverlapCircle (transform.position, 2f, 13);
		bool platform = Physics2D.OverlapCircle (transform.position, 2f, 12);
		Debug.Log ("PROJECTILE COLLISION?");
		Debug.Log (wall);
		Debug.Log (floor);
		Debug.Log (platform);
		if (wall || floor || platform) {
			Destroy(this.gameObject);
		}
	}

	public void Fire(bool direction, int level, int angle){
		Debug.Log (angle);
		Vector3 f = Quaternion.AngleAxis(0  + (20 * (direction ? angle : -angle )), Vector3.forward) * (direction ? Vector2.right : Vector2.left);
		if (direction) {
			transform.position = new Vector3 (parent.transform.position.x + 2, parent.transform.position.y + 1, parent.transform.position.z);
		} else {
			transform.position = new Vector3(parent.transform.position.x - 2, parent.transform.position.y + 1,  parent.transform.position.z);
		}
		rb.AddForce (f * projectileSpeed, ForceMode2D.Impulse);
		force.relativeForce = f * projectileSpeed;
		UnityEngine.Object.Destroy(this, timeToLive);
	}

	public void OnCollisionEnter2D(Collision2D collision){
		Debug.Log ("COLLISION");
		Destroy (this.gameObject);
	}

	public void OnCollisionEnter(){
		
	}
}
