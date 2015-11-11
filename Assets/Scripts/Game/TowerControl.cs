using UnityEngine;
using System.Collections;

public class TowerControl : MonoBehaviour {
	public bool Enemy = false;
	public bool isOpen = false;
	public GUISkin Skin;
	public GUISkin Skin2;
	public float height, width;
	public float bheight, bwidth;
	public bool Active;
	//private
	private string Tgt;
	private float Damage, Armor, Health;
	private RaycastHit hit;
	private Vector3 ScreenPos;
	private float SellPrice;
	private string Name;
	private GameObject Obj;
	private int Level;
	private bool isUpgradeMode = false;
	private bool isUpgradeClose = false;
	private Rect UpwindowRect;
	private TowerAI Data;
	private Vector2 scroll = Vector2.zero;
	private float delay = .5f;
	// Use this for initialization
	void Start () 
	{
		if(Enemy)
			Tgt = "E_Tower";
		if(!Enemy)
			Tgt = "N_Tower";
		delay += Time.time;
	}
	
	// Update is called once per frame

	void Update () 
	{
		if(!PauseManager.inst.isPasued)
		{
			Vector3 MousePos = Input.mousePosition;
			MousePos.y = Screen.height - MousePos.y;
			//Debug.Log(MousePos);
			if(Input.GetKeyDown(KeyCode.Mouse0) && isOpen)
			{
				Rect windowSize = new Rect(ScreenPos.x, ScreenPos.y, width, height);
				if(!isUpgradeMode)
				{
					windowSize = new Rect(ScreenPos.x, ScreenPos.y, width, height);
					if(!windowSize.Contains(new Vector2(MousePos.x, MousePos.y)))
					{
						
						isOpen = false;
						isUpgradeMode = false;
					}
				}else
				{
					windowSize = new Rect(ScreenPos.x, ScreenPos.y, 2*width, height);
					if(!windowSize.Contains(new Vector2(MousePos.x, MousePos.y)))
					{
						isOpen = false;
						isUpgradeMode = false;
					}
				}
			}
			if(!isOpen)
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
			if(hit.collider != null)
			{
				if(hit.collider.tag == Tgt)
				{
					if(Input.GetKeyDown(KeyCode.Mouse0) && !isOpen && !GameObject.FindWithTag("ObjectBuilder"))
					{
						isOpen = true;
					}
				}
			}
			if(hit.collider == null)
			{
				isOpen = false;
			}
			if(isOpen)
			{
				GetData(hit);
				if(isUpgradeMode)
				{
					Animate();
				}
				ReCalculatePos();
			}
		}
		if(PauseManager.inst.isPasued)
		{
			isOpen = false;
			isUpgradeMode = false;
		}
		if(!isOpen)
		{
			isUpgradeMode = false;
			isUpgradeClose = false;
		}
	}
	void Animate()
	{
		//Debug.Log(isUpgradeClose);
		Vector3 MousePos = Input.mousePosition;
		MousePos.y = Screen.height - MousePos.y;
		if(isUpgradeMode)
		{
			if(!isUpgradeClose)
			{
				if(UpwindowRect.x < ScreenPos.x + width)
				{
					//Debug.Log("Before: " + UpwindowRect);
					UpwindowRect = new Rect(UpwindowRect.x + width * Time.smoothDeltaTime, ScreenPos.y, width, height);
					//Debug.Log("After: " +UpwindowRect);
					//b = true;
				}
				if(UpwindowRect.x > ScreenPos.x + width)
				{
					
					UpwindowRect = new Rect(ScreenPos.x + width, ScreenPos.y, width, height);
					//Debug.Log("Goal Reached: " + UpwindowRect);
				}
				if(UpwindowRect.y != ScreenPos.y)
					UpwindowRect = new Rect(ScreenPos.x + width, ScreenPos.y, width, height);
				if(UpwindowRect.x == ScreenPos.x + width)
				{
					if(Input.GetKeyUp(KeyCode.Mouse0))
					{
						//Debug.Log("Called");
						Rect window = new Rect(ScreenPos.x, ScreenPos.y, width, height);
						if(window.Contains(MousePos))
						{
							isUpgradeClose = true;
						}
					}
				}
			}
		}
		if(isUpgradeClose)
		{
			if(ScreenPos.x < UpwindowRect.x)
			{
				//Debug.Log("Before: " + UpwindowRect);
				UpwindowRect = new Rect(UpwindowRect.x - width * Time.smoothDeltaTime, ScreenPos.y, width, height);
				//Debug.Log("After: " + UpwindowRect);
			}
			if(ScreenPos.x > UpwindowRect.x)
			{
				//Debug.Log("Goal Reached");
				UpwindowRect = new Rect(ScreenPos.x, ScreenPos.y, width, height);
				isUpgradeMode = false;
				isUpgradeClose = false;
			}
		}
	}
	void ReCalculatePos()
	{
		if(ScreenPos.x > Screen.width)
			isOpen = false;
		if(ScreenPos.x + width < 0)
			isOpen = false;
		if(ScreenPos.x < 0)
			ScreenPos.x = 0;
		if(!isUpgradeMode)
		{
			if(ScreenPos.x + width > Screen.width)
				ScreenPos.x = Screen.width - width;
			if(ScreenPos.y < 0)
				ScreenPos.y = 0;
			if(ScreenPos.y + height > Screen.height)
				ScreenPos.y = Screen.height - height;
		}else
		{
			if(ScreenPos.x + (2*width) > Screen.width)
			{
				ScreenPos.x = Screen.width - (2* width);
			}
			if(ScreenPos.y < 0)
				ScreenPos.y = 0;
			if(ScreenPos.y + height > Screen.height)
				ScreenPos.y = Screen.height - height;
		}
	}
	void OnGUI()
	{
		Rect windowRect = new Rect(ScreenPos.x, ScreenPos.y, width, height);
		
		if(isOpen)
		{
			if(isUpgradeMode)
			{
				Animate();
				GUI.skin = Skin2;
				GUI.Window(1, UpwindowRect, UpgradeWindow, "Upgrades");
			}
			GUI.skin = Skin;
			windowRect = GUI.Window(0, windowRect, InfoWindow, Name);
		}
	}
	void UpgradeWindow(int windowID)
	{
		GUI.skin = Skin2;
		GUILayout.Space(15);
		GUILayout.BeginHorizontal();
		//GUILayout.Space( 20.0F );
		GUILayout.BeginVertical();
		GUILayout.Space( 10.0F );
		scroll = GUILayout.BeginScrollView(scroll);
		//Range
		GUILayout.Label("Range: " + Data.AtkRange);
		if(Data.AtkRange < Data.MaxRange && Data.Level < Data.MaxLevel)
		{
			if(CreditsManager.inst.CurCredits >= Data.RangeCost * Data.Level)
			{
				if(GUILayout.Button("Buy: " + Data.RangeCost * Data.Level+ "c"))
				{
					if(Level < Data.MaxLevel && Data.AtkRange < Data.MaxRange)
					{
						if(CreditsManager.inst.RemoveCredits((int)Data.RangeCost * Data.Level))
						{
							Data.AtkRange += 5f;
							Data.Level++;
						}
					}
				}
			}else
			{
				GUILayout.Box("Insufficent Funds!");
			}
		}else
		{
			GUILayout.Box("Max!");
		}
		//Damage
		GUILayout.Label("Damage: " + Data.Damage);
		if(Data.Damage < Data.MaxDamage && Data.Level < Data.MaxLevel)
		{
			if(CreditsManager.inst.CurCredits >= Data.DamageCost * Data.Level)
			{
				if(GUILayout.Button("Buy: " + Data.DamageCost * Data.Level + "c"))
				{
					if(Level < Data.MaxLevel && Data.Damage < Data.MaxDamage)
					{
						if(CreditsManager.inst.RemoveCredits((int)Data.DamageCost * Data.Level))
						{
							Data.Damage += 5f;
							Data.Level++;
						}
					}
				}
			}else
			{
				GUILayout.Box("Insufficent Funds!");
			}
		}else
		{
			GUILayout.Box("Max!");
		}
		//Armor
		GUILayout.Label("Armor: " + Data.Armor*100 + "%");
		if(Data.Armor < Data.MaxArmor && Data.Level < Data.MaxLevel)
		{
			if(CreditsManager.inst.CurCredits >= Data.ArmorCost * Data.Level)
			{
				if(GUILayout.Button("Buy: " + Data.ArmorCost * Data.Level + "c"))
				{
					if(Level < Data.MaxLevel && Data.Armor < Data.MaxArmor)
					{
						if(CreditsManager.inst.RemoveCredits((int)Data.ArmorCost * Data.Level))
						{
							Data.Armor += .1f;
							Data.Level++;
						}
					}
				}
			}else
			{
				GUILayout.Box("Insufficent Funds!");
			}
		}else
		{
			GUILayout.Box("Max!");
		}
		//FireRate
		GUILayout.Label("FireRate: " + Data.ROF);
		if(Data.ROF > Data.MaxROF && Data.Level < Data.MaxLevel && Data.ROF != Data.MaxROF)
		{
			if(CreditsManager.inst.CurCredits >= Data.ROFCost * Data.Level)
			{
				if(GUILayout.Button("Buy: " + Data.ROFCost * Data.Level + "c"))
				{
					if(Level < Data.MaxLevel && Data.ROF > Data.MaxROF)
					{
						if(CreditsManager.inst.RemoveCredits((int)Data.ROFCost * Data.Level))
						{
							Data.ROF -= .1f;
							Data.Level++;
						}
					}
				}
			}else
			{
				GUILayout.Box("Insufficent Funds!");
			}
		}else if(Data.ROF == Data.MaxROF)
		{
			GUILayout.Box("Max!");
		}else
		{
			GUILayout.Box("Max!");
		}
		//Looting
		if(Data.Level >= 7)
		{
			GUILayout.Label("Looting: ");
			if(!Data.Looting)
			{
				if(CreditsManager.inst.CurCredits >= 2500)
				{
					if(GUILayout.Button("Buy: " + 2500+ "c"))
					{
						if(CreditsManager.inst.RemoveCredits(2500))
						{
							Data.Looting = true;
							//Data.Level++;
						}
					}
				}else
				{
					GUILayout.Box("Insufficent Funds!");
				}
			}else if(Data.Looting)
			{
				GUILayout.Box("Taken!");
			}
		}
		//Crits
		if(Data.Level >= 7)
		{
			GUILayout.Label("Critical Hits: ");
			if(!Data.Crits )
			{
				if(CreditsManager.inst.CurCredits >= 2500)
				{
					if(GUILayout.Button("Buy: " + 2500+ "c"))
					{
						if(CreditsManager.inst.RemoveCredits(2500))
						{
							Data.Crits = true;
							//Data.Level++;
						}
					}
				}else
				{
					GUILayout.Box("Insufficent Funds!");
				}
			}else if(Data.Crits)
			{
				GUILayout.Box("Taken!");
			}
		}
		//Auto Repair
		if(Data.Level >= 5)
		{
			GUILayout.Label("Auto Repair: ");
			if(!Data.AutoRepair)
			{
				if(CreditsManager.inst.CurCredits >= 2500)
				{
					if(GUILayout.Button("Buy: " + 2500+ "c"))
					{
						if(CreditsManager.inst.RemoveCredits(2500))
						{
							Data.AutoRepair = true;
							//Data.Level++;
						}
					}
				}else
				{
					GUILayout.Box("Insufficent Funds!");
				}
			}else if(Data.AutoRepair)
			{
				GUILayout.Box("Taken!");
			}
		}
		GUILayout.EndScrollView();
		GUILayout.Space( 5.0f);
		GUILayout.EndVertical();
		//GUILayout.Space( 25.0f);
		GUILayout.EndHorizontal();
	}
	void InfoWindow(int windowID)
	{
		GUI.skin = Skin;
		GUILayout.Space(25);
		GUILayout.Label("Level: " + Data.Level + " of " + Data.MaxLevel);
		GUILayout.Label("Damage: " +Damage);
		GUILayout.Label("Armor: " +Armor*100+"%");
		GUILayout.Label("Health: " +(int)Health+"%");
		if(!PauseManager.inst.isPasued)
		{
			if(GUILayout.Button("Upgrade"))
			{
				if(isUpgradeMode)
				{
					isUpgradeClose = true;
				}
				if(!isUpgradeMode && !isUpgradeClose)
				{
					UpwindowRect = new Rect(ScreenPos.x, ScreenPos.y, width, height);
					//Debug.Log("Default Set");
					isUpgradeMode = true;
				}
				//Debug.Log("Upgeade Mode: " +isUpgradeMode);
				//Debug.Log("Close: " + isUpgradeClose);
			}
			if(CreditsManager.inst.CurCredits >= (int)(100 - Health)*Level)
			{
				if(GUILayout.Button("Repair: " + (int)(100 - Health)*Level + "c"))
				{
					if(isUpgradeMode)
					{
						isUpgradeClose = true;
					}
					//isOpen = true;
					Repair((int)(100 - Health)*Level);
				}
			}else
			{
				GUILayout.Box("Insufficent Funds!");
			}
			if(GUILayout.Button("Sell: " +(int)(SellPrice*(Health/100))+"c"))
			{
				Sell((int)SellPrice*(Health/100)*Data.Level);
				StatsManager.AddTowerStat(2);
				if(isUpgradeMode)
				{
					isUpgradeClose = true;
				}
			}
		}else
		{
			GUILayout.Box("Upgrade");
			GUILayout.Box("Repair: " +(int)(100 - Health)*Level + "c");
			GUILayout.Box("Sell: " +(int)(SellPrice*(Health/100))*Data.Level + "c");
		}
	}
	void Sell(float price)
	{
		hit.collider.GetComponent<TowerAI>().Sell((int)price);
		isOpen = false;
	}
	void Repair(float price)
	{
		isOpen = true;
		hit.collider.GetComponent<TowerAI>().Repair((int)price);
	}
	void GetData(RaycastHit hitObj)
	{
		Data = hitObj.collider.GetComponent<TowerAI>();
		Damage = Data.Damage;
		Armor = Data.Armor;
		Health = Data.Health;
		SellPrice = Data.SellPrice;
		Name = Data.Name;
		Level = Data.Level;
		var cam = Camera.mainCamera;
		Obj = hitObj.collider.gameObject;
		ScreenPos = cam.WorldToScreenPoint(Obj.transform.position);
		ScreenPos.y = Screen.height - ScreenPos.y;
		//Debug.Log("ScreenPos: " + ScreenPos);
	}
}
