using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collapse : MonoBehaviour {

	// Use this for initialization

	public HashSet<GameObject> items;
	public GameObject dust;
	public GameObject explosion;
	public float maxMassSupported;
	public bool collapsed;
	[SerializeField]
	float totalMass;

	void Start () {
		items = new HashSet<GameObject> ();
		totalMass = 0;
		collapsed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (totalMass > maxMassSupported && !collapsed) {
			StartCollapsing ();
		}
		if (collapsed) {
			SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer> ();
			sprites.ToList ().ForEach ((sprite)=>{
				sprite.color = Color.Lerp(sprite.color, new Color(255f,255f,255f,0f), 0.1f);
			});
		}
	}

	void StartCollapsing(){
		Rigidbody2D[] bodies = GetComponentsInChildren<Rigidbody2D> ();
		bodies.ToList ().ForEach ((bod) => {
			bod.bodyType = RigidbodyType2D.Dynamic;
		});
		collapsed = true;	
		Destroy(Instantiate (dust, transform.position, Quaternion.identity), 2f);
		Destroy(Instantiate (explosion, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity), 0.1f);
		Destroy (this.gameObject, 3f);
	}

	void CalculateTotalMass(){
		float mass = 0;
		totalMass = items.ToList ().Aggregate (mass, (curr, next) => {
			return curr + (next.GetComponent<Rigidbody2D>() != null ? next.GetComponent<Rigidbody2D>().mass : 0);
		});
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.layer != 15) {
			items.Add (coll.gameObject);
			CalculateTotalMass ();
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		items.Remove (coll.gameObject);
		CalculateTotalMass ();
	}
}
