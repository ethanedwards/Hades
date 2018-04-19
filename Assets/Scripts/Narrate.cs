using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrate : MonoBehaviour {
	// Use this for initialization
	public int color;
	void Start () {
		Color col;
		switch (color) {
		case 0:
			col = new Color (0, 1, 0, 1);
			break;
		case 1:
			col = new Color (0, 0, 1, 1);
			break;
		case 2:
			col = new Color (1, 0, 0, 1);
			break;
		case 3:
			col = new Color (1, 1, 0, 1);
			break;
		default:
			col = new Color (0, 0, 0, 1);
			break;
		}


		GetComponentInChildren<Renderer> ().material.SetColor ("_TintColor", col);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(){
		Debug.Log ("Playing!");
		this.GetComponent<AudioSource>().Play();
		StartCoroutine(FadeDown ());
		GetComponentInChildren<Renderer> ().material.SetColor ("_TintColor", new Color (0, 0, 0, 1));
	}

	IEnumerator FadeDown()
	{
		ChangeVolume(-.01f, 0.35f);
		yield return new WaitForSeconds(this.GetComponent<AudioSource>().clip.length);
		ChangeVolume(0.01f, 1.0f);
		Debug.Log ("ChildDisabled");
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
