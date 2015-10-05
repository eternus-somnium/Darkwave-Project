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
		//parent.GetComponent<Animator>().SetBool("Attack",mainActionFlag);
		if(mainActionFlag)
		{
			AttackAnimation();
			Ready = false;
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

	void OnCollisionEnter(Collision col)
	{
		//If the weapon hit something on the opposing team
		if((gameObject.layer == 8 && col.gameObject.layer == 9) ||
		   (gameObject.layer == 9 && col.gameObject.layer == 8))
		{
			if (parent.GetComponent<Character>() != null && col.collider.material.name == "Head (Instance)")
			{
				//touchDamage = Mathf.RoundToInt(touchDamage * criticalMultiplier);
			}
			
			col.gameObject.GetComponent<Unit>().DamageController(touchDamage, false);
			Debug.Log (gameObject + " hit " + col.gameObject);
		}
	}
}
