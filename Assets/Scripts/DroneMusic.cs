using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMusic : MonoBehaviour {
	public int objno;
	public TextAsset scoreFile1;
	private string score1;
	private rtcmixmain RTcmix;
	public int talkedTo;
	private int scene;

	// Use this for initialization
	void Start () {
		//Load text at beginning
		//score1 = scoreFile1.text;
		talkedTo = 1;
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
		RTcmix.SendScore ("cnum = " + talkedTo, objno);
		RTcmix.SendScoreFile ("DroneSetup", objno);
		//ChangeScene (3);
		//Scorefile loaded from text asset attached to script
		//RTcmix.SendScore(score1, objno);
		//RTcmix.SendScoreAsset (scoreFile, objno);
	}

	public void ChangeScene(int level){
		scene = level;
		if (level == 1) {
			RTcmix.SendScoreFile ("WindSetup", objno);
		}
		else if (level == 2) {
			Debug.Log ("fire");
			RTcmix.SendScoreFile ("FireSetup", objno);
		}
		else if (level == 3) {
			RTcmix.SendScoreFile ("OrganSetup", objno);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnAudioFilterRead(float[] data, int channels) {
		RTcmix.runRTcmix (data, objno, 0);

		if (RTcmix.checkbangRTcmix (objno) == 1) {
			//Debug.Log ("drone bang");
			RTcmix.SendScore ("cnum = " + talkedTo, objno);
			if (scene == 0) {
				RTcmix.SendScoreFile ("UpdateDrones", objno);
			} else if (scene == 1) {
				RTcmix.SendScoreFile ("WindUpdate", objno);
			} else if (scene == 2) {
				Debug.Log ("firef");
				RTcmix.SendScoreFile ("FireUpdate", objno);
			}
			else if (scene == 3) {
				RTcmix.SendScoreFile ("OrganUpdate", objno);
			}
		}
		RTcmix.printRTcmix (0);
	}

	void OnApplicationQuit(){
		RTcmix.destroy (objno);
	}
}