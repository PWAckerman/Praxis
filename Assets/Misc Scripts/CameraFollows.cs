using UnityEngine;
using System.Collections;

public class CameraFollows : MonoBehaviour {

	public Transform target;
	public Transform secondaryTarget;
	public Transform transform;
	public GameObject player;
	public float smoothing = .005f;
	private BoxCollider2D cameraBox;


	Vector3 offset;
	// Use this for initialization
	void Start () {
		cameraBox = GetComponent<BoxCollider2D> ();
		AspectRatioBoxChange ();
		offset = transform.position - target.position;
		target = player.transform;
		FollowPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerController>().currentMode == PlayerMode.PLAYER) {
			target = player.transform;
			FollowPlayer ();
		}
		if(player.GetComponent<PlayerController>().currentMode == PlayerMode.DRONE){
			target = secondaryTarget;
			FollowPlayer ();
		}
		if(player.GetComponent<PlayerController>().currentMode == PlayerMode.LOCKER){
			target = player.GetComponent<PlayerController>().locker;
			FollowPlayer ();
		}
		if (player.GetComponent<PlayerController>().currentMode == PlayerMode.ELEVATOR) {
			target = player.transform;
			FollowPlayer ();
		}
	}

	public void ShakeCamera(){
		StartCoroutine ("Shake");
	}

	public IEnumerator Shake(){
		transform.position = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
		yield return new WaitForSeconds(0.2f);
		transform.position = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
		yield return new WaitForSeconds(0.2f);
		transform.position = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
		yield return new WaitForSeconds(0.2f);
		transform.position = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
		yield return new WaitForSeconds(0.2f);
	}

	void AspectRatioBoxChange(){
		//
		float aspect = Camera.main.aspect;

		if (aspect >= (1.6f) && aspect < 1.7f) {
			cameraBox.size = new Vector2 (23, 14.3f);
		}
		//
		if (aspect >= (1.7f) && aspect < 1.8f) {
			cameraBox.size = new Vector2 (25.47f, 14.3f);
		}
		//
		if (aspect >= (1.25f) && aspect < 1.3f) {
			cameraBox.size = new Vector2 (18f, 14.3f);
		}
		//
		if (aspect >= (1.3f) && aspect < 1.4f) {
			cameraBox.size = new Vector2 (19.13f, 14.3f);
		}
		//
		if (aspect >= (1.5f) && aspect < 1.6f) {
			cameraBox.size = new Vector2 (21.6f, 14.3f);
		}
	}

	void FollowPlayer(){
		if (GameObject.Find ("Boundary")) {
			BoxCollider2D boundary = GameObject.Find ("Boundary").GetComponent<BoxCollider2D> ();
			var xMax = boundary.bounds.max.x - 5f;
			var xMin = boundary.bounds.min.x + 5f;
			var yMax = boundary.bounds.max.y - 5f;
			var yMin = boundary.bounds.min.y + 5f;
			Debug.Log (xMax);
			Debug.Log (yMax);
			Debug.Log (xMin);
			Debug.Log (yMin);
			if (transform.position.x > xMin && transform.position.x < xMax && transform.position.y > yMin && transform.position.y < yMax) {
				Time.timeScale = 1f;
			}
			Vector3 targetPosition = new Vector3 (Mathf.Clamp (target.position.x, boundary.bounds.min.x + (cameraBox.size.x / 2), boundary.bounds.max.x + (cameraBox.size.x / -2)),
				Mathf.Clamp (target.position.y, boundary.bounds.min.y + (cameraBox.size.y / 2), boundary.bounds.max.y + (cameraBox.size.y / -2)),
				                         transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1);
		}
	}
}
