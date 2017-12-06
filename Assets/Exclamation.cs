using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exclamation : MonoBehaviour {

	// Use this for initialization
	public bool autoDestroy;

	void Start () {
		if (autoDestroy) {
			Destroy (this.gameObject, 10f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent) {
			if (transform.parent.localScale.x < 0) {
				Vector3 scl = transform.localScale;
				scl.x = -Mathf.Abs(scl.x);
				transform.localScale = scl;
			} else {
				Vector3 scl = transform.localScale;
				scl.x = Mathf.Abs(scl.x);
				transform.localScale = scl;
			}
		}
	}
}
