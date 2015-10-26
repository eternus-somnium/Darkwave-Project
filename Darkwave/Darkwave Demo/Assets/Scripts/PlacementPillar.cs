using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacementPillar : MonoBehaviour 
{
    Grid m_grid;
    List<GameObject> pillars;
    GameObject m_tempWall;

	void Start () 
    {
        m_grid = GameObject.Find("Ground").GetComponent<Grid>();
        m_tempWall = (GameObject)Resources.Load("Prefabs/BasicWallConnector", typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < pillars.Count; i++)
        {
            if (pillars[i] != null)
            {
                GameObject.Destroy(pillars[i]);//destorys game objects
            }
        }
        pillars.Clear();//empties list

        List<Vector3> vecs = m_grid.getAdjacentWallLocations(transform.position, 5);
        foreach (Vector3 v in vecs)
        {
            pillars.Add(Instantiate(m_tempWall, v, Quaternion.identity) as GameObject);
        }
	}
}
