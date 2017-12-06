using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAll : MonoBehaviour {

	public enum FadeMode{
		FADEIN,
		FADEOUT,
		STASIS
	}

	public FadeMode currentMode;

	List<SpriteRenderer> sprites;
	public int len;
	// Use this for initialization
	void Start () {
		sprites = new List<SpriteRenderer> ();
		if (GetComponent<SpriteRenderer> () != null) {
			SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
			sprites.Add (sprite);
		}
		sprites.AddRange (GetComponentsInChildren<SpriteRenderer> ());
		len = sprites.Count;
	}
	
	// Update is called once per frame
	void Update () {
		Fade ();
	}

	void Fade(){
		switch (currentMode) {
		case FadeMode.FADEIN:
			FadeIn ();
			break;
		case FadeMode.FADEOUT:
			FadeOut ();
			break;
		case FadeMode.STASIS:
			break;
		}
	}
		

	void FadeIn(){
		sprites.ForEach (spr => {
			spr.color = Color.Lerp (spr.color, Color.white, Time.deltaTime * 1.7f);
			if(spr.color == Color.white){
				currentMode = FadeMode.STASIS;
			}
		});
	}

	void FadeOut(){
		sprites.ForEach (spr => {
			spr.color = Color.Lerp (spr.color, Color.clear, Time.deltaTime * 1.7f);
			if(spr.color == Color.clear){
				currentMode = FadeMode.STASIS;
			}
		});
	}
}
