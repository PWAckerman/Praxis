using UnityEngine;
using System.Collections;

public class ProjectileFactory : MonoBehaviour{

	public GameObject prefab;
	public GameObject prefab2;
	public GameObject grenade;
	public GameObject chaffGrenade;
	public GameObject tranquilizer;
	InventoryManager im;

	void Start(){
		im = InventoryManager.GetInstance ();
	}
	// Use this for initialization
	public GameObject getProjectile(ProjectileManager.Mode mode, Vector3 position){
		switch (mode) {
		case ProjectileManager.Mode.PROJECTILE1:
				GameObject instance = Instantiate (prefab, position, Quaternion.identity);
				return instance;
				break;
		case ProjectileManager.Mode.TRANQUILIZER:
				instance = Instantiate (tranquilizer, position, Quaternion.identity);
				return instance;
				break;
		case ProjectileManager.Mode.ENEMY:
				instance = Instantiate (prefab2, position, Quaternion.identity);
				return instance;
				break;
		case ProjectileManager.Mode.GRENADE:
				instance = Instantiate (grenade, position, Quaternion.identity);
				Debug.Log (instance);
				return instance;
				break;
		case ProjectileManager.Mode.CHAFF_GRENADE:
				instance = Instantiate (chaffGrenade, position, Quaternion.identity);
				Debug.Log (instance);
				return instance;
				break;
			default:
				instance = Instantiate (prefab, position, Quaternion.identity);
				return instance;	
		}
	}

}
