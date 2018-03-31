using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMusic : MonoBehaviour {
	public int objno;
	public TextAsset scoreFile1;
	private string score1;
	private rtcmixmain RTcmix;

	// Use this for initialization
	void Start () {
		//Load text at beginning
		//score1 = scoreFile1.text;

		RTcmix = GameObject.Find ("RTcmixmain").GetComponent<rtcmixmain> ();
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

		//RTcmix.SendScore ("WAVETABLE(0, 3.5, 20000, 440.0)", objno);
		//Scorefile loaded from the "Resources" folder
		RTcmix.SendScoreFile ("DroneSetup", objno);
		//Scorefile loaded from text asset attached to script
		//RTcmix.SendScore(score1, objno);
		//RTcmix.SendScoreAsset (scoreFile, objno);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnAudioFilterRead(float[] data, int channels) {
		RTcmix.runRTcmix (data, objno, 0);

		if (RTcmix.checkbangRTcmix (objno) == 1) {
			RTcmix.SendScoreFile ("UpdateDrones", objno);
		}
		RTcmix.printRTcmix (0);
	}

	void OnApplicationQuit(){
		RTcmix.destroy (objno);
	}
}