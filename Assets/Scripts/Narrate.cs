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
		StartCoroutine(FadeDown ());
	}

	IEnumerator FadeDown()
	{
		ChangeVolume(-.1f, 0.35f);
		yield return new WaitForSeconds(this.GetComponent<AudioSource>().clip.length);
		ChangeVolume(0.1f, 1.0f);
		DisableChildren ();
		this.tag = "Finished";
	}

	private void ChangeVolume(float step, float vol){
		Debug.Log ("Changing volume");
		GameObject[] objs;
		objs = GameObject.FindGameObjectsWithTag ("RTcmixobj");
		foreach (GameObject obj in objs) {
			obj.GetComponent<CrossFader> ().ChangeVol (step, vol);
		}

	}

	private void DisableChildren(){
		PersonParticle part = GetComponentInChildren<PersonParticle> ();
		part.Disable ();
	}
}
