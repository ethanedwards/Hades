using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour {
	public int objno;
	public TextAsset scoreFile1;
	private string score1;
	private rtcmixmain RTcmix;
	private Transform cam;
	private float pitch;

	// Use this for initialization
	void Start () {
		//Load text at beginning
		//score1 = scoreFile1.text;

		float[] pitches = new float[]{7.0f, 7.2f, 7.4f, 7.5f, 7.7f, 7.9f, 7.11f, 7.12f};
		int index =  Random.Range (0, 10);
		pitch = pitches [index];

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

		RTcmix.SendScoreFile ("TreeSetup", objno);
		Debug.Log ("tree setup");
		//Scorefile loaded from the "Resources" folder
		//RTcmix.SendScoreFile ("RTcmixtest", objno);
		//Scorefile loaded from text asset attached to script
		//RTcmix.SendScore(score1, objno);
		//RTcmix.SendScoreAsset (scoreFile, objno);
	}

	// Update is called once per frame
	void Update () {
		Vector3 dist = cam.position - transform.position;
		RTcmix.setpfieldRTcmix (0, dist.x, objno);
		RTcmix.setpfieldRTcmix (1, dist.y, objno);
		RTcmix.setpfieldRTcmix (2, dist.z, objno);
	}

	void OnAudioFilterRead(float[] data, int channels) {
		RTcmix.runRTcmix (data, objno, 0);

		if (RTcmix.checkbangRTcmix (objno) == 1) {
			RTcmix.SendScore ("treepitch = " + pitch, objno);
			Debug.Log ("tree bang");
			RTcmix.SendScoreFile ("UpdateTrees", objno);
		}
		RTcmix.printRTcmix (0);
	}

	void OnApplicationQuit(){
		RTcmix.destroy (objno);
	}
}
