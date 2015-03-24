using UnityEngine;
using System.Collections;

public class NPC : Entity 
{
	//Behavior variables (set in editor)
	public int behavior;
	public int patrolSize;
	public int sensorRange;
	public int engagementRange;
	public bool inSight;
	public int interest;
	public int attentionSpan;
	public int distance;
	public GameObject target;
	public Vector2 targetLastPosition;

	public Vector2 startPosition;

	//Movement
	public float jumpHeight;// set in editor
	public Vector3 direction;
	bool isJumping;

	//Attack variables
	int weaponChoice  = 1;
	
	public int cooldown1 = 50;
	public int energyDrain1 = 0;
	public int currentCooldown1 = 0;
	public GameObject attack1;
	
	public int cooldown2 = 50;
	public int energyDrain2 = 5;
	public int currentCooldown2 = 0;
	public GameObject attack2;
	
	public int cooldown3 = 50;
	public int energyDrain3 = 5;
	public int currentCooldown3 = 0;
	public GameObject attack3;
	
	public int cooldown4 = 50;
	public int energyDrain4 = 5;
	public int currentCooldown4 = 0;
	public GameObject attack4;

	public Vector3 shotSpawnPosition;
	public Quaternion shotSpawnRotation;

	// Use this for initialization
	public void NPCStart () 
	{
		EntityStart();
		startPosition = transform.position;

		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			targetLastPosition = target.transform.position;
			inSight = true;
		}
		else inSight = false;
	}

	// Update is called once per frame
	public void NPCUpdate () 
	{
		EntityUpdate();
		if(currentCooldown1 > 0) currentCooldown1--;
		if(currentCooldown2 > 0) currentCooldown2--;
		if(currentCooldown3 > 0) currentCooldown3--;
		if(currentCooldown4 > 0) currentCooldown4--;
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
				Instantiate(attack1, shotSpawnPosition, shotSpawnRotation);
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
