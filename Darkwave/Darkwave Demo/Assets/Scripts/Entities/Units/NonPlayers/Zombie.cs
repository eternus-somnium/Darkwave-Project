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

		/*if(inSight && leader != null)
		{
			foreach(GameObject soldier in leader.squad)
			{
				soldier.GetComponent<Zombie>().inSight = inSight;
			}
			leader.GetComponent<ZombieMaster>().inSight = inSight;
		}*/

		if (target != null && agent.isActiveAndEnabled) {
			//if(leader == null) agent.SetDestination (target.transform.position + randomAdd);
			agent.SetDestination (target.transform.position + randomAdd);
			Vector3 targetAtHeight = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
			Vector3 finalLookGoal = CappedLerp (transform.forward, targetAtHeight - transform.position);
			//Vector3 finalLookGoal = Vector3.ClampMagnitude(targetAtHeight - transform.position, agent.angularSpeed);
			//transform.rotation = Quaternion.LookRotation(finalLookGoal);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetAtHeight) , agent.angularSpeed * Time.deltaTime);
		}
		if(GetComponent<Animator> ()) GetComponent<Animator> ().SetFloat ("Speed", augmentedSpeed / baseSpeed);
	}

	Vector3 CappedLerp (Vector3 startPos, Vector3 endPos)
	{
		float cap = 1;
		for(float i = 0; i < 1.1; i += 0.1f)
		{
			if(Mathf.Rad2Deg*Mathf.Atan(Vector3.Distance(startPos,Vector3.Lerp (startPos, endPos, i))) > agent.angularSpeed * Time.deltaTime) cap = (i - 0.1f);
		}
		return Vector3.Lerp (startPos, endPos, cap);
	}

	//Controls the behavior of the npc turret
	void WalkerAI()
	{	
		//if the player is in sight
		RaycastHit hit;
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, engagementRange) && 
		   hit.transform.gameObject == target.gameObject)
		{
			WeaponMainAction(WeaponChoice);
		}
	}
}
