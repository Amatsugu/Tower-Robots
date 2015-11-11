using UnityEngine;
using System.Collections;

public class Animation_Loop : MonoBehaviour 
{
	private GameObject PauseM;
	private PauseManager PM;
	void Start()
	{
		if(GameObject.Find("PauseMenu"))
		{
			PauseM = GameObject.Find("PauseMenu");
			PM = PauseM.GetComponent<PauseManager>();
		}
	}
	//Functions
	void Update () {
		if(GameObject.Find("PauseMenu"))
		{
			if(!PM.isPasued)
				animation.Play();
			if(PM.isPasued)
				animation.Stop();
		}else
		{
			if(!animation.isPlaying)
				animation.Play();
		}
	}
}
