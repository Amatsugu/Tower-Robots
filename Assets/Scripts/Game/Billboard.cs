using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
	public bool useMainCam;
	public Transform Target;
	// Use this for initialization
	void Start () 
	{
		if(useMainCam)
			Target = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = Target.rotation;
	}
}
