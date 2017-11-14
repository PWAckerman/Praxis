using UnityEngine;

public interface IElectrifiable {
	//Allows object to accept "Electrify" message from electric object.
	void Electrify(GameObject src);
	void Deelectrify();
	bool isElectrified();
}
