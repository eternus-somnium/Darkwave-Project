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
		transform.position += randomAdd;
	}
	
	// Update is called once per frame
	void Update () {
		
		NonPlayerUpdate();

		WalkerAI();

		if (inSight) {
			for(int s = 0; s < squad.Count; s++){
				//squad[s].GetComponent<Zombie> ().inSight = inSight;
				Vector3 surroundOffset = new Vector3(Mathf.Cos((360 * ((s + 1) / (squad.Count + 1.0f))) * Mathf.Deg2Rad) * 2,0,Mathf.Sin((360 * ((s + 1) / (squad.Count + 1.0f))) * Mathf.Deg2Rad) * 2);
				//squad[s].GetComponent<Zombie> ().agent.SetDestination(target.transform.position + surroundOffset);
			}
		}
		
		if (target != null && agent.isActiveAndEnabled) {
			Vector3 leaderPlace = new Vector3 (Mathf.Cos (360 * Mathf.Deg2Rad) * 2, 0, Mathf.Sin (360 * Mathf.Deg2Rad) * 2);
			//agent.SetDestination (target.transform.position + randomAdd);
			agent.SetDestination (target.transform.position + leaderPlace);
			Vector3 targetAtHeight = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
			transform.rotation = Quaternion.LookRotation(targetAtHeight - transform.position);
		}
		if(GetComponent<Animator> ()) GetComponent<Animator> ().SetFloat ("Speed", augmentedSpeed / baseSpeed);

		if (health <= 0) {
			for (int s = 0; s < squad.Count; s++) {
				squad [s].GetComponent<Zombie> ().leader = null;
			}
		}
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
