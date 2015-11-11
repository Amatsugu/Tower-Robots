using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour {
	public static int Losses = 0;
	public static int EasyWins = 0;
	public static int MedWins = 0;
	public static int HardWins = 0;
	public static int ExtremeWins = 0;
	public static int TowersLost = 0;
	public static int TowersBuilt = 0;
	public static int TowersSold = 0;
	public static int EnimiesKilled = 0;
	public static int CreditsGained = 0;
	public static int CreditsSpent = 0;
	public static int DamageDealt = 0;
	public static int DamageRecieved = 0;
	public static bool AllowStats = false;

	void Start () 
	{
		Losses = PlayerPrefs.GetInt("GameLoss");
		EasyWins = PlayerPrefs.GetInt("WinsEasy");
		MedWins = PlayerPrefs.GetInt("WinsMed");
		HardWins = PlayerPrefs.GetInt("WinsHard");
		ExtremeWins = PlayerPrefs.GetInt("WinsExtreme");
		TowersLost = PlayerPrefs.GetInt("LostTowers");
		TowersBuilt = PlayerPrefs.GetInt("BuildTowers");
		TowersSold = PlayerPrefs.GetInt("SoldTowers");
		EnimiesKilled = PlayerPrefs.GetInt("KilledEnimies");
		CreditsGained = PlayerPrefs.GetInt("GainedCredits");
		CreditsSpent = PlayerPrefs.GetInt("SpentCredits");
		DamageDealt = PlayerPrefs.GetInt("DealtDamage");
		DamageRecieved = PlayerPrefs.GetInt("RecievedDamage");
		AllowStats = true;
		DebugConsole.Log("Loaded all Stats and Displayed them.");
		StatsDisplay.ShowStats();
	}
	void ResetStats(int Value)
	{
		Losses = Value;
		EasyWins = Value;
		MedWins = Value;
		HardWins = Value;
		ExtremeWins = Value;
		TowersLost = Value;
		TowersBuilt = Value;
		TowersSold = Value;
		EnimiesKilled = Value;
		CreditsGained = Value;
		CreditsSpent = Value;
		DamageDealt = Value;
		DamageRecieved = Value;
	}
	public static bool AddLoss()
	{
		if(AllowStats)
		{
			Losses++;
			PlayerPrefs.SetInt("GameLoss", Losses);
			//KongregateAPI.SubmitStat("Loss", Losses);
			return true;
		}else
			return false;
	}
	public static bool AddWin()
	{
		if(AllowStats)
		{
			switch(DifficultyLoader.Instance.Difficulty)
			{
			case 0:
				EasyWins++;
				PlayerPrefs.SetInt("WinsEasy", EasyWins);
				//KongregateAPI.SubmitStat("WinsEasy", EasyWins);
				return true;
			case 1:
				MedWins++;
				PlayerPrefs.SetInt("WinsMed", MedWins);
				//KongregateAPI.SubmitStat("WinsMedium", EasyWins);
				return true;
			case 2:
				HardWins++;
				PlayerPrefs.SetInt("WinsHard", HardWins);
//				KongregateAPI.SubmitStat("WinsHard", EasyWins);
				return true;
			case 3:
				ExtremeWins++;
				PlayerPrefs.SetInt("WinsExtreme", ExtremeWins);
//				KongregateAPI.SubmitStat("WinsExtreme", EasyWins);
				return true;
			default:
				return false;
			}
		}else
			return false;
	}
	public static bool AddTowerStat(int StatID)
	{
		if(AllowStats)
		{
			switch(StatID)
			{
			case 0:
				TowersBuilt++;
				PlayerPrefs.SetInt("BuiltTowers", TowersBuilt);
				return true;
			case 1:
				TowersLost++;
				PlayerPrefs.SetInt("LostTowers", TowersLost);
				return true;
			case 2:
				TowersSold++;
				PlayerPrefs.SetInt("SoldTowers", TowersSold);
				return true;
			default:
				return false;
			}
		}else
			return false;
	}
	public static bool AddKillsStats(int StatID)
	{
		if(AllowStats)
		{
			switch(StatID)
			{
			case 0:
				EnimiesKilled++;
				PlayerPrefs.SetInt("KilledEnimies", EnimiesKilled);
				//KongregateAPI.SubmitStat("EnemiesKilled", EnimiesKilled);
				return true;
			default:
				return false;
			}
		}else
			return false;
	}
	public static bool AddCreditsStats(int StatID, int ammount)
	{
		if(AllowStats)
		{
			switch(StatID)
			{
			case 0:
				CreditsGained += ammount;
				PlayerPrefs.SetInt("GainedCredits", CreditsGained);
				//KongregateAPI.SubmitStat("CreditsGained", CreditsGained);
				return true;
			case 1:
				CreditsSpent += ammount;
				PlayerPrefs.SetInt("SpentCredits", CreditsSpent);
				return true;
			default:
				return false;
			}
		}else
			return false;
	}
	public static bool AddDamageDealt(int ammount)
	{
		if(AllowStats)
		{
			DamageDealt += ammount;
			PlayerPrefs.SetInt("DealtDamage", DamageDealt);
			return true;
		}else
			return false;
	}
	public static bool AddDamageRecieved(int ammount)
	{
		if(AllowStats)
		{
			DamageRecieved += ammount;
			PlayerPrefs.SetInt("RecievedDamage", DamageRecieved);
			return true;
		}else
			return false;
	}
}
