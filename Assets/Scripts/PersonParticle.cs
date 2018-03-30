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
	// Use this for initialization
	void Start () {
		currentPS = Instantiate (pointCloudParticlePrefab);
		GetComponent<SkinnedMeshRenderer> ().sharedMaterial.color = new Color (0, 0, 0, 0);
	}




	
	// Update is called once per frame
	void Update () {
		Mesh mesh = new Mesh ();//GetComponent<SkinnedMeshRenderer> ().sharedMesh;
		GetComponent<SkinnedMeshRenderer> ().BakeMesh (mesh);
		Vector3[] vertices = mesh.vertices;
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
			particles [index].position = Quaternion.Euler(0, 270, 0)*currentPoint + transform.position;// + offset/20.0f;
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
}
