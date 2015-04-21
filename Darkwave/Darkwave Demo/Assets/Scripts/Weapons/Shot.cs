using UnityEngine;
using System.Collections;

/* Base code for all shot objects.  All shots disapppear when they move offscreen
 */
public class Shot : Entity 
{
	public bool willBurn = false;
	public float headShot;
	public GameObject shooter;
	public Entity shooterScript;
	bool inLitArea=true;

	public void ShotStart()
	{
		EntityStart();
		headShot = 1;
		if (shooterScript == null) shooterScript = shooter.GetComponent<Entity>();
	}

	public void ShotUpdate()
	{
		EntityUpdate();
		if(health < 1) Destroy(gameObject);
		//if(health < 1) Stub for destruction animation control
		if(!inLitArea) Destroy(gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "LitArea") inLitArea=true;
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "LitArea") inLitArea=false;
	}
}
