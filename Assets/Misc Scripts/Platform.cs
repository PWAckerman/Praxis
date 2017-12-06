using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, ISwitchable {

	// Use this for initialization
	public enum Direction{
		HORIZONTAL,
		VERTICAL
	}
		

	public float distance;
	public float speed;
	public Direction direction;
	public bool on{ get; set;}
	public bool initialState;
	public SwitchableState state;

	Rigidbody2D rb;
	Transform transform;
	Vector3 startingPosition;
	Vector3 endingPosition;
	Vector3 destination;

	void Start () {
		transform = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
		on = initialState;
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

	public void TurnOff(){
		on = false;
	}

	public void TurnOn (){
		on = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position == startingPosition) {
			destination = endingPosition;
			if (on) {
				transform.position = Vector3.MoveTowards (startingPosition, destination, Time.deltaTime * speed);
			}
		} else if (transform.position == endingPosition) {
			destination = startingPosition;
			if (on) {
				transform.position = Vector3.MoveTowards (endingPosition, destination, Time.deltaTime * speed);
			}
		} else {
			if (on) {
				transform.position = Vector3.MoveTowards (transform.position, destination, Time.deltaTime * speed);
			}
		}
	}

	void FixedUpdate() {
//		rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
	}
}
