using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {
	
	public GameObject Cam;
	public bool isPasued = false;
	public bool CanPause;
	public static PauseManager inst;
	//private
	private CameraControls camc;
	private float PreWindMain;
	private float PreWindTurb;
	private float PreWindPulseMag;
	private float PreWindMainPulseFreq;
	// Use this for initialization
	void Start () {
		inst = this;
		camc = Cam.gameObject.GetComponent<CameraControls>();
		DebugConsole.RegisterCommand("Pause", Pause);
		CanPause = true;
		//var x = gameObject.GetComponent<WindZone>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(CanPause)
				Pause();
		}
		
	
	}
	void Pause(params string[] args)
	{
		if(camc.s.target !=camc.PauseMenu)
		{
			camc.s._SetTarget(camc.PauseMenu);
			isPasued = true;
		}else if(camc.s.target == camc.PauseMenu)
		{
			camc.s._SetTarget(camc.PrePause);
			isPasued = false;
		}
	}
}
