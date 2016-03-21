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
			parent.GetComponent<Rigidbody>().AddForce 
				(Vector3.Normalize(parent.GetComponent<NonPlayer>().target.transform.position -  parent.transform.position) * 
				lungeSpeedMultiplier);

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
		if((gameObject.layer == 8 && col.gameObject.layer == 9) ||
			(gameObject.layer == 9 && col.gameObject.layer == 8 &&
			lungeTimer > 0))
		{
			col.gameObject.GetComponent<Entity>().DamageController(augmentedDamage);
			lungeTimer = 0;
		}
		else if(lungeTimer > 0) lungeTimer -= Time.fixedDeltaTime;
			//Debug.Log (gameObject + " hit " + col.gameObject);
	}
}
