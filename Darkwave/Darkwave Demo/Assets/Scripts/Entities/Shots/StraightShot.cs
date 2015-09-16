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
		//If the shot hit something on the opposing team
		if((gameObject.layer == 8 && col.gameObject.layer == 9) ||
		  (gameObject.layer == 9 && col.gameObject.layer == 8))
		{
			if (col.collider.material.name == "Head (Instance)")
			{
				touchDamage = Mathf.RoundToInt(touchDamage * criticalMultiplier);

				if(parent != null && parent.GetComponent<Character>() != null)
					parent.GetComponent<Character>().causedHeadShot = true;
			}

			col.gameObject.GetComponent<Unit>().DamageController(touchDamage, onFire);
			Debug.Log (gameObject + " hit " + col.gameObject);
		}
		 Destroy(this.gameObject);
	}

}
