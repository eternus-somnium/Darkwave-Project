using UnityEngine;
using System.Collections;

//Code for the straight shots
public class StraightShot : Shot
{
	// Use this for initialization
	void Start () 
	{
		ShotStart();
		touchDamage = Mathf.RoundToInt(health);
		gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * baseSpeed);
	}

	void Update()
	{
		ShotUpdate();
		touchDamage = Mathf.RoundToInt(health);
	}

	//Controls the shot's behavior when it hits something
	void OnCollisionEnter(Collision col)
	{
		//Player layer object vs enemy layer object collisions
		if(col.gameObject.GetComponent<Entity>())
		{
			if (col.collider.material.name == "Head (Instance)")
			{
				parent.GetComponent<Entity>().causedHeadShot = true;
				criticalMultiplier += 0.5F;
			}

			/*
			Shot strikingShot = this.gameObject.GetComponent<Shot>();
			Entity struckFoe = col.gameObject.GetComponent<Entity>();

			strikingShot = shooterScript.FoeHit(strikingShot, struckFoe);
			*/
			/*
			Debug.Log("Headshot before FoeDmgEffect is " + criticalMultiplier);
			Debug.Log (parent + " is the shooter, " + parent.GetComponent<Entity>() + " is shooterScript, " + this.gameObject.GetComponent<Shot>() + " is the Shot script, and " + col.gameObject.GetComponent<Entity>() + " is the struck entity.");
			parent.GetComponent<Entity>().FoeDmgEffect(this.gameObject.GetComponent<Shot>(), col.gameObject.GetComponent<Entity>());
			Debug.Log("Headshot after FoeDmgEffect is " + criticalMultiplier);
			*/
			col.gameObject.GetComponent<Entity>().health -= (this.gameObject.GetComponent<Shot>().health * (1 - col.gameObject.GetComponent<Entity>().defMod) * criticalMultiplier);
			if(col.gameObject.GetComponent<Animator>())col.gameObject.GetComponent<Animator>().SetTrigger ("Pain");
			criticalMultiplier = 1;

			if (willBurn) col.gameObject.GetComponent<Entity>().burning = 1;
			Destroy(this.gameObject);
		}
		//If a shot hits the terrain it will zero out it's health
		else Destroy(this.gameObject);
	}

}
