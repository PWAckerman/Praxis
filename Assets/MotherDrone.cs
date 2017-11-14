using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotherDroneMode{
	PATROLLING,
	RELEASING,
	SWOOPING
}

public class MotherDrone : MonoBehaviour {

	// Use this for initialization
	public float distance;
	public float speed;
	public float swoopDistance;
	public Animator anim;
	public MotherDroneMode currentMode;
	public int maxHitPoints;
	public int hitPoints;
	public GameObject explosion;
	public GameObject drone;
	Animator myAnim;
	AudioManager am;
	Vector3 target;
	Vector3 initialPosition;
	int cycleCount;
	bool releasing;
	List<GameObject> children;


	void Start () {
		children = new List<GameObject> ();
		am = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
		myAnim = GetComponent<Animator> ();
		hitPoints = maxHitPoints;
		currentMode = MotherDroneMode.PATROLLING;
		initialPosition = transform.position;
		target = new Vector3 (initialPosition.x + distance, initialPosition.y, initialPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		children.RemoveAll(item => item == null);
		switch (currentMode) {
		case MotherDroneMode.PATROLLING:
			Patrol ();
			break;
		case MotherDroneMode.RELEASING:
			Release ();
			break;
		case MotherDroneMode.SWOOPING:
			Swoop ();
			break;
		}

		if (hitPoints <= 0) {
			Die ();
		}
	}

	void Patrol (){
		RotateUpsideUp ();
		if(transform.position == target && target != initialPosition){
			target = initialPosition;
		} else if(transform.position == target && target == initialPosition){
			target = new Vector3 (initialPosition.x + distance, initialPosition.y, initialPosition.z);
			cycleCount++;
		}
		if (cycleCount % 5 == 0 && cycleCount > 1) {
			currentMode = MotherDroneMode.RELEASING;
			cycleCount++;
		}
		if (cycleCount % 3 == 0  && cycleCount > 1) {
			currentMode = MotherDroneMode.SWOOPING;
			cycleCount++;
		}
		MoveTowards ();
	}

	void Swoop (){
		RotateUpsideUp ();
		if(transform.position == target && target != initialPosition){
			target = initialPosition;
		} else if(transform.position == target && target == initialPosition){
			target = new Vector3 (initialPosition.x + distance, initialPosition.y - swoopDistance, initialPosition.z);
			cycleCount++;
		}
		if (cycleCount % 8 > 0 && cycleCount > 1) {
			currentMode = MotherDroneMode.PATROLLING;
			cycleCount++;
		}
//		if (cycleCount % 10 == 0  && cycleCount > 1) {
//			releasing = true;
//			currentMode = MotherDroneMode.RELEASING;
//			cycleCount++;
//		}
		MoveTowards ();
	}

	void Release() {
		myAnim.SetBool ("releasing", releasing);
		if (!releasing) {
			RotateUpsideDown ();
			if (transform.eulerAngles.z > 170f) {
				releasing = true;
			}
		}
		if (releasing) {
			if (children.Count < 3) {
				Debug.Log ("INSTANTIATING CHILDREN");
				children.Add (Instantiate (drone, new Vector3 (transform.position.x, transform.position.y - 3, transform.position.z), Quaternion.identity));
				releasing = false;
				currentMode = MotherDroneMode.PATROLLING;
			}
		}
	}

	void RotateUpsideDown(){
		transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,0f,180f), Time.deltaTime);
	}

	void RotateUpsideUp(){
		transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,0f,0f), Time.deltaTime);
	}

	void Damage(int hit){
		hitPoints -= hit;
		StartCoroutine ("Flash");
	}

	void Die(){
		foreach (Transform child in transform) {
			child.gameObject.SendMessage("Die");
		}
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

	void MoveTowards(){
		transform.position = Vector3.MoveTowards(transform.position, target, speed);
	}

	void CreateChild(){
		
	}
}
