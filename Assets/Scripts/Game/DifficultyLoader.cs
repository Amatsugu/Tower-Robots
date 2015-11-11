using UnityEngine;
using System.Collections;

public class DifficultyLoader : MonoBehaviour {
	public static DifficultyLoader Instance;
	public int Waves;
	public float Modifier;
	public int Difficulty;
	public bool WasSet = false;
	//public WWW API;
	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
}
