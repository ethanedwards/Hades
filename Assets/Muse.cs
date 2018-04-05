using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muse : MonoBehaviour {
	AudioSource aud;
	bool played;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
		played = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!played&&this.GetComponent<Renderer>().isVisible){
			aud.Play();
			played = true;
		}
	}
}
