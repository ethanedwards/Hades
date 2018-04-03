using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonParticle : MonoBehaviour {
	public ParticleSystem pointCloudParticlePrefab;
	public int maxPointsToShow;
	public float particleSize = 1.0f;
	private Vector3[] m_PointCloudData;
	private bool frameUpdated = false;
	private ParticleSystem currentPS;
	private ParticleSystem.Particle [] particles;
	private Mesh mesh;
	private Transform trans;
	private Vector3[] vertices;
	// Use this for initialization
	void Start () {
		currentPS = Instantiate (pointCloudParticlePrefab);
		GetComponent<SkinnedMeshRenderer> ().sharedMaterial.color = new Color (0, 0, 0, 0);
		mesh = new Mesh ();//GetComponent<SkinnedMeshRenderer> ().sharedMesh;
		GetComponent<SkinnedMeshRenderer> ().BakeMesh (mesh);
		vertices = mesh.vertices;
		trans = this.transform;
	}




	
	// Update is called once per frame
	void Update () {
		GetComponent<SkinnedMeshRenderer> ().BakeMesh (mesh);
		vertices = mesh.vertices;
		int i = 0;
		//while (i < vertices.Length) {
		//	vertices[i] += Vector3.up * Time.deltaTime;
		//	i++;
		//}
		int numParticles = Mathf.Min (vertices.Length, maxPointsToShow);
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[numParticles];
		int index = 0;
		foreach (Vector3 currentPoint in vertices){
			Vector3 offset = new Vector3 (Random.value, Random.value, Random.value);//new Vector3 (Mathf.PerlinNoise (currentPoint.x, Time.time), Mathf.PerlinNoise (currentPoint.y, Time.time), Mathf.PerlinNoise (currentPoint.z, Time.time));
			particles [index].position = trans.rotation*currentPoint + transform.position + offset/200.0f;
			particles [index].startColor = new Color (1.0f, 1.0f, 1.0f);
			particles [index].startSize = particleSize;
			index++;
			if (index > numParticles) {
				break;
			}
		}
		currentPS.SetParticles (particles, numParticles);

		//mesh.vertices = vertices;
		//mesh.RecalculateBounds();
	}

	public void Disable(){
		Destroy (currentPS);
	}
}
