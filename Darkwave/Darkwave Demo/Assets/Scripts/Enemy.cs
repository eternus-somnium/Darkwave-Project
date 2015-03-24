using UnityEngine;
using System.Collections;
/*This function serves as the base for all enemies
*/
public class Enemy : NPC 
{
	public void EnemyStart()
	{
		NPCStart();
	}
	
	public void EnemyUpdate()
	{
		NPCUpdate();

	}
	

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{
		if((stun == 0) && (col.gameObject.layer == 8))
		{
			gameObject.GetComponent<Entity>().health -= col.gameObject.GetComponent<Entity>().touchDamage;
			stun = 20;
		}
	}
}
