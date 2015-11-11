using UnityEngine;
using System.Collections;

public class MenuGoTo : MonoBehaviour 
{

	// Use this for initialization
	public Material GUI_N;
	public Material GUI_O;
	public static bool Active = false;
	public Transform Target;

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
				SmoothCam.Instance._SetTarget(Target);
			}
		}
	}
}
