using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public string message;
	public string[] messages;
	bool triggered = false;
	public bool multiple;

	public void OnTriggerEnter2D(Collider2D coll){
		if (!triggered) {
			if (coll.name == "Player") {
				if (!multiple) {
					PlayerDialogueManager mgr = coll.GetComponentInChildren<PlayerDialogueManager> ();
					string mutated = message.Replace ("\\n", "\n");
					mutated = mutated.Replace ("\\", "");
					mgr.TriggerText (mutated);
					triggered = true;
				} else if (multiple) {
					PlayerDialogueManager mgr = coll.GetComponentInChildren<PlayerDialogueManager> ();
					StartCoroutine ("ShowMessages", mgr);
				}
			}
		}
	}

	IEnumerator ShowMessages(PlayerDialogueManager mgr){
		triggered = true;
		foreach(string str in messages.ToList()){
			string mutated = str.Replace ("\\n", "\n");
			mutated = mutated.Replace ("\\", "");
			mgr.TriggerText (mutated);
			yield return new WaitForSeconds(5f);
		}
	}
}
