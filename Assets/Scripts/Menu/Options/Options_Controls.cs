using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Options_Controls : MonoBehaviour 
{
	//Vars
	public Material GUI_N;
	public Material GUI_O;
	public GUISkin Skin;
	public GUISkin Skin2;
	public float width = 500f;
	public float height = 400f;
	public float Swidth = 500f;
	public float Sheight = 400f;
	public static bool Active = false;
	private bool ControlsScreen = false;
	private Vector2 scroll = Vector2.zero;
	//Config
	private int OptionsCount = 9;
	private List<int> ForbiddenKeys = new List<int>();
	private List<int> SetKeys = new List<int>();
	private List<string> KeyTexts = new List<string>();
	private List<int> DefaultKeys = new List<int>();
	private int ListiningTo = -1;
	private int curLoading = 0;
	private bool isLoaded = false;
	private int curApplying = 0;
	private bool isApplied = false;
	private int curRestoring = 0;
	private bool isRestored = false;
	//Functions
	void Awake()
	{
		KeyTexts.Clear();
		SetKeys.Clear();
		SetDefaults();
		Load();
		Apply();
		AddForbidden(KeyCode.Escape);
		AddForbidden(KeyCode.CapsLock);
		AddForbidden(KeyCode.F1);
		AddForbidden(KeyCode.F2);
		AddForbidden(KeyCode.F3);
		AddForbidden(KeyCode.F4);
		AddForbidden(KeyCode.F5);
		AddForbidden(KeyCode.F6);
		AddForbidden(KeyCode.F7);
		AddForbidden(KeyCode.F8);
		AddForbidden(KeyCode.F9);
		AddForbidden(KeyCode.F10);
		AddForbidden(KeyCode.F12);
		AddForbidden(KeyCode.None);
	}
	void SetDefaults()
	{
		DefaultKeys.Add((int)KeyCode.Q);
		DefaultKeys.Add((int)KeyCode.E);
		DefaultKeys.Add((int)KeyCode.W);
		DefaultKeys.Add((int)KeyCode.S);
		DefaultKeys.Add((int)KeyCode.A);
		DefaultKeys.Add((int)KeyCode.D);
		DefaultKeys.Add((int)KeyCode.Mouse1);
		DefaultKeys.Add((int)KeyCode.Mouse0);
		DefaultKeys.Add((int)KeyCode.B);
		DefaultKeys.Add((int)KeyCode.Mouse1);
	}
	void AddForbidden(KeyCode key)
	{
		if(!ForbiddenKeys.Contains((int)key))
		{
			ForbiddenKeys.Add((int)key);
		}else
		{
			Debug.LogWarning("Key: " + key + " is already a forbidden key, and was not added.");
			DebugConsole.LogWarning("Key: " + key + " is already a forbidden key, and was not added.");
		}
	}
	void Load()
	{
		if(isLoaded)
			curLoading = 0;
		while(curLoading <= OptionsCount)
		{
			SetKeys.Add(PlayerPrefs.GetInt(PrefsID(curLoading), DefaultKeys[curLoading]));
			KeyTexts.Add(KeyPharser((KeyCode)SetKeys[curLoading]));
			curLoading++;
			if(curLoading == OptionsCount)
				isLoaded = true;
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
	void Listener()
	{
		var curEvent = Event.current;
		if(ListiningTo != -1 && ListiningTo >= 0)
		{
			if(curEvent.keyCode != KeyCode.None || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2))
			{
				if(KeyIsValid(curEvent.keyCode))
				{
					SetKeys[ListiningTo] = (int)curEvent.keyCode;
					KeyTexts[ListiningTo] = KeyPharser(curEvent.keyCode);
					Screen.lockCursor = false;
					ListiningTo = -1;
				}else if(Input.GetKeyDown(KeyCode.Mouse0))
				{
					SetKeys[ListiningTo] = (int)KeyCode.Mouse0;
					KeyTexts[ListiningTo] = KeyPharser(KeyCode.Mouse0);
					Screen.lockCursor = false;
					ListiningTo = -1;
				}else if(Input.GetKeyDown(KeyCode.Mouse1))
				{
					SetKeys[ListiningTo] = (int)KeyCode.Mouse1;
					KeyTexts[ListiningTo] = KeyPharser(KeyCode.Mouse1);
					Screen.lockCursor = false;
					ListiningTo = -1;
				}else if(Input.GetKeyDown(KeyCode.Mouse2))
				{
					SetKeys[ListiningTo] = (int)KeyCode.Mouse2;
					KeyTexts[ListiningTo] = KeyPharser(KeyCode.Mouse2);
					Screen.lockCursor = false;
					ListiningTo = -1;
				}else
				{
					SetKeys[ListiningTo] = PlayerPrefs.GetInt(PrefsID(ListiningTo), DefaultKeys[ListiningTo]);
					KeyTexts[ListiningTo] = KeyPharser((KeyCode)SetKeys[ListiningTo]);
					Screen.lockCursor = false;
					ListiningTo = -1;
				}
			}
		}else
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				ControlsScreen = false;
				Restore();
			}
		}
		if(Active)
		{
			if(Input.GetMouseButtonUp(0))
			{
				ControlsScreen = true;
				//Debug.Log("Options");
			}
		}
		
	}
	string KeyPharser(KeyCode key)
	{
		if(key == KeyCode.Mouse0)
		{
			return "Left Mouse";
		}else if(key == KeyCode.Mouse1)
		{
			return "Right Mouse";
		}else if(key == KeyCode.Mouse2)
		{
			return "Middle Mouse";
		}else
		{
			return key.ToString();
		}
	}
	bool KeyIsValid(KeyCode key)
	{
		Debug.Log("Key Checker called with key: "+ key.ToString());
		if(ForbiddenKeys.Contains((int)key))
		{
			return false;
		}else
		{
			return true;
		}
	}
	string PrefsID(int index)
	{
		switch(index)
		{
		case 0:
			return "ZoomOUT";
		case 1:
			return "ZoomIN";
		case 2:
			return "Forward";
		case 3:
			return "Backward";
		case 4:
			return "Left";
		case 5:
			return "Right";
		case 6:
			return "Pan";
		case 7:
			return "Build";
		case 8:
			return "QuickBuild";
		case 9:
			return "CancelBuild";
		default:
			Debug.LogError("Invalid Index or no prefs set.");
			return null;
		}
	}
	void OnGUI()
	{
		Listener();
		GUI.skin = Skin;
		if(ControlsScreen)
		{
			GUI.Window(0, new Rect((Screen.width/2)-(width/2), (Screen.height/2)-(height/2), width, height), ControlsWindow, "Controls Settings");
		}
		
	}
	void ControlsWindow(int windowID)
	{
		//GUI.BeginHorizontal();
		//GUI.Space( 20.0F );
		//GUI.BeginVertical();
		//GUI.Space( 5.0F );
		//var e = Event.current;
		scroll = GUI.BeginScrollView(new Rect(20, 50, width - 40, height - 100), scroll, new Rect(0, 0,Swidth ,Sheight ));
		//Begin Controls List
		//ZoomOut
		GUI.Label(new Rect(50, 0, 100, 20), "Zoom Out:");
		if(GUI.Button(new Rect(200, 0, 200, 20), KeyTexts[0]))
		{
			ListiningTo = 0;
			KeyTexts[0] = "Press a Key.";
			
		}
		//ZoomIn
		GUI.Label(new Rect(50, 20, 100, 20), "Zoom In:");
		if(GUI.Button(new Rect(200, 20, 200, 20), KeyTexts[1]))
		{
			ListiningTo = 1;
			KeyTexts[1] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 40, 100, 20), "Forward:");
		if(GUI.Button(new Rect(200, 40, 200, 20), KeyTexts[2]))
		{
			ListiningTo = 2;
			KeyTexts[2] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 60, 100, 20), "Backward:");
		if(GUI.Button(new Rect(200, 60, 200, 20), KeyTexts[3]))
		{
			ListiningTo = 3;
			KeyTexts[3] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 80, 100, 20), "Left:");
		if(GUI.Button(new Rect(200, 80, 200, 20), KeyTexts[4]))
		{
			ListiningTo = 4;
			KeyTexts[4] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 100, 100, 20), "Right:");
		if(GUI.Button(new Rect(200, 100, 200, 20), KeyTexts[5]))
		{
			ListiningTo = 5;
			KeyTexts[5] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 120, 100, 20), "Pan:");
		if(GUI.Button(new Rect(200, 120, 200, 20), KeyTexts[6]))
		{
			ListiningTo = 6;
			KeyTexts[6] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 140, 100, 20), "Build:");
		if(GUI.Button(new Rect(200, 140, 200, 20), KeyTexts[7]))
		{
			ListiningTo = 7;
			KeyTexts[7] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 160, 100, 20), "Quick Build:");
		if(GUI.Button(new Rect(200, 160, 200, 20), KeyTexts[8]))
		{
			ListiningTo = 8;
			KeyTexts[8] = "Press a Key.";
			
		}
		GUI.Label(new Rect(50, 180, 150, 20), "Cancel Build:");
		if(GUI.Button(new Rect(200, 180, 200, 20), KeyTexts[9]))
		{
			ListiningTo = 9;
			KeyTexts[9] = "Press a Key.";
		}
		//End Controls List
		GUI.EndScrollView();
		//GUILayout.Space( 45.0f);
		//GUI.EndVertical();
		//GUILayout.Space( 25.0f);
		//GUI.EndHorizontal();
		GUI.skin = Skin2;
		//Apply Done Cancel
		if(GUI.Button(new Rect((width/2) - 147.5f, height - 50, 95, 20), "Apply"))
		{
			//GamePlayScreen = false;
			curApplying = 0;
			isLoaded = false;
			Apply();
		}
		if(GUI.Button(new Rect((width/2) - 47.5f, height - 50, 95, 20), "Done"))
		{
			ControlsScreen = false;
			//Restore();
		}
		if(GUI.Button(new Rect((width/2) + 52.5f, height - 50, 95, 20), "Cancel"))
		{
			Restore();
			ControlsScreen = false;
		}
	}
	void Apply()
	{
		if(isApplied)
			curApplying = 0;
		while(curApplying <= OptionsCount)
		{
			PlayerPrefs.SetInt(PrefsID(curApplying), SetKeys[curApplying]);
			curApplying++;
			if(curApplying == OptionsCount)
				isApplied = true;
		}
		GameObject cam = GameObject.Find("Cam");
		if(cam != null)
		{
			cam.GetComponent<CameraControls>().UpdatePrefs();
		}
	}
	
	void Restore()
	{
		if(isRestored)
			curRestoring = 0;
		while(curRestoring <= OptionsCount)
		{
			SetKeys[curRestoring] = PlayerPrefs.GetInt(PrefsID(curRestoring), DefaultKeys[curRestoring]);
			KeyTexts[curRestoring] = KeyPharser((KeyCode)SetKeys[curRestoring]);
			curRestoring++;
			if(curRestoring == OptionsCount)
				isRestored = true;
		}
	}
}
