using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Difficulty : MonoBehaviour {
	public GameObject[] Difficulties;
	public int[] Waves;
	public float[] Modifier;
	public List<MouseOver> ActiveDiff = new List<MouseOver>();
	// Use this for initialization
	void Start () 
	{
		//int c = 0;
		foreach(GameObject g in Difficulties)
		{
			ActiveDiff.Add(g.GetComponent<MouseOver>());
			//c++;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		int c = 0;
		if(c > ActiveDiff.Count)
		{
			c = 0;
		}
		while(c < ActiveDiff.Count)
		{
			if(ActiveDiff[c].Active)
			{
				if(Input.GetKeyUp(KeyCode.Mouse0))
				{
					SetDifficulty(c);
				}
			}
			c++;
		}
	}
	void SetDifficulty(int diff)
	{
		Debug.Log("Difficulty: " + diff);
		DifficultyLoader.Instance.Waves = Waves[diff];
		DifficultyLoader.Instance.Modifier = Modifier[diff];
		DifficultyLoader.Instance.Difficulty = diff;
		DifficultyLoader.Instance.WasSet = true;
		Application.LoadLevel(1);
	}
}
