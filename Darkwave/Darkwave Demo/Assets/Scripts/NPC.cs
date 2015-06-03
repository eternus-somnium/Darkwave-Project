using UnityEngine;
using System.Collections;

public class NPC : Entity 
{
	//Behavior variables (set in editor)
	public GameObject[] targetList;
	public GameObject target = null;
	public float targetDistance;
	public int behavior;

	public int sensorRange;
	public int engagementRange;
	public bool inSight;


	public int energy;
	public int maxEnergy;
	public GameObject treasure;
	
	//Movement
	public float jumpHeight;// set in editor
	public Vector3 direction;
	bool isJumping;

	//Attack variables
	int weaponChoice  = 0;
	public GameObject[] weapons;

	// Use this for initialization
	public void NPCStart () 
	{
		EntityStart();
		energy=maxEnergy;
		InvokeRepeating("AttackCooldowns",0,.5f);
		InvokeRepeating("ChooseTarget",0,2f);
	}

	// Update is called once per frame
	public void NPCUpdate () 
	{
		EntityUpdate();

		if(gameObject.tag == "Ally")
			targetList = GameObject.Find("Game Controller").GetComponent<GameController>().allyTargets;
		else targetList = GameObject.Find("Game Controller").GetComponent<GameController>().enemyTargets;

		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			inSight = true;
		}
		else inSight = false;

		if(health < 1)
		{
			if(treasure != null)
				Instantiate(treasure, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
			//Stub for destruction animation control
		}
	}

	void AttackCooldowns()
	{
		if(energy < maxEnergy) energy++;
	}

	void ChooseTarget()
	{
		if(targetList.Length != 0)
		{
			if(target == null)
			{
				target = targetList[0];
			}

			targetDistance = Vector3.Distance(gameObject.transform.position, target.transform.position);

			foreach (GameObject possibleTarget in targetList) 
			{
				if(possibleTarget != null)
				{
					float possibleTargetDistance = Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position);
					if((possibleTarget.GetComponent<Entity>().aggroValue / possibleTargetDistance) > 
						(target.GetComponent<Entity>().aggroValue / targetDistance))
					{
						target = possibleTarget;
						targetDistance = possibleTargetDistance;
					}
				}
			}
		}
	}


	//Function controlling the usage of shot attacks. May eventually be expanded control of melee attacks.
	//Called by the child function when the conditions have been.
	public void Attack()
	{
		weapons[weaponChoice].SendMessage("MainActionController", true);
	}



	public int WeaponChoice 
	{
		get 
		{
			return weaponChoice;
		}
		set 
		{
			weaponChoice = value;
		}
	}
}
