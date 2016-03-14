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
		List<GameObject> potentialPillars = PlacementGrid.getPillarsInRange(this.gameObject.transform.position, wallRange);
		GameObject newWall;
		foreach (GameObject p in potentialPillars)
		{
			RaycastHit h;
			Vector3 direction = p.transform.position-gameObject.transform.position;
			Physics.Raycast(gameObject.transform.position, direction, out h);
			if(h.collider != null && h.collider.gameObject == p)
			{
				newWall = (GameObject)Instantiate(
					wall,
					gameObject.transform.position + direction/2,
					gameObject.transform.rotation);

				newWall.gameObject.transform.LookAt(p.transform);
			}
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
