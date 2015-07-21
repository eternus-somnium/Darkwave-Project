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
		parent.GetComponent<Animator>().SetBool("Attack",mainActionFlag);
		if(mainActionFlag)
		{
<<<<<<< HEAD
=======
			AttackAnimation();
			Ready = false;
>>>>>>> Finished FPS Update
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
