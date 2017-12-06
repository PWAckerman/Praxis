using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOriginator : MonoBehaviour {

	// Use this for initialization

	public Transform transform;
	public Vector3 initialPosition;
	public GameObject prefab;
	InventoryManager im;
	void Start () {
		initialPosition = transform.localPosition;
		im = InventoryManager.GetInstance ();
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
			transform.localPosition = lowered ? new Vector3(initialPosition.x, initialPosition.y - 0.05f, initialPosition.z) : new Vector3(initialPosition.x, initialPosition.y + 0.15f, initialPosition.z);
			break;
		case 2:
			transform.localPosition = lowered ? new Vector3(0f, 0.28f - 0.05f, initialPosition.z) : new Vector3 (0f, 0.4f, initialPosition.z);
			break;
		case -1:
			transform.localPosition = lowered ? new Vector3(initialPosition.x - 0.05f, initialPosition.y - 0.25f, initialPosition.z) : new Vector3(initialPosition.x, initialPosition.y - 0.4f, initialPosition.z);
			break;
		case -2:
			transform.localPosition = lowered ? new Vector3(0f, -0.2f, initialPosition.z)  : initialPosition;
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
		transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
	}

	public void Lower(){
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.05f, transform.localPosition.z);
	}
}
