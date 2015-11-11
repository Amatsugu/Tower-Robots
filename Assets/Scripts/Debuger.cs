using UnityEngine;
using System.Collections;

public class Debuger : MonoBehaviour {
	private bool showStats = false;
	private bool isShowing = true;
	private string version;
	void Start()
	{
		version = ChangeLog.Build;
	}
	void OnGUI()
	{
		if(isShowing)
		{
			if(Application.isEditor)
			{
				float CurTime = Time.time;
				GUILayout.Space(60);
				GUILayout.Space(10);
				GUILayout.Label("Current Time: " + CurTime);
				GUILayout.Label("Current Towers: " + GameObject.FindGameObjectsWithTag("N_Tower").Length);
				GUILayout.Label("Current Enemies: " + GameObject.FindGameObjectsWithTag("E_Creep").Length);
				showStats = GUILayout.Toggle( showStats, "Show Stats");
				if(showStats)
				{
					GUILayout.Label("Losses: " + StatsManager.Losses);
					GUILayout.Label("Easy Wins: " + StatsManager.EasyWins);
					GUILayout.Label("Medium Wins: " + StatsManager.MedWins);
					GUILayout.Label("Hard Wins: " + StatsManager.HardWins);
					GUILayout.Label("Extreme Wins: " + StatsManager.ExtremeWins);
					GUILayout.Label("Credits Earned: " + StatsManager.CreditsGained);
					GUILayout.Label("Credits Spent: " + StatsManager.CreditsSpent);
					GUILayout.Label("Towers Lost: " + StatsManager.TowersLost);
					GUILayout.Label("Towers Built: " + StatsManager.TowersBuilt);
					GUILayout.Label("Towers Sold: " + StatsManager.TowersSold);
					GUILayout.Label("Damage Dealt: " + StatsManager.DamageDealt);
					GUILayout.Label("Damage Recieved: " + StatsManager.DamageRecieved);
					GUILayout.Label("Enimies Killed: " + StatsManager.EnimiesKilled);
				}
			}
		}
		
	}
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.F1) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
		{
			isShowing = !isShowing;
		}
		if(Input.GetKeyUp(KeyCode.F2))
		{
			Application.CaptureScreenshot("Tower Robots_"+version+"_ScreenShot");
		}
	}
}
