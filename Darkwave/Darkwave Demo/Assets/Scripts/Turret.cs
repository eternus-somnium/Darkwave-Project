using UnityEngine;
using UnityEditor;
using System.Collections;

//This code serves as the base for NPC turrets
public class Turret : NPC 
{
	void Start()
	{
		NPCStart();
		if(gameObject.layer == 8) attack1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Shot.prefab", typeof(GameObject));
		else attack1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/EShot.prefab", typeof(GameObject));
	}
	public void TurretStart()
	{
		NPCStart();
		if(gameObject.layer == 8) attack1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Shot.prefab", typeof(GameObject));
		else attack1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/EShot.prefab", typeof(GameObject));
	}

	// FixedUpdate is called at a set interval
	void FixedUpdate () 
	{
		NPCUpdate();
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
			//shotSpawnPosition = new Vector2(transform.position.x+(2.5F * facing), transform.position.y);
			//shotSpawnRotation = Quaternion.Euler(0,0,facing==1?0:180);
			
			shotSpawnPosition = transform.position;
			
			Quaternion newRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
			//newRotation *= Quaternion.FromToRotation(Vector3.forward, Vector3.left);
			shotSpawnRotation = newRotation;
			
			Attack();
		}
	}
}