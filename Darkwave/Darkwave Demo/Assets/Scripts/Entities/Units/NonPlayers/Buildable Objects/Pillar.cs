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
		List<SensedAgent> sensedPillars = GetComponentInChildren<AdjacencySensor>().sensedPillars;
		RaycastHit h;
		foreach( SensedAgent p in sensedPillars)
		{
			Physics.Raycast(transform.position, Vector3.Normalize(p.agent.transform.position-transform.position), out h);
			if(h.collider.gameObject == p.agent)
				Instantiate(wall, p.agent.transform.position-(p.agent.transform.position-transform.position)/2,Quaternion.identity);
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
