using UnityEngine;
using System.Collections;
/*This function serves as the base for all enemies
*/
public class Enemy : NPC 
{


	public GameObject[] targets;
	public Vector2 targetLastPosition;
	public Transform currentTarget;
	public double agroDistance;
	public Transform crystal;
	public float speed;
	private int agroHoldCount = 0;

	

	// Use this for initialization
	public void Estart () 
	{
		health = maxHealth;
		energy = maxEnergy;
		startPosition = transform.position;
		speed = baseSpeed;

		targets = GameObject.FindGameObjectsWithTag ("Player");  // Searches for all with "player" tag.
		crystal = GameObject.FindGameObjectWithTag ("Crystal").transform;  // Searches for the crystal.


		currentTarget = crystal;


	}
	//Updates the target variable every frame to track the player object
	public void EUpdate()
	{
		shotSpawnPosition = transform.position;

		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			targetLastPosition = target.transform.position;
			inSight = true;
		}
		else inSight = false;

		StatusUpdate ();
		FindPlayer ();

	}


	void FindPlayer() //Finds players and calls IsAgro to determine if the entity should change targets.
	{
		foreach (GameObject target in targets) {
			if (IsAgro (target)) 
			{
				currentTarget = target.transform;
				speed = baseSpeed * 1.5F; //Increases speed if player is spotted
				return;
			}
			else
			{
				currentTarget = crystal;
				speed = baseSpeed; //Back to normal speed if going for the crystal
				return;
			}
		}
	}  //TODO: Create global player_list for tracking purposes.

	bool IsAgro(GameObject target) //Does the entity Agro?
	{
		//Debug.Log ("Run IsAgro");
		double agroDistanceMod;
		double targetNoise = 1;  // <-- Increases or decreases agro area.
		double targetDistance = Vector3.Distance (target.transform.position, this.transform.position);

		agroDistanceMod = agroDistance * targetNoise; // Modifies agro distance to account for "factors" (noise)

		if (targetDistance < agroDistanceMod) 
		{
			agroHoldCount = 100;
			//Debug.Log ("In Distance");
			return true;
		} 
		else if ((targetDistance < (agroDistanceMod * 2)) && (agroHoldCount > 0))
		{ // Enemy will stay agroed still for an amount of time or until an enemy leaves a larger radius
			agroHoldCount--;
			//Debug.Log (agroHoldCount);
			return true;
		} 
		else 
		{
			return false;
		}

		//TODO Add a player noise level based on player speed to determine agro

		//TODO Add accounting for agro-from-hits and other agro sources.
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
