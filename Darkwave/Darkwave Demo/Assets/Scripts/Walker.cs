using UnityEngine;
using System.Collections;

public class Walker : NPC
{
	
	private NavMeshAgent agent;
	Vector3 goal, randomAdd;
	
	// Use this for initialization
	void Start () 
	{
		NPCStart ();
		
		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
		randomAdd = new Vector3(Random.Range (-1,1),0,Random.Range (-1,1));
	}
	
	// Update is called once per frame
	void Update () 
	{
		NPCUpdate();



		if(target != null && agent.isActiveAndEnabled)
		{
			goal = target.transform.position + randomAdd;

			agent.SetDestination (goal);
		}
	}
	
}