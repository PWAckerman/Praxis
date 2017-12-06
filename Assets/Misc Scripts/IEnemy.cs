using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

	EnemyMode currentMode {get;set;}

	void ChangeMode(EnemyMode mode);
	void FaceTarget(Vector3 target);
	int GetCurrentHitPoints();
	int GetMaxHitPoints();
	void FaceTarget();
	void SetTarget (Vector3 target);
}
