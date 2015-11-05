using UnityEngine;
using System.Collections;

public class Swiftness : Effect
{
	private float speed;
	
	/// Set variables before the object is interacted with.
	void Awake()
	{
		effectName = "Swiftness";
		speed = 0.33f;
		stackDuration = true;
		hasTrig = false;
	}
	
	/// Change the state of the target unit on creation.
	void Start ()
	{
		//targetUnit.statusEffects[6] = true;
		targetUnit.speedMod += speed;
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
	
	/// Reverses the effects of Start() and destroys this object.
	public override void EffectStop()
	{
		int num = targetUnit.GetComponents<Swiftness>().Length;
		Debug.Log(num + "is the number of instances of swiftness before expiring");
		//targetUnit.statusEffects[0] = false;
		if (hasTrig) targetUnit.speedMod -= speed;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
	
}
