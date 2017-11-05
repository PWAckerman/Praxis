using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	// Use this for initialization
	public enum Direction{
		HORIZONTAL,
		VERTICAL
	}


	public float distance;
	public float speed;
	public Direction direction; 

	Rigidbody2D rb;
	Transform transform;
	Vector3 startingPosition;
	Vector3 endingPosition;
	Vector3 destination;

	void Start () {
		transform = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
		switch(direction){
			case Direction.HORIZONTAL:
				destination = new Vector3 (transform.position.x + distance, transform.position.y, transform.position.z);
				endingPosition = destination;
				break;
			case Direction.VERTICAL:
				destination = new Vector3 (transform.position.x, transform.position.y + distance, transform.position.z);
				endingPosition = destination;
				break;
			default:
				return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position == startingPosition) {
			Debug.Log ("LERP");
			destination = endingPosition;
			transform.position = Vector3.Lerp (startingPosition, destination, Time.deltaTime * speed);
		} else if (transform.position == endingPosition) {
			Debug.Log ("LERP2");
			destination = startingPosition;
			transform.position = Vector3.Lerp (endingPosition, destination, Time.deltaTime * speed);
		} else {
			transform.position = Vector3.Lerp (transform.position, destination, Time.deltaTime * speed);
		}
	}

	void FixedUpdate() {
		rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
	}
}
