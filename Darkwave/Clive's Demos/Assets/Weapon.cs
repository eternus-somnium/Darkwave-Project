using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public int weaponType; // 0 Melee, 1 Ranged
	public int cooldown = 50;
	public int currentCooldown = 0;
	
	public int energy = 100;
	public int currentEnergy;
	public int energyDrain = 0;
	public GameObject shot;

	// Use this for initialization
	void Start () 
	{
		currentEnergy = energy;
	}

	void FixedUpdate()
	{
		if(currentEnergy < energy) currentEnergy++;
		if(currentCooldown > 0) currentCooldown--;
	}

	void OnAttackCommand()
	{
		AttackAnimation();
		if(currentCooldown == 0)
		{
			if(weaponType == 0)
			{
				//Weapon swing stub
			}
			if(weaponType == 1)
			{
				Vector3 shotSpawnPosition = this.gameObject.transform.position;
				Quaternion shotSpawnRotation = Camera.mainCamera.transform.rotation;

				Instantiate(shot, shotSpawnPosition, shotSpawnRotation);
				currentCooldown = cooldown;
				energy -= energyDrain;
			}
		}
	}

	void AttackAnimation()
	{
		//Trigger animation built into weapon object
	}
}
