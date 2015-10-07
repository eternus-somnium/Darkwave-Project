using UnityEngine;
using System.Collections;

public class Regeneration : Effect
{
	private float healthGain;

	// Set values that need to be set earlier than Start().
	void Awake()
	{
		effectName = "Regeneration";
		healthGain = 0.1f;
		stackDuration = false;
	}

	// Use this for initialization
	void Start ()
	{
		targetUnit.statusEffects[1] = true;
	}
	
	// Update is called once per frame
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

	// Reverses the effects of Start() and destroys the instance of this Regeneration class.
	public override void EffectStop()
	{
		int num = targetUnit.GetComponents<Regeneration>().Length;
		Debug.Log(num + "is the number of instances of regeneration before expiring");
		//targetUnit.statusEffects[0] = false;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
}
