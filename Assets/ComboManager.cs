using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ComboManager : MonoBehaviour {

	public GameObject comboMesh;
	public float cooldown;
	public int incrementer;
	public int count;
	public int maxCount;
	public bool coolingDown;
	// Use this for initialization
	void Start () {
		coolingDown = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void InitiateCombo(GameObject go){
		incrementer++;
		count++;
		if (count > 1) {
			Vector3 intendedLocation = new Vector3 (go.transform.position.x, go.transform.position.y + 2f, go.transform.position.z);
			GameObject note = Instantiate (comboMesh, intendedLocation, Quaternion.identity);
			StartCoroutine ("PushUp", note);
			note.GetComponent<TextMesh> ().text = count.ToString ();
		}
		if (coolingDown == false) {
			StartCoroutine ("Cooldown");
		}
	}

	public IEnumerator PushUp(GameObject note){
		for (var i = 0; i < 10; i++) {
			yield return new WaitForSeconds (0.1f);
			note.transform.position = Vector3.Lerp(note.transform.position, new Vector3(note.transform.position.x, note.transform.position.y + 1f, note.transform.position.z), Time.deltaTime);
		}
		Destroy (note);
	}

	public IEnumerator Cooldown(){
		coolingDown = true;
		while(incrementer > 0){
			incrementer = 0;
			yield return new WaitForSeconds (cooldown);
		}
		if (count > 1) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			Vector3 intendedLocation = new Vector3 (player.transform.position.x, player.transform.position.y + 4f, player.transform.position.z);
			GameObject note = Instantiate (comboMesh, intendedLocation, Quaternion.identity);
			note.GetComponent<TextMesh>().text = count.ToString() + "x COMBO";
			note.transform.parent = player.transform;
			int rand = Random.Range (0, 4);
			LootFactory.GetAssortedRandomLoot (count, this, note.gameObject);
			StartCoroutine ("PushUp", note);
		}
		coolingDown = false;
		count = 0;
	}
}
