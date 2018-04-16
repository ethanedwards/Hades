using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonParticle : MonoBehaviour {

	bool ran;

	public ParticleSystem pointCloudParticlePrefab;
	public int maxPointsToShow;
	public float particleSize = 1.0f;
	private Vector3[] m_PointCloudData;
	private bool frameUpdated = false;
	public ParticleSystem evapParticlePrefab;
	private ParticleSystem currentPS;
	//private ParticleSystem.Particle [] particles;
	private Mesh mesh;
	private Transform trans;
	private Vector3[] vertices;
	int numParticles;
	private bool skel;
	bool deleted;
	ParticleSystem.Particle[] evap;
	// Use this for initialization
	void Start () {
		deleted = false;
		currentPS = Instantiate (pointCloudParticlePrefab);
		GetComponent<SkinnedMeshRenderer> ().sharedMaterial.color = new Color (0, 0, 0, 0);
		mesh = new Mesh ();//GetComponent<SkinnedMeshRenderer> ().sharedMesh;
		GetComponent<SkinnedMeshRenderer> ().BakeMesh (mesh);
		vertices = mesh.vertices;
		trans = this.transform;
		skel = true;
		ran = false;
	}




	
	// Update is called once per frame
	void Update () {
		/*
		if (Time.time > 0.5f&&!ran) {
			skel = false;
			SkelParticle ();
			ran = true;
			StartCoroutine (FadeDown (0));
		}
		*/

		if (skel) {
			
			SkelParticle ();
			//mesh.vertices = vertices;
			//mesh.RecalculateBounds();
		} else  if(!deleted){
			for(int i = 0; i < numParticles; i++){
				Vector3 offset = new Vector3 (Random.value, Random.value, Random.value);
				evap[i].position = evap[i].position + Vector3.up / 40.0f + offset/30.0f;
				evap[i].startColor = new Color (1.0f, 1.0f, 1.0f);
				evap[i].startSize = particleSize;
			}
			currentPS.SetParticles (evap, numParticles);

		}
	}

	void SkelParticle(){
		GetComponent<SkinnedMeshRenderer> ().BakeMesh (mesh);
		vertices = mesh.vertices;
		int i = 0;
		//while (i < vertices.Length) {
		//	vertices[i] += Vector3.up * Time.deltaTime;
		//	i++;
		//}
		numParticles = Mathf.Min (vertices.Length, maxPointsToShow);
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[numParticles];
		int index = 0;
		foreach (Vector3 currentPoint in vertices) {
			Vector3 offset = new Vector3 (Random.value, Random.value, Random.value);//new Vector3 (Mathf.PerlinNoise (currentPoint.x, Time.time), Mathf.PerlinNoise (currentPoint.y, Time.time), Mathf.PerlinNoise (currentPoint.z, Time.time));
			particles [index].position = trans.rotation * currentPoint + transform.position + offset / 200.0f;
			particles [index].startColor = new Color (1.0f, 1.0f, 1.0f);
			particles [index].startSize = particleSize;
			index++;
			if (index > numParticles) {
				break;
			}
		}
		currentPS.SetParticles (particles, numParticles);
		if (!skel) {
			evap = particles;
		}
	}

	public void Disable(){
		skel = false;
		SkelParticle ();
		ran = true;
		StartCoroutine (FadeDown (0));
		StartCoroutine (Delete (5.0f));

	}

	IEnumerator FadeDown(float wait) {
		yield return new WaitForSeconds(wait);
		float s = GetComponent<Renderer> ().material.GetFloat ("_Transparency");
		for (float f = s; f > 0; f -= 0.002f) {
			GetComponent<Renderer> ().material.SetFloat ("_Transparency", f);
			yield return null;
		}
	}

	IEnumerator Delete(float wait) {
		yield return new WaitForSeconds(wait);
		Destroy (currentPS);
		deleted = true;
	}

	void OnDisable(){
		Destroy (currentPS);
	}
}
