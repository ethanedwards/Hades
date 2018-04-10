using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceShader : MonoBehaviour {
	Material[] mats;
	// Use this for initialization
	void Start () {
		mats = GetComponent<Renderer> ().materials;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Material mat in mats) {
			Debug.Log ("did it");
			mat.SetFloat ("_Distance", Vector3.Distance (transform.position, Camera.main.transform.position));
		}
	}
}
