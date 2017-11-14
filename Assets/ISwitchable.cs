using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchable {

	bool on{get; set;}
	void TurnOff();
	void TurnOn ();
}
