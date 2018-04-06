using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muse : MonoBehaviour {
	AudioSource aud;
	public AudioClip[] clips;
	bool played;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
		aud.clip = clips [0];
		played = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!played&&this.GetComponent<Renderer>().isVisible){
			StartCoroutine (PlayMuse ());
			played = true;
		}
	}

	IEnumerator PlayMuse(){
		aud.Play();
		yield return new WaitForSeconds (clips [0].length+0.2f);
		aud.clip = clips [1];
		aud.Play ();

	}
}
