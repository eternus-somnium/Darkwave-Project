using UnityEngine;
using System.Collections;

public class Armored : Effect
{
	private float defense;
	
	// Update is called once per frame
	void Awake()
	{
		effectName = "Armored";
		defense = 0.50f;
		stackDuration = true;
		hasTrig = false;
	}
	
	void Start ()
	{
		targetUnit.statusEffects[4] = true;
		targetUnit.defMod += defense;
	}

	// Counts down the duration. Stops the effect if duration reaches zero or lower.
	void Update ()
	{
		if (duration > 0) duration -= Time.deltaTime;
		if (duration <= 0)
		{
			EffectStop();
		}
	}
	
	// Reverses the effects of Start() and destroys the instance of this Empowered class.
	public override void EffectStop()
	{
		int num = targetUnit.GetComponents<Armored>().Length;
		Debug.Log(num + "is the number of instances of armored before expiring");
		//targetUnit.statusEffects[0] = false;
		if (hasTrig) targetUnit.defMod -= defense;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
	
}
