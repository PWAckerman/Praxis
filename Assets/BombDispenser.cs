using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDispenser : MonoBehaviour {

	public GameObject projectileOriginator;
	public int maxHitPoints;
	public int hitPoints;
	public GameObject explosion;
	public int angle;
	AudioManager am;
	ProjectileManager projManager;
	ProjectileOriginator po;

	// Use this for initialization
	void Start () {
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		hitPoints = maxHitPoints;
		po = projectileOriginator.GetComponent<ProjectileOriginator> ();
		projManager = new ProjectileManager ();
	}

	// Update is called once per frame
	void Update () {
		if (Time.fixedTime % 4 == 0) {
			Fire ();
		}
		if (hitPoints <= 0) {
			Die ();
		}
	}

	void Damage(int hit){
		hitPoints -= hit;
		StartCoroutine ("Flash");
	}

	void Die(){
		StartCoroutine ("FlashDie");
	}

	void Drop(){
		
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
		am.Play("destroyed");
		Drop ();
		GameObject.Find ("ComboManager").SendMessage ("InitiateCombo", this.gameObject);
		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (0f, 237f, 188f);
		yield return new WaitForSeconds(0.1f);
		Instantiate (explosion, transform.position, transform.rotation);
		Destroy (this.gameObject);
		yield return new WaitForSeconds(0.1f);
	}

	public void Fire(){
		projManager.Fire (true, 1, angle, po.GetPosition(), ProjectileManager.Mode.GRENADE);
	}
}
