using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacementWall : MonoBehaviour 
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
            m_walls.Add(Instantiate(m_tempWall, v, Quaternion.identity) as GameObject);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < m_walls.Count; i++)
        {
            if (m_walls[i] != null)
            {
                GameObject.Destroy(m_walls[i]);//destorys game objects
            }
        }
        m_walls.Clear();//empties list

        List<Vector3> vecs = m_grid.getAdjacentWallLocations(transform.position);
        foreach (Vector3 v in vecs)
        {
            m_walls.Add(Instantiate(m_tempWall, v, Quaternion.identity) as GameObject);
        }
	}

    void OnDestroy()
    {
        if (m_walls.Count > 0)
            for (int i = 0; i < m_walls.Count; i++)
            {
                if (m_walls[i] != null)
                {
                    GameObject.Destroy(m_walls[i]);
                    m_walls[i] = null;
                }
            }
    }
}
