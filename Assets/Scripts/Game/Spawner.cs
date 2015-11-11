using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject Object;
	public float StartDelay = 5;
	public float WaveRate = 5;
	public float SpawnRate = 1;
	public float Multiplier = 2;
	public bool Enemy;
	public float DamageLimit = 10;
	public int MaxWave = 20;
	public int PerWaveEnemyMod = 1;
	public int EnemiesPerWave = 10;
	public float DmgModLimit = 3;
	public float CurDmgMod = 0;
	public float HealthLimit = 200;
	public float HealthModLimit = 7;
	public float CurHealthMod = 0;
	public float LootModLimit = 60;
	public float CurLootMod = 0;
	public Texture2D VictorySplash;
	public GameObject WaveIndicator;
	public GUISkin Skin;
	public Texture2D BG;
	public float CountdownYOffset;
	//pruvate
	private string Tgt;
	private float LastWaveTime = 0f;
	private float LastWave = 0;
	private int CurWave = 1;
	private int SpawnedOfWave = 0;
	private bool wasP = false;
	private float PausedTime;
	private float NextWaveStart;
	private bool Spawning = true;
	private bool s = false;
	private float StartTime;
	private bool HasStarted = false;
	private float curTime;
	private bool isShowing = true;
	void Start()
	{
		if(Enemy)
			Tgt = "E_Creep";
		if(!Enemy)
			Tgt = "N_Creep";
		if(DifficultyLoader.Instance != null)
		{
			if(DifficultyLoader.Instance.WasSet)
			{
				Multiplier = DifficultyLoader.Instance.Modifier;	
				MaxWave = DifficultyLoader.Instance.Waves;
			}
		}
		StartDelay += Time.time;
		StartTime = Time.time;
	}
	// Update is called once per frame
	void OnGUI()
	{
		if(Application.isEditor & isShowing)
		{
			Spawning = GUI.Toggle(new Rect(Screen.width - 90, 20, 90, 20), Spawning, "Spawning");
			if(StartDelay >= curTime)
			{
				GUI.Label(new Rect(20, 40, 200, 20), "Game Starts in: " + (StartDelay-curTime));
			}
			if(s)
			{
				GUI.Label(new Rect(20, 40, 200, 20), "Next Wave Starts in: " + (NextWaveStart-curTime));
			}
		}
		GUI.skin = Skin;
		if(!PauseManager.inst.isPasued)
		{
			if(StartDelay >= curTime)
			{
				GUI.DrawTexture(new Rect((Screen.width/2)-BG.width/2, CountdownYOffset, BG.width , BG.height), BG);
				GUI.Label(new Rect((Screen.width/2)-(Screen.width/2), CountdownYOffset+35, Screen.width, 20), "Game Starts in: " + (int)(StartDelay-curTime));
			}
			if(s)
			{
				GUI.DrawTexture(new Rect((Screen.width/2)-BG.width/2, CountdownYOffset, BG.width , BG.height), BG);
				GUI.Label(new Rect((Screen.width/2)-(Screen.width/2), CountdownYOffset+35, Screen.width, 20), "Next Wave Starts in: " + (int)(NextWaveStart-curTime));
			}
		}
	}
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.F1) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
		{
			isShowing = !isShowing;
		}
		curTime = Time.time;
		if(PauseManager.inst.isPasued && Spawning)
		{
			wasP = true;
			PausedTime = curTime - StartTime;
		}
		if(!PauseManager.inst.isPasued && Spawning)
		{
			if(wasP)
			{
				if(!HasStarted)
					StartDelay += PausedTime;
				PausedTime = 0;
				wasP = false;
			}
			if(Input.GetKeyUp(KeyCode.Escape) && !wasP)
				StartTime = Time.time;
			if(StartDelay <= curTime)
			{
				HasStarted = true;
				WaveIndicator.guiText.text = "Wave: " + CurWave + " of " + MaxWave;
				if(!GameObject.FindWithTag(Tgt) && SpawnedOfWave == EnemiesPerWave)
				{
					if(wasP)
					{
						NextWaveStart += PausedTime;
						PausedTime = 0;
						wasP = false;
					}
					if(!s)
					{
						LastWaveTime = curTime;
						s = true;
					}
					BuffWave((float)CurWave/(float)MaxWave);
					//Debug.Log("Time: " + curTime);
					if(NextWaveStart <= curTime)
					{
						//Debug.Log("Proceding");
						EnemiesPerWave += PerWaveEnemyMod;
						SpawnedOfWave = EnemiesPerWave;
						CurWave++;
						SpawnedOfWave = 0;
						s = false;
					}
				}
				if(CurWave > LastWave)
				{
					if(SpawnedOfWave < EnemiesPerWave)
					{
						if(wasP)
						{
							LastWaveTime += PausedTime;
							PausedTime = 0;
							wasP = false;
						}
						
						if(LastWaveTime < curTime - (SpawnRate))
						{
							SpawnEnemy();
							SpawnedOfWave++;
						}
					}
				}
			}
		}
		if(CurWave >= MaxWave && !GameObject.FindWithTag(Tgt))
		{
			ShowEndScreen();
			
		}
	}
	void ShowEndScreen()
	{
		PauseManager.inst.isPasued = true;
		PauseManager.inst.CanPause = false;
		Screen.lockCursor = false;
		gameObject.AddComponent("GUITexture");
		gameObject.guiTexture.enabled = false;
		gameObject.guiTexture.texture = VictorySplash;
		gameObject.transform.position = new Vector3(.5f, .5f , 9999);
		gameObject.guiTexture.enabled = true;
		if(Input.GetKeyUp(KeyCode.Space))
		{
			if(StatsManager.AddWin())
				Application.LoadLevel(0);
		}
	}
	void SpawnEnemy()
	{
		GameObject Clone = Instantiate(Object, transform.position, transform.rotation) as GameObject;
		LastWaveTime = curTime;
		Clone.GetComponent<CreepAI>().RecieveBuff(Enemy, CurDmgMod, CurHealthMod, CurLootMod);
	}
	void BuffWave(float mod)
	{
		if(CurWave > LastWave)
		{
			if((DamageLimit*(mod))* Multiplier <= DmgModLimit)
				CurDmgMod += (DamageLimit*(mod))* Multiplier;
			else
				CurDmgMod += DmgModLimit;
			if((HealthLimit*(mod))* Multiplier <= HealthModLimit)
				CurHealthMod += (HealthLimit*(mod))* Multiplier;
			else
				CurHealthMod += HealthModLimit;
			if((LootModLimit*(mod)) * Multiplier <= LootModLimit)
				CurLootMod += (LootModLimit*(mod)) * Multiplier;
			else
				CurLootMod += LootModLimit;
			LastWave = CurWave;
			NextWaveStart = LastWaveTime + WaveRate;
			Debug.Log("NextWave: " + NextWaveStart);
			Debug.Log("Buffed!");
		}
	}
}
