using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pillar : BuildableObject 
{
	public int wallRange;
	public GameObject wall;

	// Use this for initialization
	void Start () 
	{
		BuildableObjectStart();

		BuildWalls();
	}
	
	// Update is called once per frame
	void Update () 
	{
		BuildableObjectUpdate();
	}

	void BuildWalls()
	{
		List<Vector3> pillarsInRange = PlacementGrid.getAdjacentWallLocations(this.gameObject.transform.position, wallRange);

		foreach (Vector3 otherPosition in pillarsInRange)
		{

			Vector3 wallCenter = otherPosition;

			Debug.Log("This: " + gameObject.transform.position + " + That: " + otherPosition + " = " + wallCenter);

			Instantiate(wall,wallCenter,Quaternion.identity);
		}
	}

}
