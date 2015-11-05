using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pillar : BuildableObject 
{
	public int wallRange;
	public GameObject wall;
	public List<GameObject> walls;

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
		List<Vector3> wallPositions = PlacementGrid.getAdjacentWallLocations(this.gameObject.transform.position, wallRange);

		foreach (Vector3 center in wallPositions)
		{
			GameObject newWall = (GameObject)Instantiate(wall,center+wall.transform.position,gameObject.transform.rotation);
			newWall.gameObject.transform.LookAt(this.gameObject.transform);
			walls.Add(newWall);
		}
	}

	void Death()
	{
		dying=true;
		if(remains != null)
			Instantiate(remains, gameObject.transform.position, gameObject.transform.rotation);
		for (int i = 0; i < walls.Count; i++)
		{
			if (walls[i] != null)
			{
				GameObject.Destroy(walls[i]);//destorys game objects
			}
		}
		Destroy(gameObject,1);
	}

}
