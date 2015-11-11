using UnityEngine;
using System.Collections;

public class Menu_Quit : MonoBehaviour 
{
	//Vars
	public Material GUI_N;
	public Material GUI_O;
	public static bool Active = false;
	public bool Main;
	private GameObject Cam;
	private SmoothCam s;
	//Functions
	void Awake()
	{
		Active = false;
		Cam = GameObject.Find("MCam");
		s = Cam.GetComponent<SmoothCam>();
	}
	void OnMouseEnter()
	{
		renderer.material = GUI_O;
		Active = true;
	}
	void OnMouseExit()
	{
		renderer.material = GUI_N;
		Active = false;
	}
	
	void Update()
	{
		if(Active)
		{
			if(Input.GetMouseButtonUp(0))
			{
				//animation.Play("Bounce");
				if(Main)
					Application.Quit();
				else if(!Main)
				{
					s.transform.position = Vector3.zero;
					Application.LoadLevel(0);
				}
			}
		}
	}
}
