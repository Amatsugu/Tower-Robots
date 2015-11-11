using UnityEngine;
using System.Collections;

public class Options_GamePlay : MonoBehaviour 
{
	//Vars
	public Material GUI_N;
	public Material GUI_O;
	public static bool Active = false;
	public float width = 500f;
	public float height = 400f;
	public GUISkin Skin;
	public GUISkin Skin2;
	public GameObject CounterS;
	public static Options_GamePlay inst;
	//Private
	private bool GamePlayScreen = false;
	private string FPSTxt = "Off";
	private FPSCounter s;
	private bool userNameRecieved = false;
	//Prefs
	string PlayerName;
	float XSen;
	float YSen;
	float ZSen;
	int FPS;
	float SenMAX = 10;
	float SenMIN = 1;
	float ZSenMin = 100;
	float ZSenMax = 200;
	//Default Prefs
	string D_PlayerName = "Player";
	float D_XSen = 8;
	float D_YSen = 8;
	float D_ZSen = 150;
	int D_FPS = 0;
	//Functions
	void Start()
	{
		s = CounterS.GetComponent<FPSCounter>();
		inst = this;
	}
	public static void SetPlayerName(string player)
	{
		inst.PlayerName = player;
		PlayerPrefs.SetString("PlayerName", inst.PlayerName);
		DebugConsole.Log("Set playername to: " + player);
	}
	void Awake()	
	{
		if(PlayerPrefs.GetInt("First_G") == 0)
		{
			//Set Defailts on first Time
			PlayerName = D_PlayerName;
			XSen = D_XSen;
			YSen = D_YSen;
			FPS = D_FPS;
			PlayerPrefs.SetInt("First_G", 1);
//			if(KongregateAPI.isKongregate)
//				PlayerName = KongregateAPI.username;
//			else
				PlayerPrefs.SetString("PlayerName", PlayerName);
			PlayerPrefs.SetFloat("XSen", XSen);
			PlayerPrefs.SetFloat("YSen", YSen);
			PlayerPrefs.SetInt("FPSCounter", FPS);
			PlayerPrefs.SetFloat("ZSen", D_ZSen);
			
		}else
		{
			//Load Prefs
//			if(KongregateAPI.isKongregate)
//				PlayerName = KongregateAPI.username;
//			else
				PlayerName = PlayerPrefs.GetString("PlayerName");
			XSen = PlayerPrefs.GetFloat("XSen");
			YSen = PlayerPrefs.GetFloat("YSen");
			ZSen = PlayerPrefs.GetFloat("ZSen", D_ZSen);
			FPS = PlayerPrefs.GetInt("FPSCounter", 0);
			s = CounterS.GetComponent<FPSCounter>();
		}
		//Apply Loaded Prefs
		ApplyLoaded();
	}
	void ApplyLoaded()
	{
		if(FPS == 0)
		{
			s.Active = false;
		}else if(FPS == 1)
		{
			s.Active = true;
		}
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
//		if(KongregateAPI.isKongregate && !userNameRecieved)
//		{
//			PlayerName = KongregateAPI.username;
//			userNameRecieved = true;
//		}
		if(Active)
		{
			if(Input.GetMouseButtonUp(0))
			{
				GamePlayScreen = true;
				Debug.Log("Options");
			}
		}
		if(Input.GetKeyUp(KeyCode.Escape))
			GamePlayScreen = false;
		//FPS Counter
		if(Input.GetKeyUp(KeyCode.F3) && s.Active == true)
		{
			FPS = 1;
			PlayerPrefs.SetInt("FPSCounter", FPS);
		}
		if(Input.GetKeyUp(KeyCode.F3) && s.Active == false)
		{
			FPS = 0;
			PlayerPrefs.SetInt("FPSCounter", FPS);
		}
		if(FPS == 0)
			FPSTxt = "Off";
		if(FPS == 1)
			FPSTxt = "On";
	}
	void OnGUI()
	{
		GUI.skin = Skin;
		if(GamePlayScreen)
		{
			GUI.Window(0, new Rect((Screen.width/2)-(width/2), (Screen.height/2)-(height/2), width, height), GamePlayWindow, "GamePlay Settings");
		}
	}
	void GamePlayWindow(int windowID)
	{
		GUI.skin = Skin;
		//PlayerName
		GUI.Label(new Rect((width/2) - 100, 40, 200, 20), "Player Name");
		PlayerName = GUI.TextField(new Rect((width/2) - 100, 60, 200, 20), PlayerName, 16);
		//X Sensitivity
		GUI.Label(new Rect((width/2) - 100, 80, 200, 20), "X Sensitivity: "+ (int)XSen);
		XSen = GUI.HorizontalSlider(new Rect((width/2) - 100, 100, 200, 15), XSen, SenMIN, SenMAX);
		//Y Sensitivity
		GUI.Label(new Rect((width/2) - 100, 120, 200, 20), "Y Sensitivity: "+ (int)YSen);
		YSen = GUI.HorizontalSlider(new Rect((width/2) - 100, 140, 200, 15), YSen, SenMIN, SenMAX);
		//FPS Counter
		GUI.Label(new Rect((width/2) - 200, 160, 400, 20), "FPS Counter(f3)");
		if(GUI.Button(new Rect((width/2) - 100, 180, 200, 20), FPSTxt))
		{
			if(FPS >= 1)
			{
				FPS = 0;
			}else if(FPS <=0)
			{
				FPS = 1;
			}
		}
		//Zoom Sensitivity
		GUI.Label(new Rect((width/2) - 100, 200, 200, 20), "Zoom Sensitivity: "+ (int)ZSen);
		ZSen = GUI.HorizontalSlider(new Rect((width/2) - 100, 220, 200, 15), ZSen, ZSenMin, ZSenMax);
		//Close Buttons
		GUI.skin = Skin2;
		if(GUI.Button(new Rect((width/2) - 147.5f, height - 50, 95, 20), "Apply"))
		{
			//GamePlayScreen = false;
			Apply();
		}
		if(GUI.Button(new Rect((width/2) - 47.5f, height - 50, 95, 20), "Done"))
		{
			GamePlayScreen = false;
			//Restore();
		}
		if(GUI.Button(new Rect((width/2) + 52.5f, height - 50, 95, 20), "Cancel"))
		{
			GamePlayScreen = false;
			Restore();
		}
	}
	void Apply()
	{
		PlayerPrefs.SetString("PlayerName", PlayerName);
		PlayerPrefs.SetFloat("XSen", XSen);
		PlayerPrefs.SetFloat("YSen", YSen);
		PlayerPrefs.SetFloat("ZSen", ZSen);
		PlayerPrefs.SetInt("FPSCounter", FPS);
		if(FPS == 0)
		{
			s.Active = false;
		}else if(FPS == 1)
		{
			s.Active = true;
		}
		GameObject cam = GameObject.Find("Cam");
		if(cam != null)
		{
			cam.GetComponent<CameraControls>().UpdatePrefs();
		}
	}
	void Restore()
	{
		PlayerName = PlayerPrefs.GetString("PlayerName");
		XSen = PlayerPrefs.GetFloat("XSen");
		YSen = PlayerPrefs.GetFloat("YSen");
		ZSen = PlayerPrefs.GetFloat("ZSen");
		FPS = PlayerPrefs.GetInt("FPSCounter");
	}
}