using UnityEngine;
using UnityEditor;
using System.Collections;

//This code serves as the base for NPC turrets
public class Turret : BuildableObject 
{
	void Start()
	{
		NonPlayerStart();
	}
	public void TurretStart()
	{
		NonPlayerStart();
	}

	// FixedUpdate is called at a set interval
	void Update () 
	{
		NonPlayerUpdate();
		TurretAI();

		
		//Debug.DrawRay (transform.position, target.transform.position - transform.position, rayColor);
	}
	public void TurretUpdate () 
	{
		NonPlayerUpdate();
		TurretAI();
		
		//Debug.DrawRay (transform.position, target.transform.position - transform.position, rayColor);
	}
	
	//Controls the behavior of the npc turret
	void TurretAI()
	{	
		//if the player is in sight
		RaycastHit hit;
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, sensorRange) && 
		   hit.transform.gameObject == target.gameObject)
		{
			weapons[WeaponChoice].transform.LookAt(target.transform.position);
			MainAction();
		}
	}
}