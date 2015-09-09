using UnityEngine;
using System.Collections;

/* Base code for all shot objects.  All shots disapppear when they move offscreen
 */
public class Shot : Entity
{
	public bool
		onFire;
	public float 
		baseSpeed,
		damageModifier=1,
		criticalMultiplier=1;
	
	public void ShotStart()
	{
		health = maxHealth;
		criticalMultiplier = 1;
	}

	public void ShotUpdate()
	{
		if(health < 1) Destroy(gameObject);
		//if(health < 1) Stub for destruction animation control
	}
	
	public void BulletModifications(GameObject p)
	{
		parent = p;
	}

}
