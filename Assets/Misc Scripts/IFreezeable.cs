public interface IFreezeable {
	//Allows object to accept "Electrify" message from electric object.
	void Freeze();
	void Melt();
	bool isFrozen();
}
