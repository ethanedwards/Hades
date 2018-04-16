using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerScene : MonoBehaviour {
	public GameObject sceneRoot;
	bool spawned;
	// Use this for initialization
	void Start () {
		spawned = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > 9.5f && !spawned) {
			Vector3 newPos = transform.position + Camera.main.transform.forward * 6;
			newPos.y = -0.8f;
			sceneRoot.transform.position = newPos;
			sceneRoot.SetActive (true);
			spawned = true;
		}
	}
}
