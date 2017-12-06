using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable {

	void Pickup(GameObject picker);
	void Throw (bool direction, float force);
	GameObject GetGameObject();
}
