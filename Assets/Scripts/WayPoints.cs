using UnityEngine;
using System.Collections;
using System.Linq;

public class WayPoints : MonoBehaviour {
	//Vars
	//Public
	public GameObject[] _WayPoints;
	// Use this for initialization
	void Awake () {
		FindWayPoints();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FindWayPoints()
	{
		_WayPoints = GameObject.FindGameObjectsWithTag("Waypoint").OrderBy( go => go.name ).ToArray();
		
	}
}
