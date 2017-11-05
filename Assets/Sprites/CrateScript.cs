using UnityEngine;
using System.Collections;
using System.Timers;

public class CrateScript : MonoBehaviour, ILootable {
	public Loot[] loot;
	public int dropRate;
	AudioSource snd;
	// Use this for initialization
	void Start () {
		snd = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		
		if (coll.collider.gameObject.layer == 8) {
			snd.Play ();
			Debug.Log ("it was a projectile");
			Destroy(coll.collider.gameObject);
			Destroy (this.gameObject);
			Drop ();
		}
	}

	public void Drop(){
		loot = LootFactory.GetAssortedRandomLoot (Random.Range(dropRate - 5, dropRate), this, new GameObject());
	}

}
