using UnityEngine;
using System.Collections;

public class TowerAI : MonoBehaviour {
	//Vars
	//Public
	public GameObject Head;
	public GameObject Ragdoll;
	public GameObject Explosion;
	public bool Animate;
	public GameObject AnimatedObj;
	public bool ShotAnimate;
	public float ParticleShotDelay;
	public float TurnSpeed = 5f;
	public float AtkRange = 100f;
	public float MaxRange = 40;
	public float RangeCost = 500;
	public bool Enemy = false;
	public Rigidbody Bullet;
	public bool isMissile;
	public float ExplosiveRadius = 5;
	public float BSpeed = 3000;
	public float SecondarySpeed = 500;
	public Transform[] BSpawn;
	public float ROF = 1;
	public float MaxROF = .1f;
	public float ROFCost = 1150;
	public float Health = 100;
	public float Armor = .20f;
	public float MaxArmor = .5f;
	public float ArmorCost = 750;
	public float Damage = 10;
	public float MaxDamage = 25;
	public float DamageCost = 1050;
	public string Name;
	public int Level;
	public int MaxLevel = 10;
	public float SellPrice;
	public bool AutoRepair;
	public float RepairRate = 5f;
	public bool Crits;
	public bool Looting;
	public Transform HealthBarContainer;
	public Transform HealthBar;
	public Transform HealthText;
	public float HealthBarOffset;
	//Private
	private float nextFireTime;
	private float nextRepairTime;
	private GameObject Target;
	private Transform TargetPos;
	private string Tgt;
	private Color GColor;
	private bool wasP;
	private float PausedTime;
	private Transform thisTransform;
	private float HealthBarWidth;
	private Vector3 DesiredBarPos;
	private bool MultiSpawn = false;
	private int curSpawn = 0;
	private float NextParticleShotTime;
	// Use this for initialization
	void Start() {
		DetermineStatus();
		FindClosestEnemy();
		if(Armor > 1)
			Armor = 1;
		if(Armor < 0)
			Armor = 0;
		RaycastHit hit;
		Vector3 Down = new Vector3(0, -1, 0);
		Physics.Raycast(transform.position, Down, out hit, 5);
		hit.transform.gameObject.tag = "InValid";
		HealthBarWidth = HealthBar.guiTexture.pixelInset.width;
		if(BSpawn.Length > 1)
			MultiSpawn = true;
		nextFireTime = Time.time + ParticleShotDelay;
	}
	void DetermineStatus()
	{
		if(Enemy)
		{
			Tgt = "N_Creep";
			gameObject.tag = "E_Tower";
			//gameObject.renderer.material.color = Color.red;
			GColor = Color.red;
		}
		if(!Enemy)
		{
			Tgt = "E_Creep";
			gameObject.tag = "N_Tower";
			//gameObject.renderer.material.color = Color.blue;
			GColor = Color.blue;
		}
		DebugConsole.Log(Tgt);
	}
	public void Sell(int price)
	{
		CreditsManager.inst.AddCredits(price);
		RaycastHit hit;
		Vector3 Down = new Vector3(0, -1, 0);
		Physics.Raycast(transform.position, Down, out hit, 5);
		hit.transform.gameObject.tag = "T_Valid";
		Destroy(gameObject);
	}
	public void Repair(int price)
	{
		if(CreditsManager.inst.CurCredits >= price)
		{
			CreditsManager.inst.RemoveCredits(price);
			Health = 100;
		}
	}
	public void RecieveStatus(bool enemy, float armor, float damage, string name, float sellPrice, int level)
	{
		//Debug.Log("Recived to tower.");
		Enemy = enemy;
		Armor = armor;
		Damage = damage;
		Name = name;
		SellPrice = sellPrice;
		Level = level;
		FindClosestEnemy();
	}
	// Update is called once per frame
	void Update () 
	{
		if(PauseManager.inst.isPasued)
		{
			wasP = true;
			PausedTime = Time.time;
		}
		if(!PauseManager.inst.isPasued)
		{
			thisTransform = transform;
			DesiredBarPos = Camera.main.WorldToViewportPoint(new Vector3(thisTransform.position.x, thisTransform.position.y, thisTransform.position.z));
			DesiredBarPos = new Vector3(DesiredBarPos.x, DesiredBarPos.y + HealthBarOffset, 0);
			HealthBarContainer.transform.position = new Vector3(DesiredBarPos.x, DesiredBarPos.y, -100);
			HealthBar.transform.position = DesiredBarPos;
			HealthText.transform.position = DesiredBarPos;			
			HealthText.guiText.text = Name + " Lvl " + Level;
			HealthBar.guiTexture.pixelInset = new Rect(HealthBar.guiTexture.pixelInset.x, HealthBar.guiTexture.pixelInset.y, HealthBarWidth*(Health/100), HealthBar.guiTexture.pixelInset.height);
			if(Health <= 0)
			{
				RaycastHit hit;
				Vector3 Down = new Vector3(0, -1, 0);
				Physics.Raycast(thisTransform.position, Down, out hit, 5);
				hit.collider.tag = "T_Valid";
				Instantiate(Ragdoll, thisTransform.position, Quaternion.identity);
				//Clone.renderer.material.color = gameObject.renderer.material.color;
				GameObject Exp = Instantiate(Explosion, thisTransform.position, Quaternion.identity) as GameObject;
				Exp.GetComponent<Detonator>().color = GColor;
				StatsManager.AddTowerStat(1);
				Destroy(gameObject);
			}
			if(AutoRepair)
			{
				if(wasP)
				{
					nextRepairTime = nextRepairTime +(Time.time - PausedTime);
					PausedTime = 0;
					wasP = false;
				}
				if (Time.time - RepairRate > nextFireTime)
					nextRepairTime = Time.time - Time.deltaTime;
			
				// Keep firing until we used up the fire time
				while( nextRepairTime < Time.time)
				{
					if(Health < 90)
					{
						Health += 10;
					}
					nextRepairTime += RepairRate;
				}
			}
			if(TargetPos != null || Target != null)
			{
				var d = Vector3.Distance(TargetPos.transform.position, transform.position);
				if(d <= AtkRange)
				{
					var rotation = Quaternion.LookRotation(TargetPos.transform.position - Head.transform.position);
					Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, rotation, Time.deltaTime * TurnSpeed);
					//RaycastHit LineOfSight;
					//Physics.Raycast(thisTransform.position, thisTransform.forward, out LineOfSight, AtkRange);
					if(wasP)
					{
						nextFireTime = nextFireTime +(Time.time - PausedTime);
						NextParticleShotTime = NextParticleShotTime + (Time.time - PausedTime);
						PausedTime = 0;
						wasP = false;
					}
					if (Time.time - ROF > nextFireTime)
						nextFireTime = Time.time - Time.deltaTime;
					if (Time.time - ParticleShotDelay > NextParticleShotTime)
						NextParticleShotTime = Time.time - Time.deltaTime;
					
					while( NextParticleShotTime < Time.time && d <= AtkRange)
					{
						if(BSpawn[0].GetComponent<ParticleEmitter>())
						{
							BSpawn[curSpawn].particleEmitter.Emit();
						}
						if(BSpawn[0].GetComponent<ParticleSystem>())
						{
							BSpawn[curSpawn].particleSystem.Play();
						}
						NextParticleShotTime += ROF;
					}
					while( nextFireTime < Time.time && d <= AtkRange)
					{
						audio.Play();
						if(ShotAnimate)
							BSpawn[curSpawn].animation.Play();
						Shoot();
						nextFireTime += ROF + ParticleShotDelay;
					
					}
				}
				if(d > AtkRange)
					FindClosestEnemy();
			}
		}
	}
	void Shoot()
	{
		float dmg = 0;
		dmg = 0;
		if(Crits)
		{
			dmg = Damage + Random.Range(0, 10);
		}else
		{
			dmg = Damage;
		}
		if(Animate)
		{
			AnimatedObj.animation.Play();
		}
		Rigidbody bullet = Instantiate(Bullet, BSpawn[curSpawn].transform.position, BSpawn[curSpawn].transform.rotation) as Rigidbody;
		if(!isMissile)
			bullet.GetComponent<ProjectileControl>().SetInfo(Enemy, BSpeed, dmg, GColor, Looting, Damage);
		else
			bullet.GetComponent<MissileControl>().SetInfo(ExplosiveRadius, BSpeed, SecondarySpeed, dmg, TargetPos);
		if(MultiSpawn)
		{
			curSpawn++;
			if(curSpawn > BSpawn.Length -1)
				curSpawn = 0;
		}
		//bullet.AddForce(Head.transform.forward * BSpeed);	
	}
	void LateUpdate()
	{
		bool r = false;
		if(r)
		{
			DetermineStatus();
			r = true;
		}
		if(!PauseManager.inst.isPasued)
		{
			if(Target == null || TargetPos == null)
				FindClosestEnemy();
			if(isMissile)
				FindClosestEnemy();
		}
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
				StatsManager.AddDamageRecieved((int)dmg);
			}
		}
	}
	void FindClosestEnemy() {
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
                distance = curDistance;
            }
        }
		if(Target != null)
			TargetPos = Target.transform;
    }
}
