using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageEffects : MonoBehaviour {

	#region Variables
	public Shader curShader;
	public float brightnessAmount = 1.0f;
	public float saturationAmount = 1.0f;
	public float contrastAmount = 1.0f;
	float distance = 0;
	public RenderTexture tex;
	public int colMode;
	Texture2D samp;
	private Texture ptex;
	private Material curMaterial;

	public Text debug1;
	public Text debug2;
	public Text debug3;

	#endregion

	Material material
	{
		get
		{
			if(curMaterial == null)
			{
				curMaterial = new Material (curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}

	// Use this for initialization
	void Start () {

		//Makes so the screen never sleeps
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		colMode = 0;
		tex = new RenderTexture (Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		if (!curShader && !curShader.isSupported) {
			enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		material.SetFloat ("_YPos", (transform.position.z-5) / 1.0f);
		material.SetFloat ("_XPos", (transform.position.x) / 15.0f);

		debug1.text = "Xpos " + transform.position.x;
		debug2.text = "Ypos " + transform.position.y;
		debug3.text = "Zpos " + transform.position.z;

	}

	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture){
		if (curShader != null) {

			//material.SetFloat ("_LuminosityAmount", grayScaleAmount);
			if (samp == null) {
			//	Debug.Log ("NULL!");
			//	samp = new Texture2D(sourceTexture.width, sourceTexture.height);
			//	RenderTexture.active = sourceTexture;
			//	samp.ReadPixels(new Rect(0, 0, sourceTexture.width, sourceTexture.height), 0, 0);
			//	samp.Apply ();
			}
			//material.SetTexture("_CurTex", samp);
			material.SetFloat ("_Random1", Random.value);
			//material.SetFloat ("_Random2", Random.value);
			//material.SetFloat ("_Random3", Random.value);
			//material.SetInt ("_colMode", colMode);
			//material.SetFloat ("_Distance", distance);
			//material.SetTexture ("_PTex", ptex);
			//material.SetFloat ("_rand", Random.value );
			Graphics.Blit (sourceTexture, destTexture, material);
			//Graphics.Blit (sourceTexture, tex, material);
			//RenderTexture.active = destTexture;

			//samp.ReadPixels(new Rect(0, 0, sourceTexture.width, sourceTexture.height), 0, 0);
			//samp.Apply ();
		} else {
			Graphics.Blit (sourceTexture, destTexture);
		}
	}

	void OnDisable(){
		if(curMaterial){
			DestroyImmediate (curMaterial);
		}
	}
}
