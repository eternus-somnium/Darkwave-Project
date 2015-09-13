using UnityEngine;
using System.Collections;

public class MeleeWeapon : Weapon 
{

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
			if(currentCooldown == 0)
			{
<<<<<<< HEAD:Darkwave Demo/Assets/Scripts/Weapons/MeleeWeapon.cs
				AttackAnimation();
=======

>>>>>>> origin/DoomTay-branch:Darkwave/Darkwave Demo/Assets/Scripts/Weapons/MeleeWeapon.cs
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
