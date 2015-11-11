using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class ServerV2 : MonoBehaviour {

	public string serverIp = "127.0.0.1";
	public int port = 2500;
	public string serverPass = "";
	public int MaxPlayers = 2;
	public GameObject Player;
	//private
	private string curStatus = "Waiting for player input...";
	private bool usePass = false;
	private enum ServerState { Offline, PreGame, InGame, PostGame};
	private ServerState curState = ServerState.Offline;
	private NetworkPeerType peerType;
	private string PlayerName = "player";
	private List<string> PlayerNames = new List<string>();
	private List<NetworkPlayer> NetPlayers = new List<NetworkPlayer>();
	private List<string> ServerLog = new List<string>();
	private bool isJoining = false;
	private bool isHosting = false;
	private bool showIP = false;
	private string curChatMsg = "";
	private Vector2 scroll = Vector2.zero;
	private bool hasSpawned = false;
	void Start()
	{
		Application.runInBackground = true;
		if(!GetComponent<NetworkView>())
		{
			gameObject.AddComponent<NetworkView>();
		}
		peerType = Network.peerType;
	}
	void Update()
	{
		peerType = Network.peerType;
		if(ServerLog.Count > 50)
			ServerLog.RemoveAt(0);
		if(!hasSpawned)
		{
			SpawnPlayer();
		}
	}
	void OnGUI()
	{
		GUILayout.Label("Server State: " + curState);
		GUILayout.Label("Currently: " + curStatus +", " + peerType);
		if(peerType == NetworkPeerType.Disconnected)
		{
			if(!isJoining && !isHosting)
			{
				if(GUILayout.Button("Join a Server"))
				{
					isJoining = true;
				}
				if(GUILayout.Button("Host a Server"))
				{
					isHosting = true;
				}
			}
			showIP = GUILayout.Toggle(showIP, "Show IP");
			if(isJoining)
			{
				GUILayout.Label("Player name:");
				PlayerName = GUILayout.TextField(PlayerName);
				GUILayout.Label("Server IP:");
				serverIp = GUILayout.TextField(serverIp);
				GUILayout.Label("Server Port:");
				port = Convert.ToInt32(GUILayout.TextField(port.ToString()));
				//GUILayout.Label("Server Password: (leave blank if none)");
				//serverPass = GUILayout.PasswordField(serverPass, '*');
				GUI.enabled = CheckInfo();
				if(GUILayout.Button("Connect"))
				{
					curStatus = "connecting to " + serverIp+":"+port;
					Network.Connect(serverIp, port);//, serverPass);
				}
				GUI.enabled = true;
				if(GUILayout.Button("Cancel"))
				{
					isJoining = false;
				}
			}
			if(isHosting)
			{
				GUILayout.Label("Server Port:");
				port = Convert.ToInt32(GUILayout.TextField(port.ToString()));
				GUILayout.Label("Max Players: " + MaxPlayers);
				MaxPlayers = (int)GUILayout.HorizontalSlider(MaxPlayers, 1, 20);
//				usePass = GUILayout.Toggle(usePass, "Password");
//				if(usePass)
//				{
//					GUILayout.Label("Server Password:");
//					serverPass = GUILayout.PasswordField(serverPass, '*');
//				}
				GUI.enabled = CheckInfo();
				if(GUILayout.Button("Start Server"))
				{
					curStatus = "Starting server";
					if(usePass)
					{
						Network.incomingPassword = serverPass;
					}
					Network.InitializeServer(MaxPlayers, port, !Network.HavePublicAddress());
				}
				GUI.enabled = true;
				if(GUILayout.Button("Cancel"))
				{
					isHosting = false;
				}
			}
		}
		if(peerType == NetworkPeerType.Server)
		{
			GUILayout.Label("Current Players: " + NetPlayers.Count);
			foreach(string p in PlayerNames)
			{
				GUILayout.BeginHorizontal();
				if(showIP)
					GUILayout.Label(p + " " + NetPlayers[PlayerNames.IndexOf(p)].ipAddress+":"+NetPlayers[PlayerNames.IndexOf(p)].port);
				else
					GUILayout.Label(p);
				GUILayout.Label(" Ping: " + Network.GetAveragePing(NetPlayers[PlayerNames.IndexOf(p)]));
				if(GUILayout.Button("Kick"))
				{
					AddServerLog(p + " was kicked from the game.");
					Network.CloseConnection(NetPlayers[PlayerNames.IndexOf(p)], true);
				}
				GUILayout.EndHorizontal();
			}
			if(curState == ServerState.PreGame)
			{
				if(GUILayout.Button("Start Game"))
				{
					curState = ServerState.InGame;
					AddServerLog("Game starting...");
				}
			}
			if(curState == ServerState.InGame)
			{
				if(GUILayout.Button("End Game"))
				{
					curState = ServerState.PostGame;
					AddServerLog("Game ending...");
				}
			}
			if(curState == ServerState.PostGame)
			{
				if(GUILayout.Button("Restart Game"))
				{
					curState = ServerState.PreGame;
					AddServerLog("Game restarting...");
				}
			}
			if(GUILayout.Button("Close Server"))
			{
				Network.Disconnect();
				CleanUp();
			}
		}
		if(peerType == NetworkPeerType.Client)
		{
			GUILayout.Label("Current Players: " + PlayerNames.Count);
			foreach(string p in PlayerNames)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(p);
				GUILayout.EndHorizontal();
			}
			if(GUILayout.Button("Disconnect"))
			{
				Network.Disconnect();
			}
		}
		if(peerType == NetworkPeerType.Client || peerType == NetworkPeerType.Server)
		{
			GUILayout.BeginArea(new Rect(0, Screen.height-210, 500, 200));
			GUILayout.BeginHorizontal();
			GUILayout.Space( 5.0F );
			GUILayout.BeginVertical();
			GUILayout.Space( 5.0F );
			scroll = GUILayout.BeginScrollView(scroll);
			foreach(string l in ServerLog)
			{
				GUILayout.Label(l);
			}
			GUILayout.EndScrollView();
			GUILayout.Space( 2.0f);
			GUILayout.EndVertical();
			GUILayout.Space( 0.0f);
			GUILayout.EndHorizontal();
			curChatMsg = GUILayout.TextField(curChatMsg);
			if(Event.current.type == EventType.keyDown && Event.current.character == '\n' && curChatMsg.Length > 0 )
			{
				AddServerChat(PlayerName + ": " + curChatMsg);
				curChatMsg = "";
				scroll.y = 100000;
			}
			GUILayout.EndArea();
			
		}
	}
	void OnPlayerConnected( NetworkPlayer player)
	{
		networkView.RPC("UpdateServerState", RPCMode.AllBuffered, (int)curState);
		if(peerType == NetworkPeerType.Client)
		{
			PlayerNames.Clear();
		}
	}
	void OnDisconnectedFromServer( NetworkDisconnection error)
	{
		curStatus = "Lost connection: "+ error;
		CleanUp();
	}
	void CleanUp()
	{
		curState = ServerState.Offline;
		PlayerNames.Clear();
		ServerLog.Clear();
		NetPlayers.Clear();
		PlayerName = "player";
		curStatus = "waiting for input";
		isHosting = false;
		isJoining = false;
	}
	void OnFailedToConnect( NetworkConnectionError error)
	{
		curStatus = "Failed to connect: " + error;
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		if(peerType == NetworkPeerType.Server && NetPlayers.Contains(player))
		{
			AddServerLog(PlayerNames[NetPlayers.IndexOf(player)] + " left the game.");
			PlayerNames.RemoveAt(NetPlayers.IndexOf(player));
			NetPlayers.Remove(player);
			Network.DestroyPlayerObjects(player);
			Network.RemoveRPCs(player);
		}
	}
	
	void OnConnectedToServer()
	{
		networkView.RPC("PlayerInfo", RPCMode.Server, PlayerName, Network.player);
		if(showIP)
			curStatus = "Connected to: " + Network.player.ipAddress+":"+Network.player.port;
		else
			curStatus = "Connected to server";
	}
	void OnServerInitialized()
	{
		PlayerName = "SERVER";
		curState = ServerState.PreGame;
		curStatus = "Server started, Waiting for connections...";
	}
	bool CheckInfo()
	{
		if(PlayerName.Contains(" ") || PlayerName == "player" && isJoining)
			return false;
		else if(serverIp.Contains(" ") || port <= 0 || serverPass.Contains(" "))
		{
			return false;
		}else
			return true;
	}
	void SendPlayerList()
	{
		networkView.RPC("UpdatePlayerList", RPCMode.AllBuffered, BinarySerialize(PlayerNames));
	}
	string BinarySerialize(List<string> list)
	{
		MemoryStream stream = new MemoryStream();
		BinaryFormatter bFormat = new BinaryFormatter();
		bFormat.Serialize(stream, list);
		string ListData = Convert.ToBase64String(stream.GetBuffer());
		return ListData;
	}
	List<string> BinaryDeserialize(string listdata)
	{
		BinaryFormatter bFormat = new BinaryFormatter();
		MemoryStream ListData = new MemoryStream(Convert.FromBase64String(listdata));
		return (List<string>)bFormat.Deserialize(ListData);
	}
	void AddServerChat(string msg)
	{
		msg = msg.Replace("\n", "");
		if(peerType == NetworkPeerType.Server)
		{
			AddServerLog(msg);
			
		}else
			networkView.RPC("AddChatMSG", RPCMode.Server, msg);
	}
	void AddServerLog(string log)
	{
		ServerLog.Add("> " + log);
		networkView.RPC("UpdateServerLog", RPCMode.All, BinarySerialize(ServerLog));
	}
	void SpawnPlayer()
	{
		//SpawnPlayer
		Debug.Log("Spawning Player");
		camera.enabled = false;
		GameObject player = Network.Instantiate(Player, Vector3.zero, Quaternion.identity, 1) as GameObject;
//		player.GetComponent<PlayerControl>().PlayerInfo(PlayerName);
		AddServerLog(PlayerName + " has spawned");
		hasSpawned = true;
	}
	[RPC]
	void AddChatMSG(string msg)
	{
		ServerLog.Add("> " + msg);
		networkView.RPC("UpdateServerLog", RPCMode.All, BinarySerialize(ServerLog));
	}
	[RPC]
	void PlayerInfo( string name, NetworkPlayer player)
	{
		if(curState == ServerState.PreGame)
		{
			PlayerNames.Add(name);
			NetPlayers.Add(player);
			SendPlayerList();
			AddServerLog(name + " joined the game.");
			networkView.RPC("UpdateServerState", RPCMode.AllBuffered, (int)curState);
		}else
		{
			Network.CloseConnection(player, true);
		}
	}
	[RPC]
	void UpdatePlayerList( string list)
	{
		if(peerType == NetworkPeerType.Client)
		{
			PlayerNames = BinaryDeserialize(list);
		}
	}
	[RPC]
	void UpdateServerState(int state)
	{
		if(peerType == NetworkPeerType.Client)
		{
			curState = (ServerState)state;
		}
	}
	[RPC]
	void UpdateServerLog(string log)
	{
		if(peerType == NetworkPeerType.Client)
		{
			ServerLog = BinaryDeserialize(log);
			scroll.y = 100000;
		}
	}
}
