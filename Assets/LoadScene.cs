using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public int sceneNum;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Load(){
		Debug.Log ("Loading Entrance");
		SceneManager.LoadScene (sceneNum);
	}
}
