using UnityEngine;
using System.Collections;

public class DOWN : MonoBehaviour {

	public bool Active = false;
	public bool Jump = false;
	private GameObject PauseManager;
	private PauseManager PM;
	void Start()
	{
		PauseManager = GameObject.Find("PauseMenu");
		PM = PauseManager.GetComponent<PauseManager>();
	}
	
	void OnMouseEnter()
	{
		if(!PM.isPasued)
		{
			gameObject.guiTexture.color = new Color(gameObject.guiTexture.color.r, gameObject.guiTexture.color.g, gameObject.guiTexture.color.b, .4f);
			Active = true;
		}
	}
	void OnMouseExit()
	{
		if(!PM.isPasued)
		{
			gameObject.guiTexture.color = new Color(gameObject.guiTexture.color.r, gameObject.guiTexture.color.g, gameObject.guiTexture.color.b, .1f);
			Active = false;
		}
	}
	
	void Update()
	{
		if(Active)
		{
			if(Input.GetMouseButton(0))
			{
				Jump = true;
				gameObject.guiTexture.color = new Color(gameObject.guiTexture.color.r, gameObject.guiTexture.color.g, gameObject.guiTexture.color.b, .45f);
			}else
			{
				Jump = false;
				gameObject.guiTexture.color = new Color(gameObject.guiTexture.color.r, gameObject.guiTexture.color.g, gameObject.guiTexture.color.b, .4f);
			}
		}else
			Jump = false;
	}
}
