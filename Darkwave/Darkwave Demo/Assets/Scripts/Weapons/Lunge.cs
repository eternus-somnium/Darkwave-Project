using UnityEngine;
using System.Collections;

public class Lunge : Weapon 
{
	public int 
		lungeSpeedMultiplier;

	float lungeTimer;


	// Use this for initialization
	void Start ()
	{
		WeaponStart();
	}

	void Update()
	{
		if(mainActionFlag) MainAction();
		if(secondaryActionFlag) SecondaryAction();
	}

	public void MainAction()
	{
		AttackAnimation();
		if(Ready && currentEnergy > energyDrain)
		{
			Debug.Log("Lunging");
			transform.position = Vector3.MoveTowards(transform.position, 
			                                         parent.GetComponent<NonPlayer>().target.transform.position, 
			                                         parent.GetComponent<NonPlayer>().baseSpeed*lungeSpeedMultiplier);

			lungeTimer = 1;
			Ready=false;
			currentCooldown = augmentedCooldown;
			currentEnergy -= energyDrain;
		}
	}

	public void SecondaryAction()
	{
		
	}

	void OnTriggerStay(Collider col)
	{
		print (col.gameObject);

		if((gameObject.layer == 8 && col.gameObject.layer == 9) ||
			(gameObject.layer == 9 && col.gameObject.layer == 8 &&
			lungeTimer > 0))
		{
			col.gameObject.GetComponent<Unit>().DamageController(augmentedDamage, false);
			lungeTimer = 0;
		}
		else if(lungeTimer > 0) lungeTimer -= Time.deltaTime;
			//Debug.Log (gameObject + " hit " + col.gameObject);
	}
}
