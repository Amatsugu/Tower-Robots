using UnityEngine;
using System.Collections;

public class KongregateAPI : MonoBehaviour {
//	public static bool isKongregate = false;
//	private int userId = 0;
//	public static string username = "Guest";
//	private string gameAuthToken = "";
//	private bool isShowing = false;
//	private string PasswordFeild = "";
//	private bool x = false;
//	// Use this for initialization
//	void Start () 
//	{
//		Application.ExternalEval("if(typeof(kongregateUnitySupport) != 'undefined'){" +" kongregateUnitySupport.initAPI('LoadingScreen', 'OnAPILoaded');" +"};");
//		DebugConsole.Log("Called Kongregate API, waiting for responce...");
//	}
//	void OnAPILoaded(string userInfoString)
//	{
//		Debug.Log("Sucess");
//		DebugConsole.Log("Loaded the Kongregate API");
//		// We now know we're on Kongregate
//		isKongregate = true;
//		// Split the user info up into tokens
//		var Params = userInfoString.Split("|"[0]);
//		userId = int.Parse(Params[0]);
//		username = Params[1];
//		gameAuthToken = Params[2];
//		Options_GamePlay.SetPlayerName(username);
//	}
//	// Update is called once per frame
//	void OnGUI()
//	{
//		if(!isKongregate)
//			GUILayout.Label("Unable to load API, are you on Kongregate?");
//		if(isShowing && Application.loadedLevel == 0)
//		{
//			if(PasswordFeild != "TDVDevPass614")
//				PasswordFeild = GUI.TextField(new Rect(5, 5, 100, 20),PasswordFeild);
//			if(PasswordFeild == "TDVDevPass614")
//			{
//				if(isKongregate)
//				{
//					GUILayout.Label("Current User: " + username);
//					GUILayout.Label("UserID: " + userId);
//					GUILayout.Label("Auth Token: " + gameAuthToken);
//				}else
//				{
//					GUILayout.Label("Unable to load API");
//				}
//			}
//		}
//	}
//	void Update()
//	{
//		if(Input.GetKeyUp(KeyCode.F1) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
//		{
//			isShowing = !isShowing;
//		}
//		if(Time.time >= 20)
//		{
//			if(!x)
//			{
//				DebugConsole.Log("Failed to connect to Konregate or it's taking too long.");
//				x = true;
//			}
//		}
//	}
//	public static void SubmitStat(string StatName, int Value)
//	{
//		if(isKongregate)
//		{
//			Application.ExternalCall("kongregate.stats.submit",StatName, Value);
//			DebugConsole.Log("Submitting stat: " + StatName + " with value: " + Value);
//		}
//	}
}
