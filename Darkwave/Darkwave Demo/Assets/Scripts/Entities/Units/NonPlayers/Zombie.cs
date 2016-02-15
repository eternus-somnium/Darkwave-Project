using UnityEngine;
using System.Collections;

public class Zombie : NonPlayer {

	private float acceleration;
	public NavMeshAgent agent;
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
		WalkerAI ();

		if (leader) target = leader.target;

		if(inSight && leader != null)
		{
			foreach(GameObject soldier in leader.squad)
			{
				soldier.GetComponent<Zombie>().inSight = inSight;
			}
			leader.GetComponent<ZombieMaster>().inSight = inSight;
		}

		if(target != null && agent.isActiveAndEnabled && leader != null)
			agent.SetDestination (target.transform.position + randomAdd);
		if(GetComponent<Animator> ()) GetComponent<Animator> ().SetFloat ("Speed", augmentedSpeed / baseSpeed);
	}

	//Controls the behavior of the npc turret
	void WalkerAI()
	{	
		//if the player is in sight
		RaycastHit hit;
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, engagementRange) && 
		   (hit.transform.gameObject == target.gameObject || hit.collider.gameObject.layer == 11))
		{
			WeaponMainAction(WeaponChoice);
		}
	}
}
