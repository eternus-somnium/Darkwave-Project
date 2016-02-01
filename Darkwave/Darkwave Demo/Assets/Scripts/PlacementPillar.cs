using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacementPillar : MonoBehaviour 
{
	public int wallRange;
	Vector3 oldPosition;
	public GameObject tempWall;
    List<GameObject> tempWalls;
    

	void Start () 
    {
		oldPosition = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(gameObject.transform.position != oldPosition)
		{
			oldPosition = gameObject.transform.position;
			BuildWalls();
		}

        for (int i = 0; i < tempWalls.Count; i++)
        {
            if (tempWalls[i] != null)
            {
                GameObject.Destroy(tempWalls[i]);//destorys game objects
            }
        }
        tempWalls.Clear();//empties list

	}

	void BuildWalls()
	{
		List<Vector3> tempWallPositions = GameObject.Find ("Ground").GetComponent<Grid>().getAdjacentWallLocations(this.gameObject.transform.position, wallRange);
		
		foreach (Vector3 center in tempWallPositions)
		{
			GameObject newWall = (GameObject)Instantiate(tempWall,center+tempWall.transform.position,gameObject.transform.rotation);
			newWall.gameObject.transform.LookAt(this.gameObject.transform);
			tempWalls.Add(newWall);
		}
	}
}
