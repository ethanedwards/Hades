﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerMusic : MonoBehaviour {
	public int objno;
	public TextAsset scoreFile1;
	private string score1;
	private rtcmixmain RTcmix;
	private Transform cam;
	private int pitch;
	private int pitchAdd;
	private int mod;
	float resetTime;
	float resetter;
	float intensity;
	bool changeLight;
	Light lit;
	private int baseNote;

	// Use this for initialization
	void Start () {
		baseNote = Random.Range (80, 160);
		resetter = Random.Range (0.1f, 1f);
		changeLight = false;
		resetTime = 0;
		//Load text at beginning
		//score1 = scoreFile1.text;
		//lit = GetComponentInChildren<Light>();
		//lit.intensity = 5.0f;
		pitch = Random.Range (0, 15);
		pitchAdd = 0;
		mod = Random.Range (3, 5);

		RTcmix = GameObject.Find ("RTcmixmain").GetComponent<rtcmixmain> ();
		cam = Camera.main.transform;
		if (RTcmix == null) {
			Debug.Log ("Error! No RTcmixmain prefab object in scene");
		} else {
			RTcmix.initRTcmix (objno);
			StartCoroutine(LateStart(0.1f));






		}

	}

	IEnumerator LateStart(float time)
	{
		yield return new WaitForSeconds(time);

		RTcmix.SendScoreFile ("CrackerSetup", objno);
		Debug.Log ("fire setup");
		//Scorefile loaded from the "Resources" folder
		//RTcmix.SendScoreFile ("RTcmixtest", objno);
		//Scorefile loaded from text asset attached to script
		//RTcmix.SendScore(score1, objno);
		//RTcmix.SendScoreAsset (scoreFile, objno);
	}

	// Update is called once per frame
	void Update () {
		if (changeLight) {
			changeLight = false;
			//StartCoroutine (FadeDown (0));
		}
		if (Time.time > resetter + resetTime) {
			//lit.intensity = Random.Range (2f, 5f);
			resetTime = Time.time;
			resetter = Random.Range (0.001f, 0.2f);
		}
		Vector3 dist = cam.position - transform.position;
		float distance = Vector3.Distance (cam.position, transform.position);
		RTcmix.setpfieldRTcmix (0, dist.x, objno);
		RTcmix.setpfieldRTcmix (1, dist.y, objno);
		RTcmix.setpfieldRTcmix (2, dist.z, objno);
		RTcmix.setpfieldRTcmix (3, Mathf.Max(1.0f-distance*distance/25.0f, 0.0f), objno);
	}

	void OnAudioFilterRead(float[] data, int channels) {
		RTcmix.runRTcmix (data, objno, 0);

		if (RTcmix.checkbangRTcmix (objno) == 1) {
			//pitchAdd++;
			//pitchAdd = pitchAdd % mod;


			Debug.Log ("fire bang");
			string score = "cpitch = " + baseNote;
			//LightFade ();
			Debug.Log (score);

			RTcmix.SendScore (score, objno);
			RTcmix.SendScoreFile ("CrackerUpdate", objno);

			//StartCoroutine (FadeDown (0));
		}
		RTcmix.printRTcmix (0);
	}

	void OnApplicationQuit(){
		RTcmix.destroy (objno);
	}

	void LightFade(){
		//Debug.Log ("ran");
		intensity = 1.5f;
		changeLight = true;
		//StartCoroutine (FadeDown (0));
	}

	IEnumerator FadeDown(float wait) {

		yield return new WaitForSeconds(wait);
		for (float f = intensity; f > 0; f -= 0.01f) {
			lit.intensity = f;
			yield return null;
		}
	}
}