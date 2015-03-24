using UnityEngine;
using System.Collections;

/* Base code for all shot objects.  All shots disapppear when they move offscreen
 */
public class Shot : Entity 
{
	public void ShotStart()
	{
		EntityStart();
	}

	public void ShotUpdate()
	{
		EntityUpdate();
		if(health < 1) Destroy(gameObject);
		//if(health < 1) Stub for destruction animation control
	}
}
