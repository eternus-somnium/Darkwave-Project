using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieMaster : NonPlayer {
	
	private float acceleration;
	private NavMeshAgent agent;
	Vector3 goal, randomAdd;
	private Vector3 targetDir;
	public List<GameObject> squad = new List<GameObject>();
	public GameObject template;

	
	// Use this for initialization
	void Start () {
		NonPlayerStart();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
		randomAdd = new Vector3(Random.Range (-1,1),0,Random.Range (-1,1));
		for (var z = 0; z < 5; z++)
		{
			var newZombie = (GameObject)Instantiate (template, transform.position + (transform.forward * (2 + z)), transform.rotation);
			newZombie.GetComponent<Zombie>().leader = GetComponent<ZombieMaster>();
			squad.Add (newZombie);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		NonPlayerUpdate();

		WalkerAI();
		
		
		if(target != null && agent.isActiveAndEnabled)
			agent.SetDestination (target.transform.position + randomAdd);
		if(GetComponent<Animator> ()) GetComponent<Animator> ().SetFloat ("Speed", augmentedSpeed / baseSpeed);
	}
	
	//Controls the behavior of the npc turret
	void WalkerAI()
	{	
		/*//if the player is in sight
		RaycastHit hit;
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, engagementRange) && 
		   (hit.transform.gameObject == target.gameObject || hit.collider.gameObject.layer == 11))
		{
			WeaponMainAction(WeaponChoice);
		}*/
	}
}
