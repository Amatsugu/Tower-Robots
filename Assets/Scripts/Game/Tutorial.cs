using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	public string TutorialName;
	public bool useTextures;
	public Texture2D[] TutorialTextures;
	public bool useGameobjects;
	public GameObject[] TurorialGameobjects;
	public int procedOnClickAt;
	public bool Pause;
	public int unPauseAt;
	public int rePauseAt;
	public bool SpawnNew;
	public GameObject newTutorial;
	
	//private
	private int curTile = 0;
	private int maxTile;
	private bool Do;
	private bool wasP;
	// Use this for initialization
	void Awake () 
	{
		if(Pause)
		{
			if(PauseManager.inst == null)
			{
				PauseManager.inst = GameObject.Find("PauseMenu").GetComponent<PauseManager>();
			}
		}
		if(useTextures && useGameobjects)
		{
			Debug.LogError("You are not allowed to use gameObjects and Textures, Aborting.");
			Destroy(gameObject);
			return;
		}
		if(TutorialTextures.Length == 0 && TurorialGameobjects.Length == 0)
		{
			Debug.LogError("You must fill in gameObjects or Textures, Aborting.");
			Destroy(gameObject);
			return;
		}
		if(Pause)
		{
			if(PauseManager.inst != null)
			{
				PauseManager.inst.isPasued = true;
				PauseManager.inst.CanPause = false;
			}
		}
		if(useTextures)
		{
			if(gameObject.guiTexture == null)
			{
				gameObject.AddComponent<GUITexture>();
			}
			maxTile = TutorialTextures.Length-1;
		}
		if(useGameobjects)
		{
			foreach(GameObject g in TurorialGameobjects)
			{
				g.SetActiveRecursively(false);
			}
			maxTile = TurorialGameobjects.Length-1;
		}
		if(PlayerPrefs.GetInt(TutorialName, 0) == 0)
		{
			Do = true;
		}else
		{
			if(Pause)
			{
				PauseManager.inst.isPasued = false;
				PauseManager.inst.CanPause = true;
			}
			Debug.Log("Not first time.");
			if(SpawnNew)
				Instantiate(newTutorial, Vector3.zero, Quaternion.identity);
			Destroy(gameObject);
		}
		guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Do)
		{
			if(curTile > maxTile)
			{
				PlayerPrefs.SetInt(TutorialName, 1);
				if(Pause)
				{
					PauseManager.inst.isPasued = false;
					PauseManager.inst.CanPause = true;
				}
				if(SpawnNew)
					Instantiate(newTutorial, Vector3.zero, Quaternion.identity);
				Destroy(gameObject);
				return;
			}
			if(Pause && !wasP)
			{
				if(PauseManager.inst != null)
				{
					PauseManager.inst.isPasued = true;
					PauseManager.inst.CanPause = false;
					wasP = true;
				}
			}
			if(unPauseAt != 0)
			{
				if(curTile >= unPauseAt)
				{
					PauseManager.inst.isPasued = false;
					PauseManager.inst.CanPause = false;
				}
				if(rePauseAt > unPauseAt && curTile >= rePauseAt)
				{
					PauseManager.inst.isPasued = true;
					PauseManager.inst.CanPause = false;
				}
			}
			
			if(Input.GetKeyUp(KeyCode.Space))
			{
				curTile++;
			}
			if(Input.GetKeyUp(KeyCode.Mouse0) && procedOnClickAt == curTile)
				curTile++;
			if(useTextures && curTile <= maxTile)
			{
				guiTexture.texture = TutorialTextures[curTile];
			}
			if(useGameobjects && curTile <= maxTile)
			{
				if(curTile >= 1)
				{
					TurorialGameobjects[curTile-1].SetActiveRecursively(false);
				}
				TurorialGameobjects[curTile].SetActiveRecursively(true);
			}
		}else
		{
			if(SpawnNew)
				Instantiate(newTutorial, Vector3.zero, Quaternion.identity);
			return;
		}
	}
	void OnDestroy()
	{
		if(Pause)
		{
			PauseManager.inst.isPasued = false;
			PauseManager.inst.CanPause = true;
		}
	}
}
