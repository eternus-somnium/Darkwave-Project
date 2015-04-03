using UnityEngine;
using System.Collections;

public class EWalker : Enemy
{

	private NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{
		EnemyStart ();

		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		EnemyUpdate();
		agent.SetDestination (target.gameObject.transform.position);
		agent.speed = speed;
	}
}
