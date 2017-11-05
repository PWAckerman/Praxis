using UnityEngine;
using System.Collections;

public class ProjectileManager {
	public enum Mode {
		PROJECTILE1
	}

	Mode currentMode;
	PlayerController parent;
	ProjectileFactory factory;
	// Use this for initialization
	public ProjectileManager(PlayerController parent){
		currentMode = Mode.PROJECTILE1;
		factory = GameObject.FindGameObjectWithTag ("ProjectileFactory").GetComponent<ProjectileFactory>();
		this.parent = parent;
	}

	public void ChangeMode(Mode mode){
		currentMode = mode;
	}

	public void Fire(bool direction, int level, int angle){
		GameObject proj = factory.getProjectile(currentMode, parent);
		proj.GetComponent<IProjectile>().Fire(direction, level, angle);
	}
}
