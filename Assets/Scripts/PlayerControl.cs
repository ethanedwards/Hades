using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class PlayerControl : MonoBehaviour {

		public GameObject sceneRoot;
		Vector3 center;
		public GameObject flame;
		public GameObject rotator;
		public GameObject crowd;
		public GameObject bloodStain;
		public GameObject Elysium;
		public GameObject blue;
		public GameObject red;
		public GameObject yellow;
		public GameObject white;
		public GameObject particles;
		public float maxRayDistance = 10.0f;
		public LayerMask collisionLayer = 1 << 10;
		bool narrating;
		bool placed;
		bool approached;
		bool pickedUp;
		bool spawned;
		bool fired;
		int talkedTo;
		int level;
		bool firstshade;
		public bool Entrance = false;
		public GameObject text;
		public GameObject RTcmix;
		public Image fadeImage;


		public bool testingScene;
		public AudioClip testClip;

		// Use this for initialization
		void Start () {
			center = new Vector3(0, -0.5f, 0);
			narrating = false;
			placed = false;
			approached = false;
			fired = false;
			spawned = false;
			firstshade = false;
			talkedTo = 0;
			level = 0;
		}

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					
					Debug.Log ("Got hit!");
					Debug.Log("Hitpos " + UnityARMatrixOps.GetPosition (hitResult.worldTransform));
					if (!placed) {
						//sceneRoot.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
						//sceneRoot.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
						//sceneRoot.SetActive (true);
						Debug.Log ("placed");
						TransitionEntrance (UnityARMatrixOps.GetPosition (hitResult.worldTransform));

						//placed = true;
						return true;
					}
				}
			}
			return false;
		}
		
		// Update is called once per frame
		void Update () {
			if (Entrance) {
				if (Time.timeSinceLevelLoad > 9.5f&&!spawned) {
					Vector3 branchPos = transform.position - Camera.main.transform.forward*4;
					//branchPos.y = 0;
					sceneRoot.transform.position = branchPos;
					sceneRoot.SetActive (true);
					spawned = true;
					rotator.transform.position = transform.position;
					Vector3 flamePos = transform.position + Camera.main.transform.forward*4;
					//flamePos.y = 0;
					flame.SetActive (true);
					flame.transform.position = flamePos;
				}
				//Do the rotation thing
				if (spawned&&!fired) {
					if (Vector3.Distance (flame.transform.position, sceneRoot.transform.position) < 0.5f||Time.timeSinceLevelLoad>60.0f) {
						flame.transform.position = sceneRoot.transform.position;
						flame.transform.parent = sceneRoot.transform;
						fired = true;
					} else {
						ParticleSystem.MainModule main = flame.GetComponent<ParticleSystem> ().main;
						Color col = main.startColor.color;
						if (col.a < 1.0f) {
							col.a = col.a + .005f;
							main.startColor = col;
							rotator.transform.position = transform.position;
							Quaternion rot = rotator.transform.rotation;
							//rot.eulerAngles = new Vector3 (0, transform.rotation.eulerAngles.y, 0);
							//Debug.Log (rot.eulerAngles);
							rotator.transform.rotation = transform.rotation;
							//Debug.Log (rotator.transform.rotation.eulerAngles);
							Vector3 flamePos = transform.position + rotator.transform.forward * 4;
							flame.transform.position = flamePos;
							Vector3 branchPos = transform.position - Camera.main.transform.forward*4;
							sceneRoot.transform.position = branchPos;
						} else {
							flame.GetComponent<Muse> ().Play ();
							Vector3 flamePos = transform.position + rotator.transform.forward * 4;
							rotator.transform.Rotate (new Vector3 (0, 1, 0) * Time.deltaTime * 7f);
							flame.transform.position = flamePos;
						}
					}
				}

				if (pickedUp && !placed) {
					//Flying around
					Vector3 goal = transform.position + Camera.main.transform.forward-Camera.main.transform.up/4;
					Vector3 start = sceneRoot.transform.position;
					if (Vector3.Distance (start, goal) > 0.1f) {
						sceneRoot.transform.position = start + (goal - start).normalized / 40.0f;
					}

					if (Input.GetMouseButtonDown (0)) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
							Debug.Log("position " + hit.transform.position);
							#if UNITY_EDITOR
							TransitionEntrance (hit.transform.position);
							#endif

						}
					}



					if (Input.GetMouseButtonDown (0)) {
						Debug.Log ("input");
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

						foreach (ARHitTestResultType resultType in resultTypes) {
							if (HitTestWithResultType (point, resultType)) {
								if (pickedUp) {
									Debug.Log ("Done!");
								}
								return;
							}
						}
					}

				}



				//if (Vector3.Distance (transform.position, sceneRoot.transform.position) < 1.0 && !approached) {
				if(sceneRoot.GetComponent<Renderer>().isVisible&&!approached&&Time.timeSinceLevelLoad>15.0f){
					text.GetComponent<TextInstructions> ().Next ();
					approached = true;
				}
				if (Input.GetMouseButtonDown (0) && pickedUp == false && approached && !placed && fired) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
						Debug.Log ("pickup");
						if (approached && hit.transform.tag == "Branch") {
							Debug.Log (pickedUp);
							pickedUp = true;
							placed = false;
							GetComponent<UnityARVideo> ().FadeBW ();
							particles.SetActive (true);
							text.GetComponent<TextInstructions> ().Open ();
							//GameObject.Find ("GeneratePlanes").SetActive (true);
						}
					}
				}

			}

			//Debug function
			if(Input.GetKeyDown("space")){
				if (Entrance) {
					TransitionEntrance (center);
				} else {
					ChangeLevel ();
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
						if (!firstshade) {
							text.GetComponent<TextInstructions> ().Fade ();
							firstshade = true;
						}
						if (testingScene) {
							hit.transform.GetComponent<AudioSource> ().clip = testClip;
						}
						StartCoroutine(NarratePause (hit.transform.GetComponent<AudioSource> ().clip.length));
						Debug.Log ("talkedTo: " + talkedTo);
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

		void ChangeLevel(){
			StartCoroutine (FadeChangeLevel ());
		}

		IEnumerator FadeChangeLevel()
		{
			talkedTo = 0;
			StartCoroutine(FadeUp(1.0f));
			yield return new WaitForSeconds(5.5f);

			Debug.Log ("Switch: " + level);
			switch(level){
			case 0:
				Elysium.SetActive (false);
				blue.SetActive (true);
				RTcmix.GetComponent<DroneMusic> ().ChangeScene (1);
				break;

			case 1:
				blue.SetActive (false);
				red.SetActive (true);
				RTcmix.GetComponent<DroneMusic> ().ChangeScene (2);
				break;
			case 2:
				red.SetActive (false);
				yellow.SetActive (true);
				RTcmix.GetComponent<DroneMusic> ().ChangeScene (3);
				break;
			case 3:
				yellow.SetActive (false);
				white.SetActive (true);
				RTcmix.GetComponent<DroneMusic> ().ChangeScene (4);
				break;
			default:
				break;
			}



			level++;
			//Increase level beforehand because green change comes first
			GetComponent<UnityARVideo> ().ChangeColor (level);
			StartCoroutine (FadeDown (0.0f));

		}

		IEnumerator BeginElysium()
		{
			yield return new WaitForSeconds(8.0f);
			Debug.Log ("Yo ELysium began");
			Entrance = false;
			//sceneRoot.SetActive (false);
			RTcmix.SetActive(true);
			crowd.SetActive (false);
			bloodStain.SetActive (false);
			PersonParticle[] parts = crowd.GetComponentsInChildren<PersonParticle> ();
			foreach (PersonParticle part in parts) {
				part.Disable ();
			}
			Elysium.transform.position = center;
			Elysium.SetActive (true);
			text.GetComponent<TextInstructions> ().Elysium ();
			GetComponent<UnityARVideo> ().ChangeColor (level);
		}

		IEnumerator NarratePause(float length)
		{
			Debug.Log ("length" + length);
			narrating = true;
			yield return new WaitForSeconds(length);
			Debug.Log ("narrated");
			narrating = false;
			talkedTo++;
			GetComponentInChildren<DroneMusic> ().talkedTo = talkedTo;
			if (!narrating && (talkedTo >= 4&&(level==0||level==3)|| talkedTo >= 3&&(level==1||level==2))) {
				Debug.Log ("Changed");
				ChangeLevel ();

			}

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

		void TransitionEntrance(Vector3 pos){

			Debug.Log ("PLACED");
			Debug.Log ("pos " + pos);
			center = pos;
			sceneRoot.transform.position = center;
			pickedUp = false;
			placed = true;
			Debug.Log ("Centered?");
			//GameObject.Find ("GeneratePlanes").SetActive (false);
			Debug.Log ("crowd");
			crowd.transform.position = center;

			bloodStain.GetComponent<Renderer> ().material.SetFloat ("_Tim", Time.time);
			bloodStain.transform.position = center;
			bloodStain.SetActive (true);
			Debug.Log ("tim " + Time.time);

			crowd.SetActive (true);

			StartCoroutine(BeginElysium ());
			Debug.Log ("fade up");
			StartCoroutine (FadeUp (4.0f));
			Debug.Log ("fade down");
			StartCoroutine (FadeDown (9.0f));
		}
	}
}
