using UnityEngine;
using System.Collections;

public class Regeneration : Effect
{
	private float healthGain;

	/// Set variables before the object is interacted with.
	void Awake()
	{
		effectName = "Regeneration";
		healthGain = 0.1f;
		stackDuration = false;
	}

	/// Change the state of the target unit on creation.
	void Start ()
	{
		targetUnit.statusEffects[1] = true;
	}
	
	/// Counts down the duration. Stops the effect if duration reaches zero or lower.
	void Update ()
	{
		if (targetUnit.health < targetUnit.maxHealth)
		{
			targetUnit.health += healthGain * Time.deltaTime;
		}
		if (targetUnit.health > targetUnit.maxHealth)
		{
			targetUnit.health = targetUnit.maxHealth;
		}

		if (duration > 0) duration -= Time.deltaTime;
		if (duration <= 0)
		{
			EffectStop();
		}
	}

	/// Reverses the effects of Start() and destroys this object.
	public override void EffectStop()
	{
		int num = targetUnit.GetComponents<Regeneration>().Length;
		Debug.Log(num + "is the number of instances of regeneration before expiring");
		//targetUnit.statusEffects[0] = false;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
}
