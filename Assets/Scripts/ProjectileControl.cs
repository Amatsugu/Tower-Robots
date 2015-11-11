using UnityEngine;
using System.Collections;

public class ProjectileControl : MonoBehaviour {
	//vars
	//public
	public GameObject HitObj;
	public float LifeTime = 2f;
	public float Damage;
	public Material red, blue;
	public GameObject damageText;
	private Vector3 PrePauseVel;
	private Vector3 PrePauseAngVel;
	private bool wasP = false;
	private bool r = false;
	private float PausedTime;
	private float StartTime;
	private PauseManager PauseM;
	private Color tColor;
	private bool Looting;
	private float defaultDMG;
	void Start()
	{
		wasP = false;
		PausedTime = 0f;
		LifeTime += Time.time;
		StartTime = Time.time;
		PauseM = PauseManager.inst;
		//Debug.Log("Start Time: " + StartTime);
	}
	void Update () 
	{
		float curTime = Time.time;
		if(!PauseM.isPasued)
		{
			if(wasP)
			{
				LifeTime += PausedTime;
				//Debug.Log("Time Paused: " + PausedTime);
				//Debug.Log("LifeTime On Resume: " + LifeTime);
				PausedTime = 0;
				wasP = false;
			}
			if(Input.GetKeyUp(KeyCode.Escape) && !wasP)
			{
				StartTime = Time.time;
			}
			if(LifeTime <= curTime)
			{
				DestroyMe();
			}
		}
		if(PauseM.isPasued)
		{
			wasP = true;
			PausedTime = curTime - StartTime;
		}
	}
	void DestroyMe()
	{
		Destroy(gameObject);
	}
	void FixedUpdate()
	{
		if(PauseM.isPasued)
		{
			if(!r)
			{
				PrePauseVel = rigidbody.velocity;
				PrePauseAngVel = rigidbody.angularVelocity;
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				r = true;
			}
		}
		if(!PauseM.isPasued)
		{
			if(r)
			{
				rigidbody.velocity = PrePauseVel;
				rigidbody.angularVelocity = PrePauseAngVel;
				r = false;
			}
		}
	}
	public void SetInfo(bool e, float s, float d, Color color, bool looting, float defaultDmg)
	{
		var x = gameObject.GetComponent<TrailRenderer>();
		tColor = color;
		if(x != null)
		{
			if(e)
			{
				gameObject.tag = "E_Proj";
				x.material = red;
			}
			if(!e)
			{
				gameObject.tag = "N_Proj";
				x.material = blue;
			}
			gameObject.renderer.material.color = color;
		}
		if(x == null)
		{
			//gameObject.AddComponent("renderer");
			if(e)
			{
				gameObject.tag = "E_Proj";
				gameObject.renderer.material = red;
			}
			if(!e)
			{
				gameObject.tag = "N_Proj";
				gameObject.renderer.material = blue;
			}
		}
		rigidbody.AddForce(transform.forward * s);
		Damage = d;
		defaultDMG = defaultDmg;
		Looting = looting;
		
	}
	void OnCollisionEnter(Collision collision)
	{
		Color txtcol = Color.red;
		if(Damage > defaultDMG)
			txtcol = new Color(1, .1f, 0, 1);
		else
			txtcol = new Color(1, .8f, 0, 1);
		GameObject Clone = Instantiate(HitObj, transform.position, transform.rotation) as GameObject;
		Clone.GetComponent<Detonator>().color = tColor;
		
		string tTag = "";
		if(gameObject.tag == "E_Proj")
			tTag = "N_Creep";
		if(gameObject.tag == "N_Proj")
			tTag = "E_Creep";
		if(collision.collider.tag == tTag)
		{
			GameObject DmgTxt = Instantiate(damageText, collision.contacts[0].point, transform.rotation) as GameObject;
			DmgTxt.GetComponent<DMGText>().SetInfo((Damage-(Damage*collision.gameObject.GetComponent<CreepAI>().Armor)).ToString(), txtcol);
		}
		if(Looting && collision.collider.tag == tTag)
		{
			CreditsManager.inst.AddCredits(Random.Range(10, 100)); 
		}
		Destroy(gameObject);
	}
}
