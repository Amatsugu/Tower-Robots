using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour {
	
	public float LifeTime = 2;
	public bool DestroyDrop;
	public GameObject Drop;
	public bool ColorDrop;
	public Color ObjColor;
	//private
	private bool wasP = false;
	private float PausedTime;
	private float StartTime;
	private PauseManager PauseM;
	void Start()
	{
		wasP = false;
		PauseM = PauseManager.inst;
		PausedTime = 0f;
		LifeTime += Time.time;
		StartTime = Time.time;
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
	public void DestroyMe()
	{
		if(DestroyDrop)
		{
			GameObject Clone = Instantiate(Drop, transform.position, Quaternion.identity) as GameObject;
			if(ColorDrop)
			{
				Clone.GetComponent<Detonator>().color = ObjColor;
			}
		}
		Destroy(gameObject);
	}
}
