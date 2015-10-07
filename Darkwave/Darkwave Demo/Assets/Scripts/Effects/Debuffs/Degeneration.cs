using UnityEngine;
using System.Collections;

public class Degeneration : Effect
{
	private float healthLoss;
	
	// Set values that need to be set earlier than Start().
	void Awake()
	{
		effectName = "Degeneration";
		healthLoss = 0.1f;
		stackDuration = false;
	}
	
	// Use this for initialization
	void Start ()
	{
		targetUnit.statusEffects[2] = true;
	}
	
	// Update is called once per frame
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
	
	// Reverses the effects of Start() and destroys the instance of this Regeneration class.
	public override void EffectStop()
	{
		int num = targetUnit.GetComponents<Degeneration>().Length;
		Debug.Log(num + "is the number of instances of degeneration before expiring");
		//targetUnit.statusEffects[0] = false;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
}
