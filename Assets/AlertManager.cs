using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlertMode{
	HIGH_ALERT,
	ALERTED,
	PATROLLING
}

public class AlertManager: MonoBehaviour {

	public AlertMode currentMode;
	public float cooldownPeriod;
	[SerializeField]
	public float tempCooldown;
	public Vector3 lastSeenPosition;
	Coroutine countdown;

	public void HighAlert(){
		CancelCountdown ();
		this.currentMode = AlertMode.HIGH_ALERT;
	}

	public void Alerted(){
		CancelCountdown ();
		this.currentMode = AlertMode.ALERTED;
		countdown = StartCoroutine ("StartCountdown");
	}

	public void CancelCountdown(){
		if (countdown != null) {
			StopCoroutine (countdown);
		}
	}

	public void Patrol(){
		this.currentMode = AlertMode.PATROLLING;
	}

	public IEnumerator StartCountdown(){
		tempCooldown = cooldownPeriod;
		while(tempCooldown > 0){
			yield return new WaitForSecondsRealtime (1f);
			tempCooldown--;
		}
		Patrol ();
	}
}
