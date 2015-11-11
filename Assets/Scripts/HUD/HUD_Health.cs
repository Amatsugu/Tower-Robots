using UnityEngine;
using System.Collections;

public class HUD_Health : MonoBehaviour {
	public float Health = 200;
	public GameObject HealthTxt;
	public GameObject DefeatSplash;
	//Private
	public void RemoveHealth(float h)
	{
		Health -= h;
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(!PauseManager.inst.isPasued)
		{
			Rect Pos = new Rect((Health-200), gameObject.guiTexture.pixelInset.y, gameObject.guiTexture.pixelInset.width, gameObject.guiTexture.pixelInset.height);
			HealthTxt.guiText.text = (int)Health + "%";
			gameObject.guiTexture.pixelInset = Pos;
			float color;
			color = Health/200;
			if(Health > 50)
			{
				gameObject.guiTexture.color = new Color(.5f, color*.5f, color*.5f, .5f);
				HealthTxt.guiText.font.material.color = new Color(1, color, color);
			}else if(Health <= 50)
			{
				animation.Play();
				
			}
		}
		if(Health <= 0)
		{
			Health = 0;
			PauseManager.inst.isPasued = true;
			PauseManager.inst.CanPause = false;
			Screen.lockCursor = false;
			if(!GameObject.Find("DefeatSplash"))
			{
				GameObject Clone = Instantiate(DefeatSplash, new Vector3(.5f, .5f, 9999), Quaternion.identity) as GameObject;
				Clone.name = "DefeatSplash";
			}
			if(Input.GetKeyUp(KeyCode.Space))
			{
				if(StatsManager.AddLoss())
					Application.LoadLevel(0);
			}
		}
	}
}
