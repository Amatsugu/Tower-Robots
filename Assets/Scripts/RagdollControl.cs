using UnityEngine;
using System.Collections;

public class RagdollControl : MonoBehaviour 
{
	private bool r;
	private Vector3 PrePauseVel;
	private Vector3 PrePauseAngVel;
	public void SetRagdollInfo(bool enemy, Color color)
	{
		gameObject.renderer.material.color = color;
		if(enemy)
		{
			gameObject.layer = 9;
		}else
		{
			gameObject.layer = 8;
		}
	}
	void FixedUpdate()
	{
		if(PauseManager.inst.isPasued)
		{
			if(!r)
			{
				PrePauseVel = rigidbody.velocity;
				PrePauseAngVel = rigidbody.angularVelocity;
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.useGravity = false;
				r = true;
			}
		}
		if(!PauseManager.inst.isPasued)
		{
			if(r)
			{
				rigidbody.velocity = PrePauseVel;
				rigidbody.angularVelocity = PrePauseAngVel;
				rigidbody.useGravity = true;
				r = false;
			}
		}
	}
}
