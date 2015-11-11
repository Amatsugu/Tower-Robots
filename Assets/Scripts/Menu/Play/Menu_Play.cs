using UnityEngine;
using System.Collections;

public class Menu_Play : MonoBehaviour 
{
	//Vars
	public Material GUI_N;
	public Material GUI_O;
	public static bool Active = false;
	public Transform Menu;
	public bool Main;
	public Transform PauseMenu;
	public Transform PrePause;
	public GameObject PauseCam;
	private SmoothCam s;
	public float BounceFactor = .5f;
	public Vector3 Tpos;
	public GameObject Load;
	private GameObject LoadS;
	private LoadingScreen LS;
	//Functions
	void Awake()
	{
		Active = false;
		if(!GameObject.Find("LoadingScreen"))
		{
			Vector3 lspos = new Vector3(0,0, 1000);
			var  l = Instantiate(Load, lspos , Quaternion.identity);
			l.name = "LoadingScreen";
		}
		LoadS = GameObject.Find("LoadingScreen");
		if(LoadS !=null)
			Debug.Log("LoadingScreen Found");
		LS = LoadS.GetComponent<LoadingScreen>();
		if(LS !=null)
			DebugConsole.Log("LoadingScreen Component Got");
		if(!Main)
		{
			s = PauseCam.GetComponent<SmoothCam>();
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
		if(Active)
		{
			if(Input.GetMouseButtonUp(0))
			{
				if(Main)
					Play();
				else if(!Main)
				{
					PauseManager.inst.isPasued = false;
					s._SetTarget(PrePause);
				}
				
			}
		}
	}
	
	void Play()
	{
		Debug.Log("Play Clicked");
		DebugConsole.Log("Play Clicked");
		//LS.Load(1);
		SmoothCam.Instance._SetTarget(Menu);
	}
}
