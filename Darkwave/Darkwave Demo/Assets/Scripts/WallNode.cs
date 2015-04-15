using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallNode : Wall
{
    Grid m_grid;
    List<GameObject> m_walls;
    GameObject m_tempWall;

	void Start () 
    {
        m_grid = GameObject.Find("Ground").GetComponent<Grid>();

        List<Vector3> vecs = m_grid.getAdjacentWallLocations(transform.position);

        m_walls = new List<GameObject>();
        m_tempWall = (GameObject)Resources.Load("Prefabs/BasicWallConnector", typeof(GameObject));

        foreach (Vector3 v in vecs)
        {
            GameObject obj = Instantiate(m_tempWall, v, Quaternion.identity) as GameObject;

            m_walls.Add(obj);//adds wall to the array of connecting walls

            m_grid.placeInGrid(obj);//places object reference in grid
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}   
}
