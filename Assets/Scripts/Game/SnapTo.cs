using UnityEngine;
using System.Collections;

public class SnapTo : MonoBehaviour {
	public bool SnapRotation;
	public Vector3 Rotation;
	public bool SnapPosition;
	public Vector3 Position;
	// Use this for initialization
	void Start () 
	{
		if(SnapRotation)
		{
			transform.Rotate(Rotation);
		}
		if(SnapPosition)
		{
			transform.position = Position;
		}
		Destroy(this);
	}

	
}
