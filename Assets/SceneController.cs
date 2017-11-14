using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	public List<IGoable> events;
	public string TransitionToSceneName;
	public string[] GONames;
	int index;

	void Start () {
		events = new List<IGoable> ();
		GONames.ToList ().ForEach ((str) => {
			Debug.Log(GameObject.Find (str).GetComponent<TypewriterText>() as IGoable);
			events.Add (GameObject.Find (str).GetComponent<TypewriterText>() as IGoable);
		});
		index = 0;
		events [index].Go (this);
	}

	public void Next(){
		index++;
		if (events.ElementAtOrDefault (index) != null) {
			events [index].Go (this);
		} else {
			Done ();
		}
	}

	public void Done(){
		Debug.Log ("Done");
		SceneManager.LoadScene (TransitionToSceneName);
	}
}
