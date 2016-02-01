using UnityEngine;
using System.Collections;

public class Burning : Effect
{
	private float healthLoss;
	private int accChange;
	
	/// Set variables before the object is interacted with.
	void Awake()
	{
		effectName = "Burning";
		healthLoss = 0.3f;
		accChange = 2;
		stackDuration = false;
	}
	
	/// Change the state of the target unit on creation.
	void Start ()
	{
		targetUnit.statusEffects[3] = true;
		targetUnit.accMod += accChange;
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
		int num = targetUnit.GetComponents<Burning>().Length;
		targetUnit.accMod-= accChange;
		Debug.Log(num + "is the number of instances of burning before expiring");
		//targetUnit.statusEffects[0] = false;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
}
