using UnityEngine;
using System.Collections;

//Code for the straight shots
public class StraightShot : Shot
{

	// Use this for initialization
	void Start () 
	{
		health = maxHealth;
		touchDamage = health;
	}
	
	// FixedUpdate is called at a set time interval
	void FixedUpdate () 
	{
		touchDamage = health;

		transform.Translate(new Vector3(0,0,baseSpeed*Time.deltaTime));
		EntityUpdate();
	}

	//Controls the shot's behavior when it hits something
	void OnCollisionEnter(Collision col)
	{
		//Player layer object vs enemy layer object collisions
		if((gameObject.layer == 8) && (col.gameObject.layer == 9) ||
		   (gameObject.layer == 9) && (col.gameObject.layer == 8))
		{
			col.gameObject.GetComponent<Entity>().health -= gameObject.GetComponent<Entity>().health;
			gameObject.GetComponent<Entity>().health -= col.gameObject.GetComponent<Entity>().touchDamage;

			//If a shot hits anything other than a shot it zeros out it's health.  If a shot hits another shot the weaker one is destroyed
			if(col.gameObject.tag == "Shot")
			{
				if(gameObject.GetComponent<Entity>().health <= col.gameObject.GetComponent<Entity>().health)
					gameObject.GetComponent<Entity>().health = 0;
			}
			else
				gameObject.GetComponent<Entity>().health = 0;

		}
		//If a shot hits the terrain it will zero out it's health
		else if(col.gameObject.layer == 0)
			gameObject.GetComponent<Entity>().health = 0;
	}

}
