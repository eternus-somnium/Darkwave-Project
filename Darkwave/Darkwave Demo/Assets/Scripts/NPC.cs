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
	int weaponChoice  = 1;
	
	public int cooldown1 = 1;
	public int energyDrain1 = 0;
	public int currentCooldown1 = 0;
	public GameObject attack1;
	
	public int cooldown2 = 1;
	public int energyDrain2 = 5;
	public int currentCooldown2 = 0;
	public GameObject attack2;
	
	public int cooldown3 = 1;
	public int energyDrain3 = 5;
	public int currentCooldown3 = 0;
	public GameObject attack3;
	
	public int cooldown4 = 1;
	public int energyDrain4 = 5;
	public int currentCooldown4 = 0;
	public GameObject attack4;

	public Vector3 shotSpawnPosition;
	public Quaternion shotSpawnRotation;

	// NPCs should use weapon scripts for weapon attacks; this is a temporary change.
	private GameObject newShot;
	private Shot shotScript;

	// Use this for initialization
	public void NPCStart () 
	{
		EntityStart();
		energy=maxEnergy;
		InvokeRepeating("AttackCooldowns",0,.5f);
		InvokeRepeating("ChooseTarget",1,1f);
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

		shotSpawnPosition = transform.position;

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
		if(currentCooldown1 > 0) currentCooldown1--;
		if(currentCooldown2 > 0) currentCooldown2--;
		if(currentCooldown3 > 0) currentCooldown3--;
		if(currentCooldown4 > 0) currentCooldown4--;
		if(energy < maxEnergy) energy++;
	}

	void ChooseTarget()
	{
		if(target == null)
		{
			target = targetList[0];
		}

		targetDistance = Vector3.Distance(gameObject.transform.position, target.transform.position);

		foreach (GameObject possibleTarget in targetList) 
		{
			if(
				(possibleTarget.GetComponent<Entity>().aggroValue / 
				Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position)) > 
				(target.GetComponent<Entity>().aggroValue / targetDistance))
				target = possibleTarget;
		}
	}


	//Function controlling the usage of shot attacks. May eventually be expanded control of melee attacks.
	//Called by the child function when the conditions have been.
	public void Attack()
	{
		switch(WeaponChoice)
		{
		case 1:
			if(currentCooldown1 == 0)
			{
				newShot = (GameObject)Instantiate(attack1, shotSpawnPosition, shotSpawnRotation);
				shotScript = newShot.GetComponent<Shot>();
				shotScript.shooter = this.gameObject;
				shotScript.maxHealth *= this.dmgMod;
				shotScript.health *= this.dmgMod;

				currentCooldown1 = cooldown1;
				energy -= energyDrain1;
			}
			break;
		case 2:
			if(currentCooldown2 == 0)
			{
				Instantiate(attack2, shotSpawnPosition, shotSpawnRotation);
				currentCooldown2 = cooldown2;
				energy -= energyDrain2;
			}
			break;
		case 3:
			if(currentCooldown3 == 0)
			{
				Instantiate(attack3, shotSpawnPosition, shotSpawnRotation);
				currentCooldown3 = cooldown3;
				energy -= energyDrain3;
			}
			break;
		case 4:
			if(currentCooldown1 == 0)
			{
				Instantiate(attack4, shotSpawnPosition, shotSpawnRotation);
				currentCooldown4 = cooldown4;
				energy -= energyDrain4;
			}
			break;
		}
	}

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{
		if((stun == 0) && 
		   ((gameObject.layer == 8 && col.gameObject.layer == 9) || 
		 	(gameObject.layer == 9 && col.gameObject.layer == 8)))
		{
			col.gameObject.GetComponent<Entity>().health -= gameObject.GetComponent<Entity>().touchDamage;
		}
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
