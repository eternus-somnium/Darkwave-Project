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

	// Update is called once per frame
	void Update ()
	{
		MainAction();
		SecondaryAction();
	}

	public void MainAction()
	{
		if(mainActionFlag)
		{
			AttackAnimation();
			if(Ready && currentEnergy > energyDrain)
			{
				Debug.Log("Lunging");
				transform.Translate((parent.GetComponent<NPC>().target.transform.position)*parent.GetComponent<NPC>().baseSpeed*lungeSpeedMultiplier);
				Ready=false;
				currentCooldown = augmentedCooldown;
				currentEnergy -= energyDrain;
			}
		}
	}

	public void SecondaryAction()
	{

		if(secondaryActionFlag)
		{
			//Raise arms
		}
		else
		{
			//lower arms
		}
	}
}
