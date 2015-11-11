using UnityEngine;
using System.Collections;

public class TimeScale : MonoBehaviour {
	public GameObject Fwd, Bkw;
	public GameObject TimescaleIndicator;
	public bool FWD_A, BKW_A;
	public Texture2D FWD_N, FWD_O, FWD_D; 
	public Texture2D BKW_N, BKW_O, BKW_D;
	public float MaxTimeScale = 3f;
	public float MinTimeScale = 1f;
	public float CurTimeScale = 1;
	public float TimeScaleIncrement = 1f;
	private bool wasP;
	// Use this for initialization
	void Start () 
	{
		Time.timeScale = CurTimeScale;
		FWD_A = Fwd.GetComponent<MouseOver>().Active;
		BKW_A = Bkw.GetComponent<MouseOver>().Active;
		TimescaleIndicator.guiText.text = Time.timeScale.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PauseManager.inst.isPasued)
		{
			Time.timeScale = 1;
			wasP = true;
		}
		if(!PauseManager.inst.isPasued)
		{
			if(wasP)
			{
				Time.timeScale = CurTimeScale;
				wasP = false;
			}
		}
		//Forward
		FWD_A = Fwd.GetComponent<MouseOver>().Active;
		if(CurTimeScale >= MaxTimeScale)
		{
			Fwd.guiTexture.texture = FWD_D;
		}
		else
		{
			if(FWD_A)
			{
				Fwd.guiTexture.texture = FWD_O;
				if(Input.GetKeyUp(KeyCode.Mouse0) && CurTimeScale < MaxTimeScale)
				{
					CurTimeScale += TimeScaleIncrement;
					//CameraControls.inst.UpdatePref(CurTimeScale);
					Time.timeScale = CurTimeScale;
					TimescaleIndicator.guiText.text = Time.timeScale.ToString();
				}
			}
			else
			{
				Fwd.guiTexture.texture = FWD_N;
			}
		}
		//Backward
		BKW_A = Bkw.GetComponent<MouseOver>().Active;
		if(CurTimeScale <= MinTimeScale)
		{
			Bkw.guiTexture.texture = BKW_D;
		}else
		{
			if(BKW_A)
			{
				Bkw.guiTexture.texture = BKW_O;
				if(Input.GetKeyUp(KeyCode.Mouse0) && CurTimeScale > MinTimeScale)
				{
					CurTimeScale -= TimeScaleIncrement;
					//CameraControls.inst.UpdatePref(CurTimeScale);
					Time.timeScale = CurTimeScale;
					TimescaleIndicator.guiText.text = Time.timeScale.ToString();
				}
			}
			else
			{
				Bkw.guiTexture.texture = BKW_N;
			}
		}
		if(CurTimeScale > MaxTimeScale)
		{
			CurTimeScale = MaxTimeScale;
			Time.timeScale = CurTimeScale;
			TimescaleIndicator.guiText.text = Time.timeScale.ToString();
		}
		if(CurTimeScale < MinTimeScale)
		{
			CurTimeScale = MinTimeScale;
			Time.timeScale = CurTimeScale;
			TimescaleIndicator.guiText.text = Time.timeScale.ToString();
		}
		
	}
}
