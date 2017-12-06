using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBack : MonoBehaviour {

	public bool playerIsPresent;
	public GameObject parent;
	public AlertManager alertManager;

	public void Start(){
		playerIsPresent = false;
	}
	// Use this for initialization
	public void OnTriggerStay2D(Collider2D coll){
		if (parent.GetComponent<IEnemy> ().currentMode != EnemyMode.TRANQUILIZED) {
			if (coll.gameObject.name == "Player") {
				playerIsPresent = true;
				if (coll.gameObject.GetComponent<PlayerController> ().aiming && parent.GetComponent<IEnemy> ().currentMode != EnemyMode.SURPRISED && parent.GetComponent<IEnemy> ().currentMode != EnemyMode.FLEEING && parent.GetComponent<IEnemy> ().currentMode != EnemyMode.FUCKED) {
					Debug.Log ("Surprise2");
					parent.GetComponent<ISurpriseable> ().BeSurprised ();
				} else if (!coll.gameObject.GetComponent<PlayerController> ().aiming && parent.GetComponent<IEnemy> ().currentMode == EnemyMode.SURPRISED) {
					alertManager.HighAlert ();
					parent.GetComponent<IEnemy> ().ChangeMode (EnemyMode.ALERTED);
					parent.GetComponent<IEnemy> ().FaceTarget (coll.gameObject.transform.position);
				}
			}
		}
//		if (coll.gameObject.layer == 15) {
//			coll.gameObject.SendMessage ("StartDefer");
//		}
	}

	public void OnTriggerEnter2D(Collider2D coll){
		if (parent.GetComponent<IEnemy> ().currentMode != EnemyMode.TRANQUILIZED) {
			if (parent.GetComponent<IEnemy> ().currentMode == EnemyMode.PURSUING) {
				if (coll.gameObject.name == "Player") {
					playerIsPresent = true;
					parent.GetComponent<IEnemy> ().SetTarget (coll.gameObject.transform.position);
					parent.GetComponent<IEnemy> ().ChangeMode (EnemyMode.ALERTED);
					parent.GetComponent<IEnemy> ().FaceTarget (coll.gameObject.transform.position);
				}
//				if (coll.gameObject.layer == 15) {
//					coll.gameObject.SendMessage ("StartDefer");
//				}
			}
		}
	}

	public void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.name == "Player") {
			playerIsPresent = false;
		}
	}
		
}
