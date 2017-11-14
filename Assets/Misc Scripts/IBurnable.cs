public interface IBurnable {

	//Object can be sent the "Burn" message when colliding with "Fire" material.
	void Burn();
	bool isBurning();
}
