using UnityEngine;
using System.Collections;

public class MeleeWeapon : Weapon 
{
	public AudioClip primaryHit;
	public AudioClip secondaryHit;
	// Use this for initialization
	void Start () 
	{
		WeaponStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mainActionFlag) MainAction();
		if(secondaryActionFlag) SecondaryAction();
	}
	public void MainAction()
	{
		if(mainActionFlag)
		{
			if(currentCooldown == 0)
			{
				AttackAnimation();
				currentCooldown=augmentedCooldown;
			}
		}
	}
	
	public void SecondaryAction()
	{
		if(secondaryActionFlag)
		{
			if(!mainActionFlag)
			{

				if(currentCooldown == 0)
				{
					AttackAnimation();
					//Weapon swing stub

					currentCooldown = augmentedCooldown;
					currentEnergy -= energyDrain;
				}
			}
		}
	}

}
