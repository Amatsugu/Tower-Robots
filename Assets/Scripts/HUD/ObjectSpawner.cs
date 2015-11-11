using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {
	public GameObject Valid;
	public GameObject Invalid;
	public bool Good = false;
	public GameObject ObjectPrefab;
	//private
	private GameObject PauseM;
	private PauseManager PM;
	private GameObject CreditsM;
	private CreditsManager CM;
	private string Type;
	private int Cost;
	private bool Enemy;
	public float Damage;
	public float Armor;
	public string Name;
	public float SellPrice;
	public int Level;
	// Use this for initialization
	void Start () {
		PauseM = GameObject.Find("PauseMenu");
		PM = PauseM.GetComponent<PauseManager>();
		CreditsM = GameObject.Find("HUD_Credits");
		CM = CreditsM.GetComponent<CreditsManager>();
		//Debug.Log("Spawned");
	}
	// Update is called once per frame
	void Update () {
		if(!PM.isPasued)
		{
			float y = 4f;
			RaycastHit hit;
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
			RaycastHit hit2;
			Vector3 Down = new Vector3(0, -1, 0);
			Physics.Raycast(transform.position, Down, out hit2, 100);
			if(Type == "Creep")
			{
				if(hit2.collider.tag == "C_Valid")
				{
					Good = true;
				}else if(hit2.collider.tag == "T_Valid" || hit2.collider.tag == "InValid")
				{
					Good = false;
				}else
				{
					Good = false;
				}
			}
			if(Type == "Tower")
			{
				if(hit2.collider.tag == "T_Valid")
				{
					Good = true;
				}else if(hit2.collider.tag == "C_Valid" || hit2.collider.tag == "InValid")
				{
					Good = false;
				}else
				{
					Good = false;
				}
			}
			if(Good)
			{
				Valid.renderer.enabled = true;
				Invalid.renderer.enabled = false;
			}
			if(!Good)
			{
				Valid.renderer.enabled =false;
				Invalid.renderer.enabled = true;
			}
			transform.position = new Vector3(hit.point.x, y, hit.point.z);
			if(Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt("Build")))
			{
				if(Good)
				{
					GameObject Clone = Instantiate(ObjectPrefab, new Vector3(hit2.transform.position.x, transform.position.y, hit2.transform.position.z), Quaternion.identity) as GameObject;
					if(Type == "Creep")
					{
						Clone.GetComponent<CreepAI>().RecieveStatus(Enemy, Armor, Damage, Name);
					}else if(Type == "Tower")
					{
						Clone.GetComponent<TowerAI>().RecieveStatus(Enemy, Armor, Damage, Name, SellPrice, Level);
						StatsManager.AddTowerStat(0);
						//Debug.Log("tower");
					}
					CM.RemoveCredits(Cost);
					Destroy(gameObject);
				}
			}else if(Input.GetKeyUp((KeyCode)PlayerPrefs.GetInt("CancelBuild")))
			{
				Destroy(gameObject);
			}
		}
	}
	public void ReciveInfo(GameObject OBJ, string type, int cost, bool enemy, float armor, float damage, string name, float sellPrice, int level)
	{
		//Debug.Log("Data Recieved");
		ObjectPrefab = OBJ;
		Type = type;
		Cost = cost;
		Enemy= enemy;
		Armor = armor;
		Damage = damage;
		Name = name;
		Level = level;
		SellPrice = sellPrice;
	}
}
