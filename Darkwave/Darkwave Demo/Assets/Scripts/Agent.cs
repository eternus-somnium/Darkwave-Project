using UnityEngine;
using System.Collections;

/*
 * This code serves as the base for all game assets that will change dynamically during play.
 * Any variables created but not assigned in this script are either set in the child scripts or in the editor.
 * At the moment energy and max energy are not implemented but the plan is to have each use of an attack drain
 * energy that will be replenished over time by the status update function.
*/
public class Agent : Entity
{
	//Ability effects
	public int accMod=0; // Accuracy modifier
	public float defMod=0; // Defense modifier
	public float dmgMod=0; // Damage modifier
	public float headShotMod=0; // Extra critical damage

	public float focus; // Improves weapon accuracy by one unit.
	public float haste; // Decreases weapon cooldown by 300%.

	public float swift; // Increases speed by 33%.
	public float crippled; // Decreases speed by 50%.


	//Movement variables
	public float baseSpeed, speedMod, augmentedSpeed;	//set in editor
	internal float yMove = 0;

	//Combat Variables
	public bool causedHeadShot; // True if a headshot was made, then sets itself back to false after use.

	public void EntityStart()
	{
		health = maxHealth;
		aggroValue = baseAggroValue;
		augmentedSpeed = baseSpeed;
		causedHeadShot = false;
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
		if (health <= 0) empowered = focus = haste = regen = degen = burning = swift = crippled = armored = 0;
		if (empowered > 0) empowered = EffectsTimer(empowered);
		// Used in RangedWeapon.cs to lower the spread of ranged weapons.
		// Will be implemented in Melee to increase max targets
		if (focus > 0) focus = EffectsTimer(focus);
		// Used in RangedWeapon.cs to lower the cooldown of ranged weapons. To be added to Melee.
		if (haste > 0) haste = EffectsTimer(haste);
		// Regenerates health at a rate of 1 health per second.
		if (regen > 0)
		{
			regen = EffectsTimer(regen);
			health += Time.deltaTime;
			if (health > maxHealth) health = maxHealth;
		}
		// Degenerates health at a rate of 1 health per second.
		if (degen > 0)
		{
			degen = EffectsTimer(degen);
			health -= Time.deltaTime;
		}
		// Searing burns causes separate degen and worsens accuracy.
		if (burning > 0)
		{
			burning = EffectsTimer(burning);
			health -= (1.5F * Time.deltaTime);
		}
		// Increases movement speed.
		if (swift > 0) swift = EffectsTimer(swift);
		// Decreases movement speed.
		if (crippled > 0) crippled = EffectsTimer(crippled);
		// Decreases incoming damage.
		if (armored > 0) armored = EffectsTimer(armored);
		/* Adjust speed multiplier based on movement effects.
		 * Only the largest positive and largest negative modifiers affect the entity.
		 */
		speedMod = 1;
		if (swift > 0) speedMod += 0.33F;
		if (crippled > 0) speedMod -= 0.5F;
		// Adjust accuracy modifier based on certain effects.
		accMod = 0;
		if (focus > 0) accMod--;
		if (burning > 0) accMod += 3;
		// if defMod = 1, the entity takes no damage. Values above 1 causes damage to heal the entity.
		defMod = 0;
		if (armored > 0) defMod += 0.5F;
		// dmgMod of 1 is base damage. Higher means more damage, and vise versa.
		dmgMod = 1;
		if (empowered > 0) dmgMod += 0.25F;
	}

	protected void ResetHeadShot()
	{
		causedHeadShot = false;
		Debug.Log("On headshot triggers end");
	}

	float EffectsTimer(float effect)
	{
		effect -= Time.deltaTime;
		if (effect < 0) effect = 0;
		return effect;
	}

	// Parent method.
	public virtual Shot FoeDmgEffect(Shot shot, Agent foe)
	{
		Debug.Log("virtual FoeDmgEffect");
		return shot;
	}

	//Stub function for implementation of an animation controller
	protected virtual void AnimationController()
	{
		//if(stun) this.gameObject.
	}

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{

		if(col.gameObject.tag != "Shot" &&
		   ((gameObject.layer == 8 && col.gameObject.layer == 9) ||
		    (gameObject.layer == 9 && col.gameObject.layer == 8)))
		{
			col.gameObject.GetComponent<Entity>().DamageController(touchDamage, burning>0?true:false);
		}
	}

	void OnCollisionStay(Collision col)
	{
		if(col.gameObject.tag == "Death")
		{
			health = 0;
		}
	}
}
