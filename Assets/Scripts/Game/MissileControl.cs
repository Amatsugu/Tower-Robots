using UnityEngine;
using System.Collections;

public class MissileControl : MonoBehaviour {
	public float Delay = 2;
	public GameObject DamageTxt;
	public ParticleSystem[] ParticleSystems;
	private GameObject[] Hits;
	private Transform Target;
	private float StartTime;
	private float PausedTime;
	private bool wasP;
	private bool r;
	private float SecForce;
	private float Radius;
	private float Damage;
	private bool locked;
	private Vector3 PrePauseVel;
	private Vector3 PrePauseAngVel;
	// Use this for initialization
	void Start () 
	{
		StartTime = Time.time;
		Delay+= Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float curTime = Time.time;
		if(!PauseManager.inst.isPasued)
		{
			if(wasP)
			{
				Delay += PausedTime;
				PausedTime = 0;
				audio.Play();
				if(ParticleSystems.Length >= 1)
				{
					foreach( ParticleSystem p in ParticleSystems)
					{
						p.Play();
					}
				}
				wasP = false;
			}
			if(Input.GetKeyUp(KeyCode.Escape))
				StartTime = curTime;
			if(Delay <= curTime)
			{
				if(!locked)
				{
					if(Target == null)
					{
						string tTag = "";
						if(gameObject.tag == "E_Proj")
							tTag = "N_Creep";
						if(gameObject.tag == "N_Proj")
							tTag = "E_Creep";
						Target = GameObject.FindGameObjectsWithTag(tTag)[Random.Range(0, GameObject.FindGameObjectsWithTag(tTag).Length -1)].transform;
					}
					transform.LookAt(Target.transform.position);
					rigidbody.velocity = Vector3.zero;
					if(!gameObject.GetComponent<ConstantForce>())
						gameObject.AddComponent<ConstantForce>();
					gameObject.GetComponent<ConstantForce>().relativeForce = Vector3.forward * (SecForce*.5f);
					locked = true;
				}
			}
		}
		if(PauseManager.inst.isPasued)
		{
			wasP = true;
			PausedTime = curTime - StartTime;
			audio.Pause();
			if(ParticleSystems.Length >= 1)
			{
				foreach( ParticleSystem p in ParticleSystems)
				{
					p.Pause();
				}
			}
		}
	}
	void FixedUpdate()
	{
		if(PauseManager.inst.isPasued)
		{
			if(!r)
			{
				if(gameObject.GetComponent<ConstantForce>())
				{
					gameObject.GetComponent<ConstantForce>().relativeForce = Vector3.zero;
				}
				PrePauseVel = rigidbody.velocity;
				PrePauseAngVel = rigidbody.angularVelocity;
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.isKinematic = true;
				r = true;
			}
		}
		if(!PauseManager.inst.isPasued)
		{
			if(r)
			{
				rigidbody.isKinematic = false;
				rigidbody.velocity = PrePauseVel;
				rigidbody.angularVelocity = PrePauseAngVel;
				if(gameObject.GetComponent<ConstantForce>())
				{
					gameObject.GetComponent<ConstantForce>().relativeForce = Vector3.forward * (SecForce*.5f);
				}
				r = false;
			}
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		Detonate();
	}
	void Detonate()
	{
		string tTag = "";
		if(gameObject.tag == "E_Proj")
			tTag = "N_Creep";
		if(gameObject.tag == "N_Proj")
			tTag = "E_Creep";
		Hits = GameObject.FindGameObjectsWithTag(tTag);
		foreach(GameObject h in Hits)
		{
			float distance = Vector3.Distance(h.transform.position, transform.position);
			if( distance <= Radius)
			{
				float dmg = Damage -(Damage *(distance/Radius));
				h.GetComponent<CreepAI>().DealDamage(dmg);
				GameObject dmgtxt = Instantiate(DamageTxt,h.transform.position, Quaternion.identity) as GameObject;
				dmgtxt.GetComponent<DMGText>().SetInfo(((int)(dmg-(h.GetComponent<CreepAI>().Armor*dmg))).ToString() , new Color(1, .8f, 0, 1));
			}
		}
		gameObject.GetComponent<ObjectDestroyer>().DestroyMe();
	}
	public void SetInfo(float radius, float force, float secForce, float damage, Transform target)
	{
		rigidbody.AddForce(transform.forward * force);
		Target = target;
		Radius = radius;
		Damage = damage;
		SecForce = secForce;
	}
}
