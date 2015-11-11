using UnityEngine;
using System.Collections;

public class ParticleControl : MonoBehaviour {
	private bool wasP;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PauseManager.inst.isPasued)
		{
			if(!wasP)
			{
				wasP = true;
				particleSystem.Pause();
			}
		}
		if(!PauseManager.inst.isPasued)
		{
			if(wasP)
			{
				wasP = false;
				particleSystem.Play();
			}
		}
	}
}
