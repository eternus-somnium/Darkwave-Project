using UnityEngine;
using System.Collections;

/*
 * This code serves as the base for all game assets that will change dynamically during play.
 * Any variables created but not assigned in this script are either set in the child scripts or in the editor.
 * At the moment energy and max energy are not implemented but the plan is to have each use of an attack drain 
 * energy that will be replenished over time by the status update function.
*/
public class Entity : MonoBehaviour 
{
	//Status Variables
	public float health;
	public float maxHealth;	//set in editor
	public int touchDamage;	//set in editor
	public int aggroValue=1;
	public int stun=0;
	public int accMod; // Accuracy modifier

	//Effects Variables
	public float empowered; // Increases damage.
	public float focus; // Improves weapon accuracy.
	public float haste; // Decreases weapon cooldown.
	public float regen; // Regenerates health.
	public float degen; // Degenerates health.
	public float burning; // Degenerates health and worsens weapon accuracy.
	public float swift; // Increases speed by 33%.
	public float crippled; // Decreases speed by 50%.

	//Movement variables
	public float baseSpeed, speedMod;	//set in editor
	
	internal float yMove = 0;
	
	public void EntityStart()
	{
		health = maxHealth;
		speedMod = 0;
		accMod = 0;
	}

	//Function used to update entity status. Called from the fixed update of the child object
	public void EntityUpdate()
	{
		if(stun > 0) stun--;
		EffectsUpdate();
	}

	// Updates current effects on entity.
	void EffectsUpdate()
	{
		if (health <= 0)
		{
			empowered = 0;
			focus = 0;
			haste = 0;
			regen = 0;
			degen = 0;
			burning = 0;
			swift = 0;
			crippled = 0;
		}
		if (empowered > 0)
		{
			empowered -= Time.deltaTime;
			if (empowered < 0) empowered = 0;
		}
		// Used in RangedWeapon.cs to lower the spread of ranged weapons.
		// Will be implemented in Melee to increase max targets
		if (focus > 0)
		{
			focus -= Time.deltaTime;
			if (focus < 0) focus = 0;
		}
		// Used in RangedWeapon.cs to lower the cooldown of ranged weapons. To be added to Melee.
		if (haste > 0)
		{
			haste -= Time.deltaTime;
			if (haste < 0) haste = 0;
		}
		// Regenerates health at a rate of 1 health per second.
		if (regen > 0)
		{
			regen -= Time.deltaTime;
			health += Time.deltaTime;
			if (health > maxHealth) health = maxHealth;
			if (regen < 0) regen = 0;
		}
		// Degenerates health at a rate of 1 health per second.
		if (degen > 0)
		{
			degen -= Time.deltaTime;
			health -= Time.deltaTime;
			if (degen < 0) degen = 0;
		}
		// Searing burns causes separate degen and worsens accuracy.
		if (burning > 0)
		{
			burning -= Time.deltaTime;
			health -= (1.5F * Time.deltaTime);
			if (burning < 0) burning = 0;
		}
		// Increases movement speed.
		if (swift > 0)
		{
			swift -= Time.deltaTime;
			if (swift < 0) swift = 0;
		}
		// Decreases movement speed.
		if (crippled > 0)
		{
			crippled -= Time.deltaTime;
			if (crippled < 0) crippled = 0;
		}
		// Adjust speed multiplier based on movement effects.
		speedMod = 1;
		if (swift > 0) speedMod += 0.33F;
		if (crippled > 0) speedMod -= 0.5F;
		// Adjust accuracy modifier based on certain effects.
		accMod = 0;
		if (focus > 0) accMod--;
		if (burning > 0) accMod += 3;
	}

	//Stub function for implementation of an animation controller
	protected virtual void AnimationController()
	{
		//if(stun) this.gameObject.
	}

	/*Function recognizes if an entity hits a Death object. Collisions don't occur between objects if they 
	 * are both on the player or enemy layers. There may be a better way to implement entities being affected 
	 * by terrain movement
	*/
	void OnCollisionStay(Collision col)
	{
		if(col.gameObject.tag == "Death")
		{
			health = 0;
		}
	}
}
