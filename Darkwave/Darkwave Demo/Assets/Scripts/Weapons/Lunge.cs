using UnityEngine;
using System.Collections;

public class Lunge : Weapon 
{
	public int distance, lungeSpeedMultiplier;


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
			//transform.Translate((parent.GetComponent<NPC>().target.transform.position)*parent.GetComponent<NPC>().baseSpeed*lungeSpeedMultiplier);
			Ready=false;
			currentCooldown = augmentedCooldown;
			currentEnergy -= energyDrain;
		}
	}

	public void SecondaryAction()
	{
		
	}
}
