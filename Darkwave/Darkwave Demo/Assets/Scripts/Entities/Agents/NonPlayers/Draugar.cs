using UnityEngine;
using System.Collections;

public class Draugar : Walker {

	private float acceleration;
	private NavMeshAgent agent;
	Vector3 goal, randomAdd;
	private Vector3 targetDir;
	private OffMeshLink[] passZones;
	public bool isWisp = false;

	// Use this for initialization
	void Start () {
		NonPlayerStart();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.speed = baseSpeed;
		randomAdd = new Vector3(Random.Range (-1,1),0,Random.Range (-1,1));
		passZones = FindObjectsOfType<OffMeshLink> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		NonPlayerUpdate();

		//UsePassZones ();

		if(agent.isOnOffMeshLink) isWisp = false;

		if(!isWisp) WeaponMainAction(WeaponChoice);
		
		WalkerAI();
		
		
		if(target != null && agent.isActiveAndEnabled)
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

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == 11 && isWisp)
			Physics.IgnoreCollision (GetComponent<Collider> (), col.gameObject.GetComponent<Collider> ());
	}

	void UsePassZones ()
	{
		foreach(OffMeshLink zone in passZones)
		{
			if(Vector3.Distance(transform.position,zone.transform.position) <= sensorRange && isWisp) zone.activated = true;
			else zone.activated = false;
		}
	}
}
