using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Options_Video : MonoBehaviour 
{
	//Vars
	//public
	public Material GUI_N;
	public Material GUI_O;
	public static bool Active = false;
	public GUISkin Skin;
	public GUISkin Skin2;
	public float width = 500f;
	public float height = 400f;
	public bool EDIT = true;
	//private
	private bool VideoScreen = false;
	private string TQualityTxt = "Low";
	private string VSyncTxt = "Off";
	private string AnisoFilterTxt = "Off";
	private string AATxt = "Off";
	private string HQTTxt = "Off";
	private string FULTxt = "Fullscreen : Off";
	public Resolution[] ResList;
	private string RESTxt = "x";
	//Prefs
	int TQM; //Texture Quality
	int VSy; //Vsync
	int AOF; //Anisotropic Filtering
	int AAF; //AA Filtering
	int HQT; //HighQuality Terrain
	public int RESI; //Res width
	int FUL; //Fullsceen?
	bool FULB; //^bool
	//Prefs Default
	int D_TQM = 2;
	int D_VSy = 0;
	int D_AOF = 0;
	int D_AAF = 0;
	int D_HQT = 0;
	int D_RESI = 0;
	int D_FUL = 0;
	//Functions
	void Awake()	
	{
		ResList = Screen.resolutions;
		if(PlayerPrefs.GetInt("First_V") == 0)
		{
			TQM = D_TQM;
			VSy = D_VSy;
			AOF = D_AOF;
			AAF = D_AAF;
			HQT = D_HQT;
			RESI = D_RESI;
			FUL = D_FUL;
			PlayerPrefs.SetInt("First_V", 1);
			PlayerPrefs.SetInt("TexQual", D_TQM);
			PlayerPrefs.SetInt("VSync", VSy);
			PlayerPrefs.SetInt("AnisoFilter", AOF);
			PlayerPrefs.SetInt("AAFilter", AAF);
			PlayerPrefs.SetInt("HighQualityTer", HQT);
			PlayerPrefs.SetInt("ResI", RESI);
			PlayerPrefs.SetInt("Fullscreen", FUL);
			
		}else
		{
			//Load Prefs
			TQM = PlayerPrefs.GetInt("TexQual", D_TQM);
			VSy = PlayerPrefs.GetInt("VSync", D_VSy);
			AOF = PlayerPrefs.GetInt("AnisoFilter", D_AOF);
			AAF = PlayerPrefs.GetInt("AAFilter", D_AAF);
			HQT = PlayerPrefs.GetInt("HighQualityTer", D_HQT);
			RESI = PlayerPrefs.GetInt("ResI", D_RESI);
			FUL = PlayerPrefs.GetInt("Fullscreen", D_FUL);
			if(FUL == 1)
				FULB = true;
			if(FUL == 0)
				FULB = false;
		}
		if(EDIT)
			RESI = 0;
		//Apply Loaded Prefs
		ApplyLoaded();
	}
	void ApplyLoaded()
	{
		//Vsync
		QualitySettings.vSyncCount = VSy;
		//Texture Quality
		QualitySettings.masterTextureLimit = TQM;
		//Aniso Filter
		if(AOF == 0)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		}else if(AOF == 1)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
		}
		//AA Filter
		QualitySettings.antiAliasing = AAF;
		//High Quality Terrain
		if(HQT == 0)
		{
			QualitySettings.softVegetation = false;
		}else if(HQT == 1)
		{
			QualitySettings.softVegetation = true;
		}
		//Fullscreen and res
		if(FUL == 1)
			FULB = true;
		if(FUL == 0)
			FULB = false;
		if(RESI > ResList.Length -1)
		{
			RESI = ResList.Length -1;
			DebugConsole.Log("Resolution Reset to: " + ResList[RESI].width + "x" + ResList[RESI].height +", "+ ResList[RESI].refreshRate+"Hz");
		}
		//Screen.SetResolution(ResList[RESI].width, ResList[RESI].height, FULB);
		Screen.SetResolution(960, 600, FULB);
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
				VideoScreen = true;
				Debug.Log("Options");
			}
		}
		if(Input.GetKeyUp(KeyCode.Escape))
			VideoScreen = false;
		//Quality
		if(TQM == 0) //Very High
		{
			TQualityTxt = "Very High";
			
		}else if(TQM == 1) //High
		{
			TQualityTxt = "High";
		}else if(TQM == 2) //Medium
		{
			TQualityTxt = "Medium";	
		}else if(TQM == 3) //Low
		{
			TQualityTxt = "Low";
		}
		//Vsync
		if(VSy == 0)
		{
			VSyncTxt = "Off";
		}else if(VSy == 1)
		{
			VSyncTxt = "VBlank";	
		}else if(VSy  == 2)
		{
			VSyncTxt = "2nd VBlank";
		}
		//Aniso Filter
		if(AOF == 0)
		{
			AnisoFilterTxt = "Off";
		}else if(AOF == 1)
		{
			AnisoFilterTxt = "On";
		}
		//AA Filter
		if(AAF == 0)
			AATxt = "Off";
		if(AAF >= 2)
			AATxt = AAF+"x";
		//High Quality Terrain
		if(HQT == 0)
		{
			HQTTxt = "Off";
		}else if(HQT == 1)
		{
			HQTTxt = "On";
		}
		//Res
		RESTxt = ResList[RESI].width + "x" + ResList[RESI].height +", "+ ResList[RESI].refreshRate+"Hz";
		//Fullscreen?
		if(FUL == 0)
		{
			FULTxt = "Fullscreen: Off";
			FULB = false;
		}else if(FUL == 1)
		{
			FULTxt = "Fullscreen: On";
			FULB = true;
		}
		
	}
	void OnGUI()
	{
		GUI.skin = Skin;
		if(VideoScreen)
		{
			GUI.Window(0, new Rect((Screen.width/2)-(width/2), (Screen.height/2)-(height/2), width, height), VideoWindow, "   Video Settings");
		}
	}
	void VideoWindow(int windowID)
	{
		//Texture Quality
		GUI.Label(new Rect((width/2) - 100, 40, 200, 20), "Texture Quality");
		if(GUI.Button(new Rect((width/2) - 100, 60, 200, 20),TQualityTxt))
		{
			if(TQM >= 0)
				TQM-=1;
			if(TQM < 0)
				TQM = 3;
		}
		//Vsync
		GUI.Label(new Rect((width/2) - 100, 85, 200, 20), "VSync");
		if(GUI.Button(new Rect((width/2) - 100, 105, 200, 20), VSyncTxt))
		{
			if(VSy <= 2)
				VSy+=1;
			if(VSy > 2)
				VSy = 0;
		}
		//Anisotropic Filtering
		GUI.Label(new Rect((width/2) - 200, 130, 400, 20), "Anisotropic Filtering");
		if(GUI.Button(new Rect((width/2) - 100, 150, 200, 20), AnisoFilterTxt))
		{
			if(AOF <= 1)
				AOF += 1;
			if(AOF > 1)
				AOF = 0;
		}
		//AA Filter
		GUI.Label(new Rect((width/2) - 100, 175, 200, 20), "AntiAliasing");
		if(GUI.Button(new Rect((width/2) - 100, 195, 200, 20), AATxt))
		{
			if(AAF == 0)
			{
				AAF = 2;
			}else if(AAF == 2)
			{
				AAF = 4;
			}else if(AAF == 4)
			{
				AAF = 8;
			}else if(AAF == 8)
			{
				AAF = 0;
			}
		}
		//High Quality Terrain
		GUI.Label(new Rect((width/2) - 200, 220, 400, 20), "High Quality Terrain");
		if(GUI.Button(new Rect((width/2) - 100, 240, 200, 20), HQTTxt))
		{
			if(HQT == 0)
			{
				HQT = 1;
			}else if(HQT == 1)
			{
				HQT = 0;	
			}
		}
		//Resolution
		GUI.Label(new Rect((width/2) - 100, 265, 200, 20), "Resolution");
		if(GUI.Button(new Rect((width/2) - 100, 285, 200, 20), RESTxt)) //Res
		{
			if(RESI < ResList.Length-1)
			{
				RESI += 1;
			}else if(RESI >= ResList.Length-1)
			{
				RESI = 0;	
			}
		}
		if(GUI.Button(new Rect((width/2) - 100, 305, 200, 20), FULTxt)) //Full?
		{
			if(FUL == 0)
			{
				FUL = 1;
			}else if(FUL == 1)
			{
				FUL = 0;	
			}
		}
		GUI.skin = Skin2;
		//Close Buttons
		if(GUI.Button(new Rect((width/2) - 147.5f, height - 50, 95, 20), "Apply"))
		{
			//GamePlayScreen = false;
			Apply();
		}
		if(GUI.Button(new Rect((width/2) - 47.5f, height - 50, 95, 20), "Done"))
		{
			VideoScreen = false;
			//Restore();
		}
		if(GUI.Button(new Rect((width/2) + 52.5f, height - 50, 95, 20), "Cancel"))
		{
			VideoScreen = false;
			Restore();
		}
	}
	void Apply()
	{
		PlayerPrefs.SetInt("TexQual", TQM); //Texture Quality
		QualitySettings.masterTextureLimit = TQM;
		PlayerPrefs.SetInt("VSync", VSy); //Vsync
		QualitySettings.vSyncCount = VSy;
		if(AOF == 0) //Aniso Filter
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		}else if(AOF == 1)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
		}
		PlayerPrefs.SetInt("AnisoFilter", AOF);
		QualitySettings.antiAliasing = AAF; //AA Filter
		PlayerPrefs.SetInt("AAFilter", AAF);
		PlayerPrefs.SetInt("HighQualityTer", HQT); //High Quality Terrain
		if(HQT == 0)
		{
			QualitySettings.softVegetation = false;
		}else if(HQT == 1)
		{
			QualitySettings.softVegetation = true;
		}
		PlayerPrefs.SetInt("ResI", RESI);//Res
		PlayerPrefs.SetInt("Fullscreen", FUL);//Fullscreen?
		Screen.SetResolution(ResList[RESI].width, ResList[RESI].height, FULB);
	}
	void Restore()
	{
		TQM = PlayerPrefs.GetInt("TexQual");
		VSy = PlayerPrefs.GetInt("VSync");
		AOF = PlayerPrefs.GetInt("AnisoFilter");
		AAF = PlayerPrefs.GetInt("AAFilter");
		HQT = PlayerPrefs.GetInt("HighQualityTer");
		RESI = PlayerPrefs.GetInt("ResI");
		FUL = PlayerPrefs.GetInt("Fullscreen");
	}
}