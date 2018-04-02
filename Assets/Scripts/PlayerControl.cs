﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class PlayerControl : MonoBehaviour {

		public GameObject sceneRoot;
		Vector3 center;
		public GameObject crowd;
		public GameObject Elysium;
		public float maxRayDistance = 10.0f;
		public LayerMask collisionLayer = 1 << 10;
		bool narrating;
		bool placed;
		bool approached;
		bool pickedUp;
		bool spawned;
		public bool Entrance = false;
		public GameObject text;
		public Image fadeImage;

		// Use this for initialization
		void Start () {
			narrating = false;
			placed = false;
			approached = false;
			spawned = false;
		}

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					
					Debug.Log ("Got hit!");
					if (!placed) {
						sceneRoot.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
						//sceneRoot.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
						sceneRoot.SetActive (true);
						Debug.Log ("placed");
						placed = true;
						return true;
					}
				}
			}
			return false;
		}
		
		// Update is called once per frame
		void Update () {
			if (Entrance) {

				if (Time.time > 2.5f&&!spawned) {
					sceneRoot.transform.position = transform.position - Camera.main.transform.forward;
					sceneRoot.SetActive (true);
					spawned = true;

				}

				if (pickedUp && !placed) {

					sceneRoot.transform.position = transform.position + Camera.main.transform.forward;

					if (Input.GetMouseButtonDown (0)) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
							Debug.Log ("PLACED");
							center = hit.transform.position;
							sceneRoot.transform.position = center;
							pickedUp = false;
							placed = true;
							GameObject.Find ("GeneratePlanes").SetActive (false);
							crowd.transform.position = center;
							crowd.SetActive (true);

							StartCoroutine (FadeUp (4.0f));
							StartCoroutine (FadeDown (7.0f));
							StartCoroutine(BeginElysium ());
						}
					}


					/*
					if (Input.GetMouseButtonDown (0)) {
						Debug.Log ("input");
						ARPoint point = new ARPoint {
							x = Camera.main.pixelWidth / 2,
							y = Camera.main.pixelHeight / 2
						};

						// prioritize reults types
						ARHitTestResultType[] resultTypes = {
							ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
							// if you want to use infinite planes use this:
							//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
							ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
							ARHitTestResultType.ARHitTestResultTypeFeaturePoint
						}; 

						foreach (ARHitTestResultType resultType in resultTypes) {
							if (HitTestWithResultType (point, resultType)) {
								if (pickedUp) {
									Debug.Log ("Done!");
								}
								return;
							}
						}
					}
					*/
				}


				if (Vector3.Distance (transform.position, sceneRoot.transform.position) < 1.0 && !approached) {
					Debug.Log ("next");
					text.GetComponent<TextInstructions> ().Next ();
					approached = true;
				}
				if (Input.GetMouseButtonDown (0) && pickedUp == false && approached) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
						Debug.Log ("pickup");
						if (approached && hit.transform.tag == "Branch") {
							Debug.Log (pickedUp);
							pickedUp = true;
							placed = false;
							GetComponent<UnityARVideo> ().FadeBW ();
							text.GetComponent<TextInstructions> ().Open ();
							GameObject.Find ("GeneratePlanes").SetActive (true);
						}
					}
				}

			}

			//Prevent this from being used before the system kicks in.
			if (Input.GetMouseButtonDown (0)&&!Entrance) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				Debug.Log ("cast!");

				//we'll try to hit one of the plane collider gameobjects that were generated by the plugin
				//effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
				if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
					//we're going to get the position from the contact point
					//m_HitTransform.position = hit.point;
					Debug.Log (hit.transform.name);


					if (hit.transform.tag == "Person"&&!narrating) {
						Debug.Log ("narrate!");
						hit.transform.GetComponent<Narrate> ().Play ();
						NarratePause (hit.transform.GetComponent<AudioSource> ().clip.length);
					} 
						
					//Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));

					//and the rotation from the transform of the plane collider
					//m_HitTransform.rotation = hit.transform.rotation;
				}

				var touch = Input.GetTouch(0);

				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				ARPoint point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};

				// prioritize reults types
				ARHitTestResultType[] resultTypes = {
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
					// if you want to use infinite planes use this:
					//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
					ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
					ARHitTestResultType.ARHitTestResultTypeFeaturePoint
				}; 

				foreach (ARHitTestResultType resultType in resultTypes)
				{
					if (HitTestWithResultType (point, resultType))
					{
						return;
					}
				}



			}
		}

		IEnumerator BeginElysium()
		{
			yield return new WaitForSeconds(6.0f);
			Entrance = false;
			crowd.SetActive (false);
			Elysium.transform.position = center;
			Elysium.SetActive (true);
		}

		IEnumerator NarratePause(float length)
		{
			narrating = true;
			yield return new WaitForSeconds(length);
			narrating = false;
		}

		IEnumerator FadeDown(float wait) {
			yield return new WaitForSeconds(wait);
			for (float f = 1f; f > 0; f -= 0.01f) {
				Color c = fadeImage.color;
				c.a = f;
				fadeImage.color = c;
				yield return null;
			}
		}

		IEnumerator FadeUp(float wait) {
			yield return new WaitForSeconds(wait);
			for (float f = 0f; f < 1.01; f += 0.01f) {
				Color c = fadeImage.color;
				c.a = f;
				fadeImage.color = c;
				yield return null;
			}
		}
	}
}
