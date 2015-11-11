using UnityEngine;
using System.Collections;

public class Options_Back : MonoBehaviour 
{
	//Vars
	public Material GUI_N;
	public Material GUI_O;
	public static bool Active = false;
	public Transform Menu;
	public GameObject Cam;
	private SmoothCam s;
	//Functions
	void Awake()
	{
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
				Back();
				Debug.Log("Options");
			}
		}
	}
	void Back()
	{
		s._SetTarget(Menu);
	}
}
