using UnityEngine;
using System.Collections;

//This code serves as the base for floating enemies
public class Turret : NPC 
{
	void Start()
	{
		NPCStart();
	}

	// FixedUpdate is called at a set interval
	void Update () 
	{
		NPCUpdate();
		EntityAI();
		
		//Debug.DrawRay (transform.position, target.transform.position - transform.position, rayColor);
		
		//Movement();
	}

	//Controls the behavior of the enemy object
	void EntityAI()
	{	
		//if the player is in sight
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			
			//shotSpawnPosition = new Vector2(transform.position.x+(2.5F * facing), transform.position.y);
			//shotSpawnRotation = Quaternion.Euler(0,0,facing==1?0:180);
			
			shotSpawnPosition = transform.position;
			
			Quaternion newRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

			transform.Rotate (newRotation.eulerAngles);

			shotSpawnRotation = newRotation;

			Attack();
			
			startPosition = transform.position;
		}
	}
}
