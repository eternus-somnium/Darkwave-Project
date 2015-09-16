using UnityEngine;
using System.Collections;

public class Walker : NonPlayer
{
	
	private NavMeshAgent agent;
	Vector3 goal, randomAdd;
	
	// Use this for initialization
	void Start () 
	{
		NonPlayerStart();
		
		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
		randomAdd = new Vector3(Random.Range (-1,1),0,Random.Range (-1,1));
	}
	
	// Update is called once per frame
	void Update () 
	{
		NonPlayerUpdate();
		WalkerAI();


		if(target != null && agent.isActiveAndEnabled)
			agent.SetDestination (target.transform.position + randomAdd);
	}

	//Controls the behavior of the npc turret
	void WalkerAI()
	{	
		//if the player is in sight
		RaycastHit hit;
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, engagementRange) && 
		   hit.transform.gameObject == target.gameObject)
		{
			MainAction();
		}
	}
	
}