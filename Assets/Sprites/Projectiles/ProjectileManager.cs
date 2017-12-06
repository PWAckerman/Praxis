using UnityEngine;
using System.Collections;

public class ProjectileManager {
	public enum Mode {
		PROJECTILE1,
		TRANQUILIZER,
		ENEMY,
		GRENADE,
		CHAFF_GRENADE
	}

	Mode currentMode;
	ProjectileFactory factory;
	// Use this for initialization
	public ProjectileManager(){
		currentMode = Mode.PROJECTILE1;
		factory = GameObject.FindGameObjectWithTag ("ProjectileFactory").GetComponent<ProjectileFactory>();

	}

	public void ChangeMode(Mode mode){
		currentMode = mode;
	}

	public void Fire(bool direction, int level, int angle, Vector3 origin, Mode mode){
		GameObject proj = factory.getProjectile(mode, new Vector3(origin.x, origin.y, 0f));
		Debug.Log ("DID GRENADE MAKE IT THIS FAR");
		proj.GetComponent<IProjectile>().Fire(direction, angle, angle);
	}
}
