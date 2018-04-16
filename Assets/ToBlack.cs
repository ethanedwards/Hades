using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToBlack : MonoBehaviour {
	public Image fadeImage;
	// Use this for initialization
	void Start () {
		StartCoroutine (Activate ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Black(){
		Color col = fadeImage.color;
		col.a = 1.0f;
		fadeImage.color = col;
	}
	void End(){
		SceneManager.LoadScene (0);
	}

	IEnumerator Activate() {
		yield return new WaitForSeconds(40.0f);
		Black ();
		yield return new WaitForSeconds(3.0f);
		End ();
	}
}
