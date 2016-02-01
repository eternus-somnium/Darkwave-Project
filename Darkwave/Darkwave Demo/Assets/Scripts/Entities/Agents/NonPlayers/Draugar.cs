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

<<<<<<< HEAD
		//UsePassZones ();
=======
		UsePassZones ();

		/*if (target != null) targetDir = transform.InverseTransformPoint (target.transform.position);
		else return;
		
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position,transform.forward, out hit, 5) && hit.transform.gameObject.tag != "Shot"  && hit.transform.gameObject.tag != "LitArea" && !weapons[WeaponChoice].GetComponent<Weapon>().mainActionFlag)
		{
			Vector3 avoidDir = transform.InverseTransformPoint(hit.point);
>>>>>>> origin/AIExperiment

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
<<<<<<< HEAD
			if(Vector3.Distance(transform.position,zone.transform.position) <= sensorRange && isWisp) zone.activated = true;
			else zone.activated = false;
=======
			if(Vector3.Distance(transform.position,zone.transform.position) <= sensorRange && weapons[WeaponChoice].GetComponent<Weapon>().mainActionFlag)
			{
				zone.activated = true;
				agent.areaMask = 13;
			}
			else
			{
				zone.activated = false;
				agent.areaMask = 5;
			}
>>>>>>> origin/AIExperiment
		}
	}
}
