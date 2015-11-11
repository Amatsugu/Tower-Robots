using UnityEngine;
using System.Collections;

public class Options_Audio : MonoBehaviour 
{
	//Vars
	public Material GUI_N;
	public Material GUI_O;
	public GUISkin Skin;
	public GUISkin Skin2;
	public float width = 500f;
	public float height = 400f;
	public static bool Active = false;
	private bool AudioScreen = false;
	float MA;
	float MA_Slider;
	float D_MA = 1;
	//Functions
	void Start()
	{
		LoadPrefs();
	}
	void LoadPrefs()
	{
		MA = PlayerPrefs.GetFloat("MasterAudio", D_MA);
		MA_Slider = MA * 100;
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
				AudioScreen = true;
				//Debug.Log("Options");
			}
		}
		if(Input.GetKeyUp(KeyCode.Escape))
			AudioScreen = false;
		//MA_Slider = MA * 100;
		MA = MA_Slider/ 100;
		AudioListener.volume = MA;
	}
	void OnGUI()
	{
		GUI.skin = Skin;
		if(AudioScreen)
		{
			GUI.Window(0, new Rect((Screen.width/2)-(width/2), (Screen.height/2)-(height/2), width, height), AudioWindow, "Audio Settings");
		}
		
	}
	void AudioWindow(int windowID)
	{
		GUI.Label(new Rect((width/2) - 150, 45, 300, 20), "Master Volume: " + (int)(MA * 100) +"%");
		MA_Slider = GUI.HorizontalSlider(new Rect((width/2) - 150, 65, 300, 20), MA_Slider, 0, 100);
		
		GUI.skin = Skin2;
		//Apply Done Cancel
		if(GUI.Button(new Rect((width/2) - 147.5f, height - 50, 95, 20), "Apply"))
		{
			//GamePlayScreen = false;
			Apply();
		}
		if(GUI.Button(new Rect((width/2) - 47.5f, height - 50, 95, 20), "Done"))
		{
			AudioScreen = false;
			//Restore();
		}
		if(GUI.Button(new Rect((width/2) + 52.5f, height - 50, 95, 20), "Cancel"))
		{
			AudioScreen = false;
			Restore();
		}
	}
	void Apply()
	{
		PlayerPrefs.SetFloat("MasterAudio", MA);
		AudioListener.volume = MA;
		//Camera.main.audio.volume = MA;
	}
	
	void Restore()
	{
		LoadPrefs();
	}
}
