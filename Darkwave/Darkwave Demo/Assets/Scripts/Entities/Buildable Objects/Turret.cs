using UnityEngine;
using UnityEditor;
using System.Collections;

//This code serves as the base for NPC turrets
public class Turret : BuildableObject 
{
	int numberOfWeapons;
	void Start()
	{
		BuildableObjectStart();
		numberOfWeapons = weapons.Length;
	}

	// FixedUpdate is called at a set interval
	void Update () 
	{
		BuildableObjectUpdate();
		TurretAI();

		
		//Debug.DrawRay (transform.position, target.transform.position - transform.position, rayColor);
	}
	
	//Controls the behavior of the npc turret
	void TurretAI()
	{	
		//if the target is in sight
		RaycastHit hit;
		if(target != null && 
		   Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, sensorRange) && 
		   hit.transform.gameObject == target.gameObject)
		{
			transform.LookAt(target.transform.position);
			WeaponMainAction(WeaponChoice);
			WeaponChoice = (WeaponChoice+1)%numberOfWeapons;
		}
	}
}