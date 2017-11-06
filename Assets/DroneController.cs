using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DroneController : MonoBehaviour {

	// Use this for initialization
	Transform transform;
	Animator myAnim;
	Rigidbody2D rb;
	BoxCollider2D cd;
	Transform target;
	public float perceptionRange;
	int hitPoints;
	public int maxHitPoints;

	void Start () {
		transform = GetComponent<Transform> ();
		myAnim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		cd = GetComponent<BoxCollider2D> ();
		hitPoints = maxHitPoints;
	}

	// Update is called once per frame
	void Update () {
		bool nearPlayer = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 5f, 9);
		Debug.Log ("Overlap?");
		Debug.Log (nearPlayer);
		if (nearPlayer) {
			myAnim.SetBool ("alerted", true);
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			float distance = Vector3.Distance (target.position, transform.position);
			if (Mathf.Abs (distance) > 5f) {
				MoveTowards ();
			}
		} else {
			myAnim.SetBool ("alerted", false);
		}
	}

	void MoveTowards(){
		transform.position = Vector3.MoveTowards(transform.position, target.position, 0.2f);
//		rb.MovePosition(target.position);
	}

	void FixedUpdate(){

		//Altitude
//		rb.AddForce(0, Input.GetAxis("Vertical") * 5, 0);
	}

	public void Damage(int hit){
		hitPoints -= hit;
		if (hitPoints <= 0) {
			StartCoroutine ("FlashDie");
		}
		StartCoroutine ("Flash");
	}

	public void Die(){
		Destroy (this.gameObject);
	}
		
	public IEnumerator Flash() {
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 0f, 255f);
		yield return new WaitForSeconds(0.1f);
	}

	public IEnumerator FlashDie(){
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (33f, 255f, 255f);
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		Destroy (this.gameObject);
		yield return new WaitForSeconds(0.1f);
	}
}
