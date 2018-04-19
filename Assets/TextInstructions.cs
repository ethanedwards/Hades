using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextInstructions : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		Color c = text.color;
		c.a = 0;
		text.color = c;
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

	public void Elysium(){
		StartCoroutine (BeginElysium ());
	}

	public void Fade(bool first){
		if (!first) {
			StartCoroutine (FadeDown ());
		}
	}

	IEnumerator Beginning()
	{
		Debug.Log("begin");
		text.text = "Look Around with Your Camera";
		yield return new WaitForSeconds(3.0f);
		StartCoroutine (FadeUp());
		yield return new WaitForSeconds(6.5f);
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(4.0f);
		text.text = "Follow the Flame";
		StartCoroutine (FadeUp());
	}
		

	IEnumerator Transition()
	{
		Debug.Log("trans");
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(0.7f);
		text.text = "Tap the Screen on the Branch to Pick it Up";
		StartCoroutine (FadeUp());
	}

	IEnumerator BeginElysium()
	{
		StartCoroutine (FadeDown());
		yield return new WaitForSeconds(4.0f);
		text.text = "Tap the Spirits to Interact";
		StartCoroutine (FadeUp());
		//yield return new WaitForSeconds(8.0f);
		//StartCoroutine (FadeDown());
	}

	IEnumerator OpenSpace()
	{
		Debug.Log("open");
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
		for (float f = 1f; f > -0.1; f -= 0.1f) {
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
