using UnityEngine;
using System.Collections;

public class Zombie : NonPlayer {

	private float acceleration;
	private NavMeshAgent agent;
	Vector3 goal, randomAdd;
	private Vector3 targetDir;
	public ZombieMaster leader;

	// Use this for initialization
	void Start () {
		NonPlayerStart();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
		randomAdd = new Vector3(Random.Range (-1,1),0,Random.Range (-1,1));
	}
	
	// Update is called once per frame
	void Update () {
		NonPlayerUpdate();

		if (leader) target = leader.target;

		if(inSight)
		{
			foreach(GameObject soldier in leader.squad)
			{
				soldier.GetComponent<Zombie>().inSight = inSight;
				if(target != null && agent.isActiveAndEnabled)
					soldier.GetComponent<Zombie>().agent.SetDestination (target.transform.position + randomAdd);
			}
			leader.GetComponent<ZombieMaster>().inSight = inSight;
		}

		if(target != null && agent.isActiveAndEnabled)
			agent.SetDestination (target.transform.position + randomAdd);
		if(GetComponent<Animator> ()) GetComponent<Animator> ().SetFloat ("Speed", augmentedSpeed / baseSpeed);
	}
}
