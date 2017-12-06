using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStashable {
	GameObject marker { get; set;}
	void Stash();
	void Release(Transform trans);
}
