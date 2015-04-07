using UnityEngine;
using System.Collections;
/*This function serves as the base for all enemies
*/
public class Enemy : NPC 
{
	// Use this for initialization
	public void EnemyStart () 
	{
		NPCStart();
	}
	//Updates the target variable every frame to track the player object
	public void EnemyUpdate()
	{
		NPCUpdate();
		shotSpawnPosition = transform.position;

		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			inSight = true;
		}
		else inSight = false;
	}

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{
		if((stun == 0) && (col.gameObject.layer == 8))
		{
			Debug.Log ("Read Collision");
			gameObject.GetComponent<Entity>().health -= col.gameObject.GetComponent<Entity>().touchDamage;
			col.gameObject.GetComponent<Entity>().health -= gameObject.GetComponent<Entity>().touchDamage;
			print ("hit");
			stun = 5;
		}
	}
	
}
