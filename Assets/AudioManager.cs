using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	// Use this for initialization
	public AudioClip damage;
	public AudioClip shoot1;
	public AudioClip droneShoot;
	public AudioClip hit;
	public AudioClip destroyed;
	public AudioClip woodBreak;
	public AudioClip alert;
	public AudioClip playerDie;
	public AudioClip land;
	public AudioClip jump;
	public AudioClip drop;
	public AudioClip water;
	public AudioClip droneHovering;
	public AudioClip droneExtracting;
	public AudioClip countdown;
	public AudioClip laserOff;
	public AudioClip laserOn;
	public AudioClip droneAppear;
	public AudioClip droneDisappear;
	public AudioClip doorOpen;
	public AudioClip doorClose;
	public AudioClip elevatorMove;
	AudioSource src;
	public Dictionary<string, AudioClip> clips;


	void Start () {
		clips = new Dictionary<string, AudioClip> () {
			{ "damage", damage },
			{ "shoot1", shoot1 },
			{ "droneShoot", droneShoot },
			{"droneHovering", droneHovering},
			{"droneExtracting", droneExtracting},
			{ "hit", hit },
			{"laserOff", laserOff},
			{"laserOn", laserOn},
			{"countdown", countdown},
			{ "destroyed", destroyed },
			{ "woodBreak", woodBreak },
			{ "alert", alert },
			{ "playerDie", playerDie },
			{ "land", land },
			{ "jump", jump },
			{ "drop", drop },
			{ "water", water },
			{"droneAppear", droneAppear},
			{"droneDisappear", droneDisappear},
			{"doorOpen", doorOpen},
			{"doorClose", doorClose},
			{"elevatorMove", elevatorMove}
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(string clip){
		src = GetComponents<AudioSource> ()[Random.Range(0,2)];
		src.clip = clips [clip];
		src.Play ();
	}

	public void Loop(string clip){
		src = GetComponent<AudioSource> ();
		src.loop = true;
		src.clip = clips [clip];
		src.Play ();
	}

	public void StopLoop(){
		src = GetComponent<AudioSource> ();
		src.Stop ();
		src.loop = false;
	}

	public void PlayWithLocalAudioSource(AudioSource srce, string clip){
		srce.clip = clips [clip];
		srce.Play ();
	}

	public void LoopWithLocalAudioSource(AudioSource srce, string clip){
		srce.clip = clips [clip];
		srce.loop = true;
		srce.Play ();
	}
}
