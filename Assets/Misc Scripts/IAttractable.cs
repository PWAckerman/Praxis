using UnityEngine;

public interface IAttractable {
	//Allows object to accept "Electrify" message from electric object.
	void MoveTowards(Vector3 position);
	void ResetMass();
}
