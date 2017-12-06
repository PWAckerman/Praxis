using UnityEngine;
using System.Collections;
using System.Timers;

public class CrateScript : MonoBehaviour, ILootable, IBurnable, IAttachable {
	public Loot[] loot;
	public int dropRate;
	public GameObject dust;
	public GameObject bigFire;
	bool burning = false;
	AudioManager am;
	// Use this for initialization
	void Start () {
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.gameObject.layer == 8) {
			Debug.Log ("it was a projectile");
			Destroy(coll.collider.gameObject);
			am.Play ("woodBreak");
			Drop ();
			Destroy (this.gameObject);
		}
	}

	public void Burn(){
		burning = true;
		GameObject fire = Instantiate (bigFire, transform.position, Quaternion.identity);
		fire.transform.SetParent (this.transform);
		Destroy (this.gameObject, 3f);
	}

	public bool isBurning(){
		return burning;
	}

	void OnDestroy(){
		Instantiate (dust, transform.position, Quaternion.identity);
	}

	public void Drop(){
		loot = LootFactory.GetAssortedRandomLoot (Random.Range(dropRate - 5, dropRate), this, new GameObject());
	}

	public void Attach(GameObject go, Vector2 point){
		if (GetComponent<HingeJoint2D> () == null) {
			this.gameObject.AddComponent<HingeJoint2D> ();
			GetComponent<HingeJoint2D> ().connectedBody = go.GetComponent<Rigidbody2D> ();
			GetComponent<HingeJoint2D> ().connectedAnchor = transform.InverseTransformPoint(point);
		}
	}
}
