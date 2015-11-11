using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {
	public bool Active = false;
	public bool States;
	public Material Normal;
	public Material Over;
	// Use this for initialization
	void OnMouseEnter()
	{
		Active = true;
		if(States)
			gameObject.renderer.material = Over;
	}
	void OnMouseExit()
	{
		Active = false;
		if(States)
			gameObject.renderer.material = Normal;
	}
}
