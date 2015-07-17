using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour 
{
    public bool m_hasTurret;
    GameObject m_turret;

	// Use this for initialization
	void Start () 
    {
        m_hasTurret = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void addTurret(GameObject turret)
    {
        if (!m_hasTurret)
        {
            m_turret = turret;
            m_hasTurret = true;
        }
        else
        {
            Debug.Log("ERROR:Wall Already Has Turret");
        }
    }
}
