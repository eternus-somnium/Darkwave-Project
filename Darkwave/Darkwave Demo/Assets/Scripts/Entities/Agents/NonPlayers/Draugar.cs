using UnityEngine;
using System.Collections;

public class Draugar : NonPlayer {

	private float acceleration;
	private NavMeshAgent agent;
	Vector3 goal, randomAdd;
	private Vector3 targetDir;
	private OffMeshLink[] passZones;

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

		UsePassZones ();

		/*if (target != null) targetDir = transform.InverseTransformPoint (target.transform.position);
		else return;
		
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position,transform.forward, out hit, 5) && hit.transform.gameObject.tag != "Shot"  && hit.transform.gameObject.tag != "LitArea" && !weapons[WeaponChoice].GetComponent<Weapon>().mainActionFlag)
		{
			Vector3 avoidDir = transform.InverseTransformPoint(hit.point);

			Debug.DrawLine (transform.position, transform.TransformDirection(avoidDir), Color.red);
			//transform.Rotate(new Vector3(0,-avoidDir.x,0).normalized);
			if(targetDir.y < 0) transform.Rotate(new Vector3(0,-1,0));
			else if(targetDir.y > 0) transform.Rotate(new Vector3(0,1,0));
			//acceleration = 0;
		}
		else if(target != null)
		{
			transform.Rotate(new Vector3(0,targetDir.x,0).normalized * 1);
			if(Mathf.Abs(Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg) < 60 * Mathf.Abs(GetComponent<Rigidbody>().angularVelocity.y))
			{
				transform.Rotate(Vector3.zero);
			}
			
			//if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, engagementRange) && hit.transform.gameObject == target.gameObject && Random.value >= 0.5) WeaponMainAction(WeaponChoice);
			
			acceleration = augmentedSpeed * Time.deltaTime;
		}
		
		transform.Translate(new Vector3(0,0,acceleration));
		Debug.DrawLine (transform.position, transform.TransformDirection(targetDir), Color.green);*/
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
		if (col.gameObject.layer == 11 && weapons[WeaponChoice].GetComponent<Weapon>().mainActionFlag)
			Physics.IgnoreCollision (GetComponent<Collider> (), col.gameObject.GetComponent<Collider> ());
	}

	void UsePassZones ()
	{
		foreach(OffMeshLink zone in passZones)
		{
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
		}
	}
}
