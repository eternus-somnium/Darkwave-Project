using UnityEngine;
using System.Collections;

public class Pillar : BuildableObject 
{
	GameObject[] allyList;
	public float wallRange;

	// Use this for initialization
	void Start () 
	{
		BuildableObjectStart();
		InvokeRepeating("BuildWalls",1,1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		BuildableObjectUpdate();
		allyList = GameObject.Find("Game Controller").GetComponent<GameController>().enemyTargets;
	}

	void BuildWalls()
	{

		foreach (GameObject item in allyList)
		{
			if(item != null && 
			   item != this && 
			   item.GetComponent<BuildableObject>() != null && 
			   item.GetComponent<BuildableObject>().isPillar)
			{
				float wallDistance = Vector3.Distance(gameObject.transform.position, item.transform.position);
				if(wallDistance < wallRange)
				{
					Debug.Log(this + " sees " + item);
				}
			}
		}
	}

}
