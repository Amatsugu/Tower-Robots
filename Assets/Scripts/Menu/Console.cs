using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Console : MonoBehaviour {
	void Start()
	{
		DebugConsole.RegisterCommand("SetTimeScale", SetTimeScale);
		DebugConsole.RegisterCommand("ResetTimeScale", ResetTimeScale);
		DebugConsole.RegisterCommand("ReloadLevel", ReLoadLevel);
		DebugConsole.RegisterCommand("loadLevel", LoadLevel);
		DebugConsole.RegisterCommand("log", Log);
	}
	void Update () {
		if(Input.GetKey(KeyCode.F1) && Input.GetKey(KeyCode.LeftShift))
		{
			if(DebugConsole.IsOpen)
				DebugConsole.IsOpen = false;
			if(!DebugConsole.IsOpen)
				DebugConsole.IsOpen = true;
		}
	}
	void SetTimeScale(params string[] args)
	{
		DebugConsole.Log("TimeScale set to: " +args[0]);
		float ts = float.Parse(args[0]);
		Time.timeScale = ts;
	}
	void ResetTimeScale(params string[] args)
	{
		DebugConsole.Log("TimeScale reset!");
		Time.timeScale = 1;
	}
	void ReLoadLevel(params string[] args)
	{
		Application.LoadLevel(Application.loadedLevelName);
	}
	void LoadLevel(params string[] args)
	{
		Application.LoadLevel(int.Parse(args[0]));
	}
	void Log(params string[] args)
	{
		DebugConsole.Log(args[0]);
	}
}
