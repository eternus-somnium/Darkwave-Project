using UnityEngine;
using System.Collections;

public class Focused : Effect
{
	private float focus;
	
	// Update is called once per frame
	void Awake()
	{
		effectName = "Focused";
		focus = 5.0f;
		stackDuration = true;
		hasTrig = false;
	}
	
	void Start ()
	{
		targetUnit.statusEffects[5] = true;
		targetUnit.accMod -= focus;
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
		int num = targetUnit.GetComponents<Focused>().Length;
		Debug.Log(num + "is the number of instances of focused before expiring");
		//targetUnit.statusEffects[0] = false;
		if (hasTrig) targetUnit.accMod += focus;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
	
}
