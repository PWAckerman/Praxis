using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerVision : MonoBehaviour {

	// Use this for initialization
	public bool seePlayer;
	[SerializeField]
	bool seeExplosive;
	[SerializeField]
	bool seeComrade;
	public float fieldOfVision;
	public float angle;
	public GameObject miner;
	GameObject player;
	GameObject alertManager;
	Coroutine flipCycle;
	bool facingLeft;
	public SpriteRenderer rd;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		alertManager = GameObject.Find("ComboManager");
		facingLeft = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerStay2D(Collider2D coll){
		if (miner.GetComponent<IEnemy> ().currentMode != EnemyMode.TRANQUILIZED) {
			Vector3 direction = coll.gameObject.transform.position - transform.position;
			angle = Vector3.Angle (direction, transform.right);
			if (coll.gameObject.layer == 16 && miner.GetComponent<IEnemy> ().currentMode == EnemyMode.FLEEING) {
				Vector3 point = coll.gameObject.transform.position;
				miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x, miner.transform.position.y, miner.transform.position.y));
				miner.GetComponent<IEnemy> ().ChangeMode (EnemyMode.FUCKED);
			}
			if (coll.gameObject.name == "RegularGrenade(Clone)" && miner.GetComponent<IEnemy> ().currentMode != EnemyMode.FUCKED && miner.GetComponent<IEnemy> ().currentMode != EnemyMode.FLEEING) {
				seeExplosive = true;
				Vector3 point = coll.gameObject.transform.position;
				miner.GetComponent<IEnemy> ().ChangeMode (EnemyMode.FLEEING);
				alertManager.GetComponent<AlertManager> ().Alerted ();
				if (transform.position.x < point.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x - 15f, miner.transform.position.y, point.z));
				} else if (transform.position.x >= point.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x + 15f, miner.transform.position.y, point.z));
				}
			} else {
				seeExplosive = false;
			}
			if (coll.gameObject.tag == "Lava" && miner.GetComponent<IEnemy> ().currentMode != EnemyMode.FUCKED && miner.GetComponent<IEnemy> ().currentMode != EnemyMode.FLEEING) {
				seeExplosive = true;
				Vector3 point = coll.gameObject.transform.position;
				miner.GetComponent<IEnemy> ().ChangeMode (EnemyMode.FLEEING);
				alertManager.GetComponent<AlertManager> ().Alerted ();
				if (transform.position.x < point.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x - 15f, miner.transform.position.y, point.z));
				} else if (transform.position.x >= point.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x + 15f, miner.transform.position.y, point.z));
				}
			} else {
				seeExplosive = false;
			}
			if (coll.gameObject.layer == 15) {
				seeComrade = true;
				Vector3 point = coll.gameObject.transform.position;
				if (Mathf.Abs (Vector3.Distance (point, miner.transform.position)) < 3f && !coll.gameObject.GetComponent<ITranquilizeable>().IsTranquilized()) {
					miner.SendMessage ("StartDefer", coll.gameObject); 
				}
				if (Mathf.Abs (Vector3.Distance (point, miner.transform.position)) < 3f && coll.gameObject.GetComponent<ITranquilizeable>().IsTranquilized()) {
					coll.gameObject.SendMessage ("Wake");
					miner.GetComponent<MinerController> ().dm.ShowWake ();
				}
			} else {
				seeComrade = false;
			}
			if (coll.gameObject == player) {
				Debug.DrawLine (coll.gameObject.transform.position, transform.position, Color.red);
				if (!miner.GetComponent<MinerController> ().facingLeft) {
					if (angle < fieldOfVision / 2) {
						CheckForPlayer (angle, direction);
					} else {
						seePlayer = false;
						if (alertManager.GetComponent<AlertManager> ().currentMode == AlertMode.HIGH_ALERT) {
							alertManager.GetComponent<AlertManager> ().Alerted ();
						}
					}
				}
				if (miner.GetComponent<MinerController> ().facingLeft) {
					if (angle < ((fieldOfVision / 2) + 180f)) {
						CheckForPlayer (angle, direction);
					} else {
						seePlayer = false;
						if (alertManager.GetComponent<AlertManager> ().currentMode == AlertMode.HIGH_ALERT) {
							alertManager.GetComponent<AlertManager> ().Alerted ();
						}
					}
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (miner.GetComponent<IEnemy> ().currentMode != EnemyMode.TRANQUILIZED) {
			if (coll.gameObject == player && miner.GetComponent<IEnemy> ().currentMode != EnemyMode.FLEEING && alertManager.GetComponent<AlertManager> ().currentMode == AlertMode.HIGH_ALERT) {
				seePlayer = false;
				alertManager.GetComponent<AlertManager> ().Alerted ();
			}
		}
	}

	bool IsNotObscured(Vector3 position, Vector3 direction){
		Debug.DrawLine (position, direction, Color.red);
		RaycastHit2D hit = Physics2D.Raycast (position, direction);
		return hit.collider.gameObject == player;
	}

	bool IsNotObscuredExplosive(Vector3 position, Vector3 direction){
		Debug.DrawLine (position, direction, Color.red);
		RaycastHit2D hit = Physics2D.Raycast (position, direction);
		return hit.collider.gameObject.layer == 11;
	}

	void CheckForExplosive(float angle, Vector3 direction){
		if (IsNotObscured (transform.position, direction)) {
			seeExplosive = true;
		} else {
			seeExplosive = false;
		}
		
	}

	void CheckForComrade(float angle, Vector3 direction){
		if (IsNotObscured (transform.position, direction)) {
			seeComrade = true;
		} else {
			seeComrade = false;
		}
	}

	void CheckForPlayer(float angle, Vector3 direction){
		if (IsNotObscured (transform.position, direction) && player.GetComponent<PlayerController> ().currentMode != PlayerMode.LOCKER && player.GetComponent<PlayerController> ().currentMode != PlayerMode.ELEVATOR) {
			seePlayer = true;
			if (miner.GetComponent<IEnemy> ().currentMode != EnemyMode.FLEEING) {
				miner.GetComponent<MinerController> ().target = player.transform.position;
				alertManager.GetComponent<AlertManager> ().lastSeenPosition = player.transform.position;
				alertManager.GetComponent<AlertManager> ().HighAlert ();
				if (miner.GetComponent<MinerController> ().unarmed && player.GetComponent<PlayerController> ().aiming) {
					Vector3 point = player.transform.position;
					miner.GetComponent<IEnemy> ().ChangeMode (EnemyMode.FLEEING);
					if (transform.position.x < player.transform.position.x) {
						miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (player.transform.position.x + 15f, miner.transform.position.y, point.z));
					} else if (transform.position.x >= player.transform.position.x) {
						miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (player.transform.position.x - 15f, miner.transform.position.y, point.z));
					}
				} else if (miner.GetComponent<MinerController> ().currentMode != EnemyMode.ALERTED && miner.GetComponent<MinerController> ().currentMode != EnemyMode.PURSUING ){
					miner.GetComponent<MinerController> ().currentMode = EnemyMode.ALERTED;
				}
				if (!miner.GetComponent<MinerController> ().unarmed && miner.GetComponent<MinerController> ().currentMode != EnemyMode.ALERTED && miner.GetComponent<MinerController> ().currentMode != EnemyMode.PURSUING) {
					Debug.Log ("Alerted!");
					miner.GetComponent<MinerController> ().currentMode = EnemyMode.ALERTED;
				}
			}else if(miner.GetComponent<IEnemy> ().currentMode == EnemyMode.FLEEING){
				Vector3 point = direction;
				if (transform.position.x < player.transform.position.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (miner.GetComponent<MinerController>().target.x - 15f, miner.transform.position.y, point.z));
				} else if (transform.position.x >= player.transform.position.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (miner.GetComponent<MinerController>().target.x + 15f, miner.transform.position.y, point.z));
				}
			} else {
				Vector3 point = direction;
				miner.GetComponent<IEnemy> ().ChangeMode (EnemyMode.FLEEING);
				if (transform.position.x < point.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x - 15f, miner.transform.position.y, point.z));
				} else if (transform.position.x >= point.x) {
					miner.GetComponent<IEnemy> ().SetTarget (new Vector3 (point.x + 15f, miner.transform.position.y, point.z));
				}
			}
		} else {
			seePlayer = false;
			if (alertManager.GetComponent<AlertManager> ().currentMode == AlertMode.HIGH_ALERT) {
				alertManager.GetComponent<AlertManager> ().Alerted ();
			}
		}
	}
}
