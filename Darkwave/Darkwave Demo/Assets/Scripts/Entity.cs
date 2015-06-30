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
	public int baseAggroValue;
	public int aggroValue;
	public int stun=0;
	public int accMod; // Accuracy modifier
	public float defMod; // Defense modifier
	public float dmgMod; // Damage modifier
	public float headShotMod; // Extra critical damage

	//Effects Variables
	//public float[] effectArray = new float[10];
	public float empowered; // Increases damage by 25%.
	public float focus; // Improves weapon accuracy by one unit.
	public float haste; // Decreases weapon cooldown by 300%.
	public float regen; // Regenerates health by 1 point per second.
	public float degen; // Degenerates health by 1 point per second.
	public float burning; // Degenerates health by 1.5 points per second and worsens weapon accuracy.
	public float swift; // Increases speed by 33%.
	public float crippled; // Decreases speed by 50%.
	public float armored; // Decreases incoming damage by 50%.

	//Movement variables
	public float baseSpeed, speedMod;	//set in editor
	protected bool aiming; // Improves accuracy by one unit and slows down speed by 40%.

	internal float yMove = 0;

	//Combat Variables
	public bool causedHeadShot; // True if a headshot was made, then sets itself back to false after use.

	public void EntityStart()
	{
		health = maxHealth;
		aggroValue = baseAggroValue;
		speedMod = 0;
		accMod = 0;
		headShotMod = 0;
		aiming = false;
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
		else if (aiming) speedMod -= 0.40F;
		// Adjust accuracy modifier based on certain effects.
		accMod = 0;
		if (focus > 0) accMod--;
		if (aiming) accMod--;
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
	public virtual Shot FoeDmgEffect(Shot shot, Entity foe)
	{
		Debug.Log("virtual FoeDmgEffect");
		return shot;
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

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{
		if((stun == 0) &&
		  ((gameObject.layer == 8 && col.gameObject.layer == 9) ||
		   (gameObject.layer == 9 && col.gameObject.layer == 8)))
		{
			if(col.gameObject.tag == "Shot")
				gameObject.GetComponent<Entity>().health -= col.gameObject.GetComponent<Shot>().touchDamage;
			else
				gameObject.GetComponent<Entity>().health -= col.gameObject.GetComponent<Entity>().touchDamage;
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
