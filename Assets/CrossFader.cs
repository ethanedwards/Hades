using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFader : MonoBehaviour {
	AudioSource aud;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Fade(float step, float stop) {
		if (step<0) {
			for (float f = aud.volume; f >= stop; f += step) {
				aud.volume = f;
				yield return null;
			}
		} else {
			for (float f = aud.volume; f <= stop; f += step) {
				aud.volume = f;
				yield return null;
			}
		}
	}

	public void ChangeVol(float step, float stop){
		StartCoroutine (Fade (step, stop));
	}
		
}
