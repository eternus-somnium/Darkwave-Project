using UnityEngine;
using System.Collections;

public class EWalker : NPC
{

	private NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{
		NPCStart ();

		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		NPCUpdate();
		if(target != null && agent.isActiveAndEnabled)
			agent.SetDestination (target.gameObject.transform.position);
	}

}
