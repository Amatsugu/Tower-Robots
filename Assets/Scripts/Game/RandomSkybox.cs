using UnityEngine;
using System.Collections;

public class RandomSkybox : MonoBehaviour {
	public Material[] Skyboxes;
	public bool ResetSky;
	int index;
	// Use this for initialization
	void Start () 
	{
		//gameObject.AddComponent("SkyBox");
		index = Random.Range(1, Skyboxes.Length -1);
		gameObject.GetComponent<Skybox>().material = Skyboxes[index];
	}
	void Update()
	{
		if(ResetSky)
		{
			index = Random.Range(1, Skyboxes.Length -1);
			gameObject.GetComponent<Skybox>().material = Skyboxes[index];
			ResetSky = false;
		}
	}
}
