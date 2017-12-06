using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TextWrapMesh : MonoBehaviour {

	TextMesh textMesh;
	string prevText;
	List<string> strList;
	public int lineSize;
	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh> ();
		strList = new List<string> ();
	}

	void SplitText(){
		
		string line = "";
		char[] splits = { ' ' };
		List<string> lines = new List<string> ();
		string[] strings = textMesh.text.Split (splits);
		strings.ToList ().ForEach (str => {
			Debug.Log(str);
			if((str.Length + line.Length) < lineSize){
				line += str;
				line += " ";
			} else {
				lines.Add(line);
				line = "";
				line += str;
				line += " ";
			}
		});
		if (line != "") {
			lines.Add(line);
		}
		textMesh.text = String.Join ("\n", lines.ToArray());
		prevText = textMesh.text;
	}
	// Update is called once per frame
	void Update () {
		if (prevText != textMesh.text) {
			SplitText ();
		}
	}
}
