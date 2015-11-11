using UnityEngine;
using System.Collections;

public class Updater : MonoBehaviour {
	public GUISkin Skin;
	public float height = 50;
	public float width = 150;
	public static bool isLatestVer;
	private string[] curVersionInfo;
	public bool showWindow;
	public bool showError;
	private WWW ver;
	private string[] gotVersionInfo;
	private bool Active;
	void OnMouseEnter()
	{
		//guiText.fontStyle = FontStyle.Bold;
		guiText.material.color = Color.gray;
		Active = true;
	}
	void OnMouseExit()
	{
		//guiText.fontStyle = FontStyle.Normal;
		guiText.material.color = Color.white;
		Active = false;
	}
	IEnumerator Start()
	{
		if(Application.isWebPlayer)
			Destroy(gameObject);
		curVersionInfo = ChangeLog.Build.Split( );
		Debug.Log(curVersionInfo[0] + "/" + curVersionInfo[1]);
		ver = new WWW("http://towerrobots.webs.com/Version/Version.txt");
		yield return ver;
		if(ver.error != null && ver.isDone)
		{
			Debug.LogError(ver.error);
			DebugConsole.LogError(ver.error);
			showError = true;
			showWindow = true;
			Destroy(gameObject);
		}
		if(ver.error == null && ver.isDone)
		{
			gotVersionInfo = ver.text.Split( );
			DebugConsole.Log(gotVersionInfo[0] + "/" + gotVersionInfo[1]);
			if(curVersionInfo[0] == gotVersionInfo[0] && float.Parse(curVersionInfo[1]) >= float.Parse(gotVersionInfo[1]))
			{
				//Debug.Log("This is the latest version: " + ChangeLog.Build);
				isLatestVer = true;
				showWindow = false;
			}else
			{
				//Debug.Log("New Version: " + ver.text);
				isLatestVer = false;
				showWindow = true;
			}
		}
	}
	IEnumerator CheckVersion()
	{
		curVersionInfo = ChangeLog.Build.Split( );
		Debug.Log(curVersionInfo[0] + "/" + curVersionInfo[1]);
		ver = new WWW("http://towerrobots.webs.com/Version/Version.txt");
		yield return ver;
		if(ver.error != null)
		{
			Debug.LogError(ver.error);
			DebugConsole.LogError(ver.error);
			showError = true;
			showWindow = true;
		}
		if(ver.error == null)
		{
			gotVersionInfo = ver.text.Split( );
			DebugConsole.Log(gotVersionInfo[0] + "/" + gotVersionInfo[1]);
			if(curVersionInfo[0] == gotVersionInfo[0] && float.Parse(curVersionInfo[1]) >= float.Parse(gotVersionInfo[1]))
			{
				//Debug.Log("This is the latest version: " + ChangeLog.Build);
				isLatestVer = true;
				showWindow = false;
			}else
			{
				//Debug.Log("New Version: " + ver.text);
				isLatestVer = false;
				showWindow = true;
			}
		}
	}
	void Update()	
	{
		if(Active)
		{
			if(Input.GetKeyUp(KeyCode.Mouse0))
			{
				CheckVersion();
				if(ver.error != null)
				{
					Debug.LogError(ver.error);
					DebugConsole.LogError(ver.error);
					showError = true;
					showWindow = true;
				}else
				{
					showWindow = true;
				}
			}
		}
	}
	void OnGUI()
	{
		GUI.skin = Skin;
		string windowName;
		if(showWindow && ver.text != null)
		{
			if(isLatestVer)
			{
				windowName = "No update Avilable!";
			}else
			{
				windowName = "Version " + ver.text + " available!";
			}
			if(showError)
				windowName = "Error!";
			GUI.Window(0, new Rect((Screen.width/2)-(width/2), (Screen.height/2)-(height/2), width, height), UpdateWindow, windowName);
		}
	}
	void UpdateWindow(int windowID)
	{
		//GUILayout.Space(25f);
		if(!isLatestVer && !showError)
		{
			GUI.Label(new Rect((width/2)- 145, 25, 290, 40),"There is a new version of Tower Robots Availabe: " + ver.text + ".");
			GUI.Label(new Rect((width/2)- 145, 65, 290, 20),"You can get it here:");
			if(GUI.Button(new Rect((width/2) - 145, 85, 290, 20),"http://towerrobots.webs.com"))
			{
				Application.OpenURL("http://towerrobots.webs.com/apps/forums/show/7060153-releases");
			}
		}
		if(isLatestVer && !showError)
		{
			GUI.Label(new Rect((width/2)- 145, 25, 290, 40), "This is the latest version: " + curVersionInfo[0] + " " + curVersionInfo[1] + ".");
			GUI.Label(new Rect((width/2)- 145, 65, 290, 20), "There is no availbe update.");
		}
		if(showError)
		{
			GUI.Label(new Rect((width/2)- 145, 25, 290, 20),"There was a problem connecting to the server.");
			GUI.Label(new Rect((width/2)- 145, 45, 290, 60),"Details: " + ver.error);
		}
		if(GUI.Button(new Rect((width/2) - 47.5f, height - 30, 95, 20), "Close"))
		{
			showWindow = false;
			if(showError)
				showError = false;
		}
	}
}
