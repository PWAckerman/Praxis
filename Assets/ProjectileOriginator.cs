using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOriginator : MonoBehaviour {

	// Use this for initialization

	public Transform transform;
	public Vector3 initialPosition;
	public GameObject prefab;
	void Start () {
		initialPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetPosition(){
		Instantiate (prefab, transform.position, Quaternion.identity);
		return transform.position;
	}

	public void PositionByAngle(int angle, bool lowered){
		int increments = angle / 45;
		Debug.Log (increments);
		switch (increments) {
		case 0:
			transform.localPosition = lowered ? new Vector3(initialPosition.x, initialPosition.y - 0.05f, initialPosition.z) : initialPosition;
			break;
		case 1:
			transform.localPosition = lowered ? new Vector3(initialPosition.x, initialPosition.y - 0.05f, initialPosition.z) : initialPosition;
			break;
		case 2:
			transform.localPosition = lowered ? new Vector3(0f, 0.28f - 0.05f, initialPosition.z) : new Vector3 (0f, 0.28f, initialPosition.z);
			break;
		case 3:
			transform.localPosition = lowered ? new Vector3 (-initialPosition.x, initialPosition.y - 0.05f, transform.localPosition.z) : new Vector3 (-initialPosition.x, initialPosition.y, transform.localPosition.z);
			break;
		case 4:
			transform.localPosition = lowered ? new Vector3 (-initialPosition.x, initialPosition.y - 0.05f, transform.localPosition.z) : new Vector3 (-initialPosition.x, initialPosition.y, transform.localPosition.z);
			break;
		case 5:
			transform.localPosition = lowered? new Vector3 (-initialPosition.x, initialPosition.y - 0.05f, transform.localPosition.z) : new Vector3 (-initialPosition.x, initialPosition.y, transform.localPosition.z);
			break;
		case -2:
			transform.localPosition = lowered ? new Vector3(0f, -0.28f, initialPosition.z) : new Vector3 (0f, -0.28f, initialPosition.z);
			break;
		case 7:
			transform.localPosition = lowered ? new Vector3(initialPosition.x, initialPosition.y - 0.05f, initialPosition.z) : initialPosition;
			break;
		}
	}

	public void Reset(){
		transform.localPosition = new Vector3 (transform.localPosition.x, initialPosition.y, initialPosition.z);
	}

	public void CenterTop(){
		transform.localPosition = new Vector3 (0f, 1f, transform.localPosition.z);
	}

	public void CenterBottom(){
		transform.localPosition = new Vector3 (0f, 1, transform.localPosition.z);
	}


	public void Flip(){
		transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
	}

	public void Lower(){
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.05f, transform.localPosition.z);
	}
}
