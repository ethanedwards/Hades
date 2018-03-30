using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(){
		Debug.Log ("Playing!");
		this.GetComponent<AudioSource>().Play();
	}
}
