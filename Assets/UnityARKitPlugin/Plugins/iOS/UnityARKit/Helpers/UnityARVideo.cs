using System;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.XR.iOS
{

    public class UnityARVideo : MonoBehaviour
    {
        public Material m_ClearMaterial;

        private CommandBuffer m_VideoCommandBuffer;
        private Texture2D _videoTextureY;
        private Texture2D _videoTextureCbCr;
		private Matrix4x4 _displayTransform;

		float ModFade;

		private bool bCommandBufferInitialized;

		public void Start()
		{
			UnityARSessionNativeInterface.ARFrameUpdatedEvent += UpdateFrame;
			bCommandBufferInitialized = false;
			Color col = Color.HSVToRGB(0f, 0f, 255/255.0f);
			Debug.Log ("col " + col);
			m_ClearMaterial.SetColor("_Color", col);
		}

		void UpdateFrame(UnityARCamera cam)
		{
			_displayTransform = new Matrix4x4();
			_displayTransform.SetColumn(0, cam.displayTransform.column0);
			_displayTransform.SetColumn(1, cam.displayTransform.column1);
			_displayTransform.SetColumn(2, cam.displayTransform.column2);
			_displayTransform.SetColumn(3, cam.displayTransform.column3);		
		}

		void InitializeCommandBuffer()
		{
			m_VideoCommandBuffer = new CommandBuffer(); 
			m_VideoCommandBuffer.Blit(null, BuiltinRenderTextureType.CurrentActive, m_ClearMaterial);
			GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeForwardOpaque, m_VideoCommandBuffer);
			bCommandBufferInitialized = true;

		}

		void OnDestroy()
		{
			GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, m_VideoCommandBuffer);
			UnityARSessionNativeInterface.ARFrameUpdatedEvent -= UpdateFrame;
			bCommandBufferInitialized = false;
		}

#if !UNITY_EDITOR

        public void OnPreRender()
        {
			ARTextureHandles handles = UnityARSessionNativeInterface.GetARSessionNativeInterface ().GetARVideoTextureHandles();
            if (handles.textureY == System.IntPtr.Zero || handles.textureCbCr == System.IntPtr.Zero)
            {
                return;
            }

            if (!bCommandBufferInitialized) {
                InitializeCommandBuffer ();
            }

            Resolution currentResolution = Screen.currentResolution;

            // Texture Y
            if (_videoTextureY == null) {
              _videoTextureY = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
                  TextureFormat.R8, false, false, (System.IntPtr)handles.textureY);
              _videoTextureY.filterMode = FilterMode.Bilinear;
              _videoTextureY.wrapMode = TextureWrapMode.Repeat;
              m_ClearMaterial.SetTexture("_textureY", _videoTextureY);
            }

            // Texture CbCr
            if (_videoTextureCbCr == null) {
              _videoTextureCbCr = Texture2D.CreateExternalTexture(currentResolution.width, currentResolution.height,
                  TextureFormat.RG16, false, false, (System.IntPtr)handles.textureCbCr);
              _videoTextureCbCr.filterMode = FilterMode.Bilinear;
              _videoTextureCbCr.wrapMode = TextureWrapMode.Repeat;
              m_ClearMaterial.SetTexture("_textureCbCr", _videoTextureCbCr);
            }

            _videoTextureY.UpdateExternalTexture(handles.textureY);
            _videoTextureCbCr.UpdateExternalTexture(handles.textureCbCr);

			m_ClearMaterial.SetMatrix("_DisplayTransform", _displayTransform);
			EthanSet();
        }
#else

		public void SetYTexure(Texture2D YTex)
		{
			_videoTextureY = YTex;
		}

		public void SetUVTexure(Texture2D UVTex)
		{
			_videoTextureCbCr = UVTex;
		}

		public void OnPreRender()
		{

			if (!bCommandBufferInitialized) {
				InitializeCommandBuffer ();
			}

			m_ClearMaterial.SetTexture("_textureY", _videoTextureY);
			m_ClearMaterial.SetTexture("_textureCbCr", _videoTextureCbCr);

			m_ClearMaterial.SetMatrix("_DisplayTransform", _displayTransform);

			//Added by Ethan
			EthanSet();
		}



 
#endif

		void EthanSet(){
			m_ClearMaterial.SetFloat ("_Random1", Random.value);
			m_ClearMaterial.SetFloat ("_ModFade", ModFade);
		}

		public void ChangeColor(int level){
			Color col;
			Debug.Log ("changed color " + level);
			switch (level) {
			case 0:
				
				col = Color.HSVToRGB (124/359.0f, 38/255.0f, 255/255.0f);
				break;
			case 1:
				col = Color.HSVToRGB (222/359.0f, 38/255.0f, 255/255.0f);
				break;
			case 2:
				col = Color.HSVToRGB (0/359.0f, 38/255.0f, 255/255.0f);
				break;
			case 3:
				col = Color.HSVToRGB (53/359.0f, 38/255.0f, 255/255.0f);
				break;
			case 4:
				col = new Color (255, 255, 255, 255);
				break;
			default:
				col = Color.HSVToRGB (0/359.0f, 0/255.0f, 255/255.0f);
				break;
			}
			Debug.Log ("changed color to " + col);
			m_ClearMaterial.SetColor("_Color", col);
		}

		public void FadeBW(){
			StartCoroutine (FadeUp ());
		}


		IEnumerator FadeUp() {
			for (float f = 0f; f < 1; f += 0.005f) {
				ModFade = f;
				yield return null;
			}
		}
    }
}
