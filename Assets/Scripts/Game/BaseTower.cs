using UnityEngine;
using System.Collections;

public class BaseTower : MonoBehaviour {
	public bool Enemy;
	public float Armor = .2f;
	public GameObject HealthManager;
	//private
	public HUD_Health HM;
	// Use this for initialization
	void Start () 
	{
		DetermineStatus();
		HM = HealthManager.GetComponent<HUD_Health>();
	}
	void DetermineStatus()
	{
		if(Enemy)
		{
			gameObject.tag = "E_Base";
		}
		if(!Enemy)
		{
			gameObject.tag = "N_Base";
		}
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
//	void OnCollisionEnter(Collision c)
//	{
//		Debug.Log("Collided with: " + c.gameObject.name);
//		var i = c.gameObject.GetComponent<CreepAI>();
//		float dmg = 0;
//		if(i != null)
//			dmg = (i.Damage-(Armor*i.Damage));
//		HM.RemoveHealth(dmg);
//		i.Health = 0;
//	}
}
