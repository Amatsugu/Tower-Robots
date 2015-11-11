using UnityEngine;
using System.Collections;

public class HUD_Icon_V2 : MonoBehaviour {
	public string Type;
	public Texture2D[] Icons;
	public string[] Name;
	public GameObject[] Prefab;
	public GameObject Spawner;
	public Camera[] PreviewCamera;
	public Vector2 RenderPos;
	public float[] Damage;
	public float[] Armor;
	public int[]Cost;
	public int[] Level;
	public bool isEnemy;
	public Rect PreviewWindow;
	public Rect MainButton;
	public Texture2D MainIcon;
	public Rect StartPoint;
	public Rect IconSize;
	public int MaxCol = 5;
	public float Padding = 7.3f;
	public GUISkin Skin;
	public GUISkin Skin2;
	public bool Draw;
	
	
	//private
	private int Index;
	private bool Preview;
	private float initMB;
	private float initPW;
	// Use this for initialization
	void Start () 
	{
		initMB = MainButton.y;
		initPW = PreviewWindow.y;
		RecalculatePos();
	}
	void RecalculatePos()
	{
		MainButton.y = Screen.height - (initMB + MainButton.height);
		PreviewWindow.y = Screen.height - (initPW + PreviewWindow.height);
		foreach( Camera c in PreviewCamera)
		{
			c.pixelRect = new Rect(((PreviewWindow.x + PreviewWindow.width * .17f)) + RenderPos.x,((Screen.height - PreviewWindow.y) - PreviewWindow.height) - RenderPos.y, c.pixelRect.width, c.pixelRect.height);
		}
		
	}
	void OnGUI()
	{
		RecalculatePos();
		if(PauseManager.inst.isPasued)
			GUI.enabled = false;
		else
			GUI.enabled = true;
		GUI.skin = Skin;
		if(GUI.Button(MainButton, MainIcon))
		{
			if(Draw)
				Draw = false;
			else if(!Draw)
				Draw = true;
			if(Preview)
				Preview = false;
		}
		if(Draw)
		{
			int Col = 0;
			int Row = 0;
			int Count = 0;
			if(Count <= Icons.Length-1)
			{
				foreach(Texture2D ico in Icons)
				{
					if(GUI.Button(new Rect( StartPoint.x +((IconSize.width + Padding) *Col), Screen.height - (StartPoint.y +((IconSize.height+ Padding)*Row)), IconSize.width, IconSize.height), ico))
					{
						Preview = true;
						Index = Count;
					}
					Col++;
					if(Col > MaxCol)
					{
						Col = 0;
						Row++;
					}
					Count++;
				}
			}
			if(Input.GetKeyDown(KeyCode.Mouse1))
			{
				Draw = false;
				if(Preview)
					Preview = false;
			}
		}
		if(Preview)
		{
			GUI.Window(2,PreviewWindow , PreviewWin, Type + " Info");
		}
		if(!Preview)
		{
			if(Input.GetKeyUp((KeyCode)PlayerPrefs.GetInt("QuickBuild")))
			{
				if(CreditsManager.inst.CurCredits >= Cost[Index])
					Buy(Index);
				else
					audio.Play();
			}
		}
		if(Event.current.isKey)
		{
			if(Event.current.keyCode == KeyCode.Alpha1)
				ShowItem(0);
			if(Event.current.keyCode == KeyCode.Alpha2)
				ShowItem(1);
			if(Event.current.keyCode == KeyCode.Alpha3)
				ShowItem(2);
		}
		if(PauseManager.inst.isPasued)
		{
			Preview = false;
			Draw = false;
		}
	}
	void PreviewWin(int windowID)
	{
		GUI.depth = -1;
//		if(!PreviewWindow.Contains(Input.mousePosition))
//		{
//			if(Input.GetKeyDown(KeyCode.Mouse0))
//				Preview = false;
//		}
		GUILayout.Space(25);
		GUILayout.Label(Name[Index]);
		GUILayout.Label("Cost: " + Cost[Index]+"c");
		GUILayout.Label("Armor: " + (int)(Armor[Index]*100)+"%");
		GUILayout.Label("Damage: " + Damage[Index]);
		GUI.skin = Skin2;
		if(CreditsManager.inst.CurCredits < Cost[Index])
		{
			GUI.enabled = false;
		}else
		{
			GUI.enabled = true;
		}
		if(GUILayout.Button("Buy: " + Cost[Index] + "c"))
		{
			Preview = false;
			Buy(Index);
		}
		if(CreditsManager.inst.CurCredits >= Cost[Index] && Input.GetKeyUp(KeyCode.B))
		{
			Preview = false;
			Buy(Index);
		}
		if(Event.current.type == EventType.Repaint)
		{
			PreviewCamera[Index].Render();
		}
	}
	void ShowItem(int index)
	{
		Preview = true;
		if(!Draw)
			Draw = true;
		Index = index;
	}
	void Buy(int index)
	{
		if(!GameObject.FindGameObjectWithTag("ObjectBuilder"))
		{
			RaycastHit hit;
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
			GameObject clone = Instantiate(Spawner, new Vector3(hit.point.x, 7, hit.point.z), Quaternion.identity) as GameObject;
			clone.GetComponent<ObjectSpawner>().ReciveInfo(Prefab[index], Type, Cost[index], isEnemy, Armor[index], Damage[index], Name[index], Cost[index] * .9f - Random.Range(0, 10), Level[index]);
		}
	}
}
