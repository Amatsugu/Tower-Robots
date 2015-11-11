using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreepAI : MonoBehaviour {
	//Vars
	//Public
	public Transform Head;
	public GameObject Ragdoll;
	public GameObject Explosion;
	public Rigidbody Bullet;
	public Transform BSpawn;
	public float ROF = 10;
	public float BSpeed = 50;
	public float WpRange = 5;
	public float AtkRange = 3;
	public float StayD = 20;
	public float TurnSpeed = 3;
	public float MoveSpeed = 2;
	public bool Enemy = false;
	public float ViewAngle = .5f;
	public float Health = 100;
	public float Armor = .10f;
	public float Damage = 5;
	public string Name;
	public float Loot;
	//Private
	private float nextFireTime;
	private GameObject WayPointList;
	private GameObject[] Waypoints;
	private Transform TargetPos = null;
	private GameObject Target = null;
	private GameObject CreditsM;
	private CreditsManager CM;
	private WayPoints wp;
	private int CurWp = 0;
	private float distance;
	public string Tgt;
	//private string TgtGoal;
	private LineRenderer Lr;
	private RaycastHit hit;
	private RaycastHit lhit;
	private RaycastHit rhit;
	private string CTag;
	private int layerMask;
	private Color GColor;
	private bool wasP;
	private float PausedTime;

	// Use this for initialization
	void Awake() {
		WayPointList = GameObject.Find("Wp0");
		GetWayPoints();
		DetermineStatus();
		FindClosestWaypoint();
		FindClosestEnemy();
		Loot = Random.Range(10, 150);
		//MoveSpeed += (float)Random.Range(0,3);
	}
	public void RecieveBuff( bool enemy, float damageMod, float healthMod, float lootMod)
	{
		RecieveStatus(enemy, Armor, Damage, Name);
		Damage += damageMod;
		Health += healthMod;
		Loot += lootMod;
	}
	public void DealDamage(float damage)
	{
		float dmg = 0;
		dmg = (damage-(Armor*damage));
		Health -= dmg;		
	}
	void FindClosestWaypoint()
	{
		int c = 0;
		int closest = c;
		float distance = Mathf.Infinity;
        Vector3 position = transform.position;
		foreach (GameObject wp in Waypoints)
		{
			Vector3 diff = wp.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) 
			{
				closest = c;
				distance = curDistance;
            }
			c++;
		}
		if(Enemy)
			CurWp = closest+1;
		if(!Enemy)
			CurWp = closest;
	}
	void Start()
	{
		CreditsM = GameObject.Find("HUD_Credits");
		CM = CreditsM.GetComponent<CreditsManager>();
		Lr = gameObject.GetComponent<LineRenderer>();
		if(Armor > 1)
			Armor = 1;
		if(Armor < 0)
			Armor = 0;
	}
	void GetWayPoints()
	{
		wp = WayPointList.GetComponent<WayPoints>();
		Waypoints = wp._WayPoints;
		
	}
	void FindClosestEnemy() {
		Target = null;
		TargetPos = null;
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(Tgt);
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos) 
		{
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                Target = go;
				TargetPos = Target.transform;
                distance = curDistance;
            }
        }
		if(Target != null)
			TargetPos = Target.transform;
    }
	void SelfDestruct(bool loot)
	{
		GameObject Clone = Instantiate(Ragdoll, transform.position, Quaternion.identity) as GameObject;
		Clone.GetComponent<RagdollControl>().SetRagdollInfo(Enemy, gameObject.renderer.material.color);
		GameObject Exp = Instantiate(Explosion, transform.position, Quaternion.identity) as GameObject;
		Exp.GetComponent<Detonator>().color = gameObject.renderer.material.color;
		if(CM.Enemy != Enemy && loot)
		{
			CM.AddCredits((int)Loot);
		}
		StatsManager.AddKillsStats(0);
		Destroy(gameObject);
	}
	void DetermineStatus()
	{
		if(Enemy)
		{
			Tgt = "N_Tower";
			//TgtGoal = "N_Base";
			layerMask = 1<<9;
			layerMask = ~layerMask;
			gameObject.renderer.material.color = Color.red;
			GColor = Color.red;
			gameObject.layer = 8;
			gameObject.tag = "E_Creep";
		}
		if(!Enemy)
		{
			Tgt = "E_Tower";
			//TgtGoal = "E_Base";
			layerMask = 1<<8;
			layerMask = ~layerMask;
			gameObject.renderer.material.color = Color.blue;
			GColor = Color.blue;
			gameObject.layer = 9;
			gameObject.tag = "N_Creep";
		}
	}
	void LateUpdate () {
		//GetWayPoints();
		//distance = Vector3.Distance(Waypoints[CurWp].transform.position, transform.position);
		bool r = false;
		if(r)
		{
			DetermineStatus();
			GetWayPoints();
			r = true;
		}
		if(!PauseManager.inst.isPasued)
		{
			if(Target == null || TargetPos == null)
			{
				FindClosestEnemy();
			}
		}
	}
	void Update()
	{
		if(PauseManager.inst.isPasued)
		{
			wasP = true;
			PausedTime = Time.time;
		}
		if(!PauseManager.inst.isPasued)
		{
			Transform thisTransform = transform;
			if(Health <= 0)
			{
				bool r2 = true;
				if(r2)
				{
					SelfDestruct(true);
					r2 = false;
				}
			}
			if(CurWp == Waypoints.Length-1)
			{
				float dist = distance = Vector3.Distance(Waypoints[CurWp].transform.position, transform.position);
				if(dist <= WpRange)
				{
					GameObject x = GameObject.Find("BaseTower");
					var x2 = x.GetComponent<BaseTower>();
					x2.HM.RemoveHealth(Damage-(x2.Armor*Damage));
					SelfDestruct(false);
				}
			}
			DetermineCurWayPoint();
			bool r = false;
			var rotation = Quaternion.LookRotation(Waypoints[CurWp].transform.position - thisTransform.position);
			thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, Time.deltaTime * TurnSpeed);
			Vector3 fwd = thisTransform.TransformDirection(Vector3.forward);
			Vector3 left = thisTransform.TransformDirection(new Vector3(-ViewAngle, 0, 1));
			Vector3 right = thisTransform.TransformDirection(new Vector3(ViewAngle, 0 , 1));
			if(!Physics.Raycast(thisTransform.position, left, out lhit, StayD, layerMask) && !Physics.Raycast(thisTransform.position, fwd, out hit, StayD, layerMask) && !Physics.Raycast(thisTransform.position, right, out rhit, StayD, layerMask))
				thisTransform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
			else if(Physics.Raycast(thisTransform.position, left, out lhit, StayD))
				thisTransform.Translate(left * Time.deltaTime * (MoveSpeed/2));
			else if(Physics.Raycast(transform.position, right, out rhit, StayD))
				thisTransform.Translate(right * Time.deltaTime * (MoveSpeed/2));
			if(hit.point != Vector3.zero)
				Debug.DrawLine(thisTransform.position, hit.point, Color.red);
			if(lhit.point != Vector3.zero)
				Debug.DrawLine(thisTransform.position, lhit.point, Color.green);
			if(rhit.point != Vector3.zero)
				Debug.DrawLine(thisTransform.position, rhit.point, Color.yellow);
			if(DebugConsole.IsOpen)
			{
				Lr.SetColors(Color.green, Color.yellow);
				Lr.SetPosition(0, transform.position);
				if(hit.point != Vector3.zero)
				{
					Lr.SetColors(Color.yellow, Color.red);
					Lr.SetPosition(1, hit.point);
				}
				else if(hit.point == Vector3.zero)
				{
					Lr.SetColors(Color.green, Color.yellow);
					Lr.SetPosition(1, thisTransform.position);
				}
			}else if(!DebugConsole.IsOpen)
			{
				Lr.SetColors(Color.green, Color.yellow);
				Lr.SetPosition(0, thisTransform.position);
				Lr.SetPosition(1, thisTransform.position);
			}
			float d = 0f;
			if(TargetPos != null)
				d = Vector3.Distance(TargetPos.transform.position, thisTransform.position);
			if(d > AtkRange)
			{
				if(!r)
				{
					Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, rotation, Time.deltaTime * TurnSpeed);
					r = true;
				}
				FindClosestEnemy();
			}
			if(d <= AtkRange)
			{
				FindClosestEnemy();
				if(Target != null)
				{
					var hrotation = Quaternion.LookRotation(TargetPos.transform.position - Head.transform.position);
					Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, hrotation, Time.deltaTime * TurnSpeed);
					//Physics.Raycast(BSpawn.position, BSpawn.forward, out LineOfSight, AtkRange);

					if(wasP)
					{
						nextFireTime = nextFireTime +(Time.time - PausedTime);
						PausedTime = 0;
						wasP = false;
					}
					if (Time.time - ROF > nextFireTime)
							nextFireTime = Time.time - Time.deltaTime;
					
					// Keep firing until we used up the fire time
					while( nextFireTime < Time.time && d <= AtkRange)
					{
						Shoot();
						nextFireTime += ROF;
					
					}
				}
				r = false;
			}
		}
	}
	void Shoot()
	{
		Rigidbody bullet = Instantiate(Bullet, BSpawn.transform.position, BSpawn.transform.rotation) as Rigidbody;
		bullet.GetComponent<ProjectileControl>().SetInfo(Enemy, BSpeed, Damage, GColor, false, Damage);
	}
	void OnCollisionEnter(Collision c)
	{
		var i = c.gameObject.GetComponent<ProjectileControl>();
		float dmg = 0;
		if(i != null)
			dmg = (i.Damage-(Armor*i.Damage));
		if(Enemy)
		{
			if(c.gameObject.tag == "N_Proj")
			{
				//Debug.Log("Damage Delt: " +dmg);
				Health -= dmg;
			}
		}
		if(!Enemy)
		{
			if(c.gameObject.tag == "E_Proj")
			{
				//Debug.Log("Damage Delt: " +dmg);
				Health -= dmg;
			}
		}
	}
	public void RecieveStatus(bool enemy, float armor, float damage, string  name)
	{
		Enemy = enemy;
		Damage = damage;
		Armor = armor;
		Name = name;
		DetermineStatus();
		FindClosestWaypoint();
		FindClosestEnemy();
	}
 	void DetermineCurWayPoint()
	{
		if(Waypoints.Length == 0)
		{
			GetWayPoints();
		}
		distance = Vector3.Distance(Waypoints[CurWp].transform.position, transform.position);
		if(distance <= WpRange)
		{
			int l = Waypoints.Length-1;
			if(Enemy)
			{
				if(CurWp < l)
					CurWp++;
			}
			if(!Enemy)
			{
				if(CurWp > 0)
					CurWp--;
			}
		}
	}
}
