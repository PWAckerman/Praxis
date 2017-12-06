using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ItemPanel : MonoBehaviour {

	public TextMesh name;
	public TextMesh description;
	public SpriteRenderer icon;
	public GameModeManager gm;
	public AudioManager am;
	[SerializeField]
	float timeScale;
	// Use this for initialization
	void Awake () {
		
	}

	void Start() {
		gm = GameModeManager.getInstance ();
	}

	public void Show(string name){
		am.Play ("newItem");
		Time.timeScale = 0;
		timeScale = Time.timeScale;
		gameObject.SetActive (true);
		icon.sprite = Resources.Load<Sprite>(name);
	}
	
	// Update is called once per frame
	void Update () {
		timeScale = Time.timeScale;
		if (Input.GetKeyDown (KeyCode.Joystick1Button17)) {
			gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}
}
