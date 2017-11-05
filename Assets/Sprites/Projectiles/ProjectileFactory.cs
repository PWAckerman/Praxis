using UnityEngine;
using System.Collections;

public class ProjectileFactory : MonoBehaviour{

	public GameObject prefab;
	// Use this for initialization
	public GameObject getProjectile(ProjectileManager.Mode mode, PlayerController parent){
		switch (mode) {
			case ProjectileManager.Mode.PROJECTILE1:
				return Instantiate(prefab);
				break;
			default:
				return Instantiate(prefab);	
		}
	}

}
