using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextInstructions : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		StartCoroutine (Beginning ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Next(){
		StartCoroutine (Transition ());
	}

	public void Open(){
		StartCoroutine (OpenSpace ());
	}

	IEnumerator Beginning()
	{
		text.text = "Look Around with Your Camera";
		yield return new WaitForSeconds(1.0f);
		StartCoroutine (FadeUp());
		yield return new WaitForSeconds(2.5f);
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(2.0f);
		text.text = "Find the Branch and Approach It";
		StartCoroutine (FadeUp());
	}
		

	IEnumerator Transition()
	{
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(0.7f);
		text.text = "Tap the Branch to Pick it Up";
		StartCoroutine (FadeUp());
	}

	IEnumerator OpenSpace()
	{
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(0.7f);
		text.text = "Find an Open Space to Begin";
		StartCoroutine (FadeUp());
		yield return new WaitForSeconds(1.5f);
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(0.7f);
		text.text = "When You Are Ready, Look at the Ground and Tap to Place the Branch";
		StartCoroutine (FadeUp());
	}

	IEnumerator FadeDown() {
		for (float f = 1f; f > 0; f -= 0.1f) {
			Color c = text.color;
			c.a = f;
			text.color = c;
			yield return null;
		}
	}

	IEnumerator FadeUp() {
		for (float f = 0f; f <= 1; f += 0.1f) {
			Color c = text.color;
			c.a = f;
			text.color = c;
			yield return null;
		}
	}
}
