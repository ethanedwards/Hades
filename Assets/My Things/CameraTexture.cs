using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTexture : MonoBehaviour {
	private Camera _camera;

	private Texture2D _screenShot;

	private IEnumerator TakeScreenShot()
	{
		yield return new WaitForEndOfFrame();

		int resWidth = _camera.pixelWidth;
		int resHeight = _camera.pixelHeight;

		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
		_camera.targetTexture = rt;
		_screenShot= new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
		_camera.Render();
		RenderTexture.active = rt;
		_screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
		_screenShot.Apply ();
		_camera.targetTexture = null;
		RenderTexture.active = null;
		Destroy(rt);


		Sprite tempSprite = Sprite.Create(_screenShot,new Rect(0,0,resWidth,resHeight),new Vector2(0,0));
		GameObject.Find("SpriteObject").GetComponent<SpriteRenderer>().sprite = tempSprite;
		Debug.Log ("taken");
	}
	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();
		Invoke ("SS", 5.0f);

	}
	void SS(){
		Debug.Log ("taking");
		//StartCoroutine (TakeScreenShot());
	}
	
	// Update is called once per frame
	void Update () {
		Touch myTouch = Input.GetTouch(0);

		Touch[] myTouches = Input.touches;
		for(int i = 0; i < Input.touchCount; i++)
		{
			StartCoroutine (TakeScreenShot());
		}
	}


}
