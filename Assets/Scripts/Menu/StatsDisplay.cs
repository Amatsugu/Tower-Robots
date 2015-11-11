using UnityEngine;
using System.Collections;

public class StatsDisplay : MonoBehaviour {
	public TextMesh[] feild;
	public static TextMesh[] Feilds;
	// Use this for initialization
	void Start()
	{
		Feilds = feild;
	}
	public static void ShowStats()
	{
		if(Feilds.Length <= 0)
		{
			Debug.LogError("No, feilds set.");
			return;
		}
		Feilds[0].text = "Wins on Easy: " + StatsManager.EasyWins;
		Feilds[1].text = "Wins on Medium: " + StatsManager.MedWins;
		Feilds[2].text = "Wins on Hard: " + StatsManager.HardWins;
		Feilds[3].text = "Wins on Extreme: " + StatsManager.ExtremeWins;
		Feilds[4].text = "Total Losses: " + StatsManager.Losses;
		Feilds[5].text = "Damage Dealt: " + StatsManager.DamageDealt;
		Feilds[6].text = "Damage Taken: " + StatsManager.DamageRecieved;
		Feilds[7].text = "Enimies Killed: " + StatsManager.EnimiesKilled;
		Feilds[8].text = "Total Credits Earned: " + StatsManager.CreditsGained;
		Feilds[9].text = "Total Credits Spent: " + StatsManager.CreditsSpent;
		Feilds[10].text = "Total Towers Built: " + StatsManager.TowersBuilt;
		Feilds[11].text = "Total Towers Sold: " + StatsManager.TowersSold;
		Feilds[12].text = "Total Towers Lost: " + StatsManager.TowersLost;
	}
}
