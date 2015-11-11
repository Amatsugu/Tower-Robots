using UnityEngine;
using System.Collections;

public class HUD_Icon : MonoBehaviour {
	public string Tag;
	public int ID;
	public int ID_Max;
	
	public GameObject[] Icons;
	public Texture2D G_N;
	public Texture2D G_O;
	public Texture2D G_A;
	public Texture2D R_N;
	public Texture2D R_O;
	public GameObject ObjectPrefab;
	public GameObject ObjSpawner;
	public string Name;
	public float Cost;
	public float Armor;
	public float Damage;
	public string Type;
	public bool Valid = true;
	public bool Enemy;
	public int Level;
	//private
	private KeyCode KID;
	private bool Active = false;
	private GameObject ToolTip;
	private GameObject T_Name;
	private GameObject T_Cost;
	private GameObject T_Armor;
	private GameObject T_Damage;
	private GameObject PauseM;
	private PauseManager PM;
	private GameObject CreditsM;
	private CreditsManager CM;
	// Use this for initialization
	void Start () {
		gameObject.tag = Tag;
		guiTexture.texture = G_N;
		ToolTip = GameObject.Find("HUD_ToolTip");
		ToolTip.transform.position = new Vector3(-1, ToolTip.transform.position.y, ToolTip.transform.position.z);
		T_Name = GameObject.Find("ToolTip_Name");
		T_Cost = GameObject.Find("ToolTip_Cost");
		T_Armor = GameObject.Find("ToolTip_Armor");
		T_Damage = GameObject.Find("ToolTip_Dmg");
		PauseM = GameObject.Find("PauseMenu");
		PM = PauseM.GetComponent<PauseManager>();
		CreditsM = GameObject.Find("HUD_Credits");
		CM = CreditsM.GetComponent<CreditsManager>();
		switch (ID)
		{
		case 1:
			KID = KeyCode.Alpha1;
			break;
		case 2:
			KID = KeyCode.Alpha2;
			break;
		case 3:
			KID = KeyCode.Alpha3;
			break;
		case 4:
			KID = KeyCode.Alpha4;
			break;
		default:
			Debug.LogError("Default reached");
			KID = KeyCode.Alpha1;
			break;
		}
	}
	
	// Update is called once per frame
	void OnMouseEnter()
	{
		if(!PM.isPasued)
		{
			Active = true;
			if(Valid)
			{
				guiTexture.texture = G_O;
			}
			else
			{
				guiTexture.texture = R_O;
			}
		}
	}
	void OnMouseExit()
	{
		if(!PM.isPasued)
		{
			Active = false;
			ToolTip.transform.position = new Vector3(-1, ToolTip.transform.position.y, ToolTip.transform.position.z);
			if(Valid)
			{
				guiTexture.texture = G_N;
			}
			else
			{
				guiTexture.texture = R_N;
			}
		}
	}
	void Update () {
		if(Event.current != null)
			Debug.Log(Event.current.keyCode.ToString());
		if(ID_Max <= 0)
			ID_Max = 1;
		if(ID > ID_Max)
			ID = ID_Max;
		if(ID <= 0)
			ID = 1;
		if(!PM.isPasued)
		{
			if(CM.CurCredits >= Cost)
			{
				Valid = true;
			}else
			{
				Valid = false;
			}
			if(Valid && !Active)
			{
				guiTexture.texture = G_N;
			}
			if(!Valid && !Active)
			{
				guiTexture.texture = R_N;
			}
			if(Active)
			{
				if(Input.GetKey(KeyCode.Mouse0) || Input.GetKeyUp(KID))
				{
					if(Valid)
						guiTexture.texture = G_A;
					else
						guiTexture.texture = R_N;
				}else
				{
					if(Valid)
						guiTexture.texture = G_O;
					else
						guiTexture.texture = R_O;
				}
				if(Input.GetKeyUp(KeyCode.Mouse0) && Valid)
				{
					CreateSpawner();
				}
				ToolTip.transform.position = new Vector3((Input.mousePosition.x/Screen.width), ToolTip.transform.position.y, ToolTip.transform.position.z);
				T_Name.guiText.text = Name;
				T_Cost.guiText.text = "Cost: " + Cost;
				T_Armor.guiText.text = "Armor: " + Armor + "%";
				T_Damage.guiText.text = "Damage: " + Damage;
			}
		}
	}
	void CreateSpawner()
	{
		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
		GameObject Clone = Instantiate(ObjSpawner, hit.point, Quaternion.identity) as GameObject;
		Clone.GetComponent<ObjectSpawner>().ReciveInfo(ObjectPrefab, Type, (int)Cost, Enemy, Armor/100, Damage, Name, Cost * .9f - Random.Range(0, 10), Level);
	}
}

