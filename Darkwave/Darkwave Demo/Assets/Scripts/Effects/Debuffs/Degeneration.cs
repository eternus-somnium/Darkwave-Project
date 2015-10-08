using UnityEngine;
using System.Collections;

public class Degeneration : Effect
{
	private float healthLoss;
	
	/// Set variables before the object is interacted with.
	void Awake()
	{
		effectName = "Degeneration";
		healthLoss = 0.1f;
		stackDuration = false;
	}
	
	/// Change the state of the target unit on creation.
	void Start ()
	{
		targetUnit.statusEffects[2] = true;
	}
	
	/// Counts down the duration. Stops the effect if duration reaches zero or lower.
	void Update ()
	{
		if (targetUnit.health > 0)
		{
			targetUnit.health -= healthLoss * Time.deltaTime;
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
		int num = targetUnit.GetComponents<Degeneration>().Length;
		Debug.Log(num + "is the number of instances of degeneration before expiring");
		//targetUnit.statusEffects[0] = false;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
}
