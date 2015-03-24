using UnityEngine;
using System.Collections;

public class EWalker : Enemy
{

	private NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{

		agent = gameObject.GetComponent<NavMeshAgent> ();
		currentTarget = crystal;
		Estart ();

		agent.speed = baseSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		agent.SetDestination (currentTarget.position);
		agent.speed = speed;
		EUpdate ();
	}
}
