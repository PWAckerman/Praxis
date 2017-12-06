using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStashTarget {
	void StashInside(IStashable go);
}
