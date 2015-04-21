using UnityEngine;
using UnityEditor;
using System.Collections;

//This code serves as the base for NPC turrets
public class Turret : NPC 
{
	void Start()
	{
		NPCStart();
	}
	public void TurretStart()
	{
		NPCStart();
	}

	// FixedUpdate is called at a set interval
	void FixedUpdate () 
	{
		NPCUpdate();
		shotSpawnPosition = transform.position;
		shotSpawnRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
		EntityAI();
		
		//Debug.DrawRay (transform.position, target.transform.position - transform.position, rayColor);
	}
	public void TurretFixedUpdate () 
	{
		NPCUpdate();
		EntityAI();
		
		//Debug.DrawRay (transform.position, target.transform.position - transform.position, rayColor);
	}
	
	//Controls the behavior of the enemy object
	void EntityAI()
	{	
		//if the player is in sight
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			Attack();
		}
	}
}