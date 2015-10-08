using UnityEngine;
using System.Collections;

public class Empowered : Effect
{
	private float power; // The percent increase in outgoing damage.

	/// Set variables before object is interacted with.
	void Awake()
	{
		effectName = "Empowered";
		power = 0.25f;
		stackDuration = false;
	}

	/// Change the state of the target unit on creation.
	void Start ()
	{
		targetUnit.statusEffects[0] = true;
		targetUnit.dmgMod += power;
	}

	/// Counts down the duration. Stops the effect if duration reaches zero or lower.
	void Update ()
	{
		if (duration > 0) duration -= Time.deltaTime;
		if (duration <= 0)
		{
			EffectStop();
		}
	}

	// Reverses the effects of Start() and destroys this object.
	public override void EffectStop()
	{
		int num = targetUnit.GetComponents<Empowered>().Length;
		Debug.Log(num + "is the number of instances of empowered before expiring");
		//targetUnit.statusEffects[0] = false;
		targetUnit.dmgMod -= power;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}

}
