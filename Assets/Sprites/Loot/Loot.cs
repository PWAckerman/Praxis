﻿using UnityEngine;
 
using System.Collections;
using System.Timers;
using System.Linq;
using System;

public class Loot : MonoBehaviour {

	public GameObject go = new GameObject();
	float pullSpeedFactor = 0.1f; // Higher values make objects home in faster
	private PlayerController player;
	public Rigidbody2D rb;
	AudioManager am;
	public int radius = 10;
	public GameObject Collected;
	public Collider2D colliders;
	public LootTypes kind;
	public SpriteRenderer renderer;
	public CircleCollider2D collider;
	public Transform transform;
	public Sprite sprite;
	public MonoBehaviour par;
	public int count = 0;
	public bool moving = false;

	public Loot(LootTypes knd, MonoBehaviour parent){
		
	}

	public void Init(LootTypes knd, MonoBehaviour parent){
		kind = knd;
		Debug.Log ("Resource" + knd.ToString () + ".png");
		sprite = Resources.Load<Sprite>("Resource" + knd.ToString());
		par = parent;
		transform = gameObject.GetComponent<Transform> ();
	}
		

	// Use this for initialization
	void Start () {
		int[] rnd = { 1, 2, 3, 4, 5 };
		int rndSelect = rnd.OrderBy (e => Guid.NewGuid ()).FirstOrDefault ();
		am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		renderer = gameObject.AddComponent<SpriteRenderer> ();
		Collected = Resources.Load<GameObject>("Collected");
		rb = gameObject.AddComponent<Rigidbody2D> () as Rigidbody2D;
		collider = gameObject.AddComponent<CircleCollider2D> () as CircleCollider2D;
		collider.radius = sprite.bounds.size.x / 2;
		transform.position = new Vector3 (par.GetComponent<Transform>().position.x + (rndSelect * 0.2f), par.GetComponent<Transform>().position.y + 1, 1f);
		transform.localScale = new Vector3 (8,8,1);
		renderer.sprite = sprite;
		renderer.sortingOrder = 4;
		enabled = true;
		gameObject.layer = 17;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		colliders = Physics2D.OverlapCircle (transform.position, radius);
		if ((Input.GetKey (KeyCode.Joystick1Button13) || Input.GetKey(KeyCode.F))) {
			Collect ();
		}
	}

	public void Collect(){
		Vector3 diff = player.transform.position - this.transform.position;
		this.rb.mass = 0.1f;
		if (!player.dead && !player.aiming) {
			this.rb.AddForce (diff, ForceMode2D.Force);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.gameObject.layer == 9) {
			Debug.Log ("it was a player");
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			Destroy(Instantiate<GameObject>(Collected, transform.position, Quaternion.identity), 0.25f);
			Destroy (this.gameObject);
			player.CollectResource(this.kind);
		}
		if (coll.collider.gameObject.layer == 10) {
			am.Play ("drop");
		}
	}
}
