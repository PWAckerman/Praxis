using UnityEngine;
 
using System.Collections;
using System.Timers;

public class Safe : MonoBehaviour, ILootable {
	public Gold[] gold;
	public int dropRate;
	public Sprite open;
	public AudioSource snd;
	public GameObject dust;
	public bool dropped = false;
	void Start () {
		open = Resources.Load<Sprite>("SafeOpen");
		snd = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll){
		Vector2 normalizedVelocity = coll.otherRigidbody.velocity.normalized;
		if (coll.collider.gameObject.layer == 10 && Mathf.Abs(normalizedVelocity.y) > 0.5 && !dropped) {
			Debug.Log ("it was a safe");
			Open ();
		} else if(coll.collider.gameObject.layer == 9 && coll.relativeVelocity.y < -2 && !dropped){
			PlayerController player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
			player.Damage ();
			Open ();
		}
	}

	void Open(){
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = open;
		Drop ();
		snd.Play ();
		dropped = true;
		Instantiate (dust, transform.position, Quaternion.identity);
	}

	void Damage(int amount){
		Open ();
	}

	public void Drop(){
		if (!dropped) {
			gold = GoldFactory.GetAssortedGold (dropRate, this, new GameObject ());
		}
	}

}