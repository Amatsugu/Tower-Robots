using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour {
	public int D_Credits = 5000;
	public int CurCredits;
	public bool Enemy;
	public static CreditsManager inst;
	// Use this for initialization
	void Start () 
	{
		inst = this;
		CurCredits = D_Credits;
		guiText.text = CurCredits.ToString();
		if(Application.isEditor)
			CurCredits += 999999999;
	}
	public void AddCredits(int Ammount)
	{
		CurCredits += Ammount;
		guiText.text = CurCredits.ToString();
		StatsManager.AddCreditsStats(0, Ammount);
	}
	public bool RemoveCredits(int Ammount)
	{
		if(CurCredits >= Ammount)
		{
			CurCredits -= Ammount;
			guiText.text = CurCredits.ToString();
			Debug.Log("Transaction sucessful.");
			StatsManager.AddCreditsStats(1, Ammount);
			return true;
		}else
		{
			Debug.Log("Transaction unsucesfull.");
			return false;
		}
	}
}
