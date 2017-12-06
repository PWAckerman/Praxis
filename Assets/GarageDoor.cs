using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageDoor : MonoBehaviour, ISwitchable, IUnlockable {
	public bool on { get; set;}
	public bool locked;
	Animator myAnim;
	public GameObject contents;
	public GameObject instance;
	public InventoryManager im;
	public string neededItem;
	public bool instantiated;

	void Start(){
		myAnim = GetComponent<Animator> ();
		instantiated = false;
		im = InventoryManager.GetInstance ();
	}

	public void TurnOff(){
		on = false;
		myAnim.SetBool ("on", false);
	}

	public bool IsLocked(){
		return locked;
	}

	public void Unlock(){
		if (im.currentItem.name == neededItem) {
			locked = false;
			im.RemoveItem (im.currentItem);
		}
	}

	public void TurnOn(){
		Unlock ();
		if (!locked) {
			on = true;
			ProduceContents ();
			myAnim.SetBool ("on", true);
		}
		if (locked) {
			GameObject.FindGameObjectWithTag ("PlayerDialogue").GetComponent<PlayerDialogueManager> ().TriggerText ("I need a key for this.");
		}
	}

	public void ProduceContents(){
		if (!instantiated) {
			instance = Instantiate (contents, transform.position, Quaternion.identity);
			instantiated = true;
		}
	}

	public void FixSortingOrder(){
		instance.GetComponent<ISortable> ().FixSort ();
	}
}
