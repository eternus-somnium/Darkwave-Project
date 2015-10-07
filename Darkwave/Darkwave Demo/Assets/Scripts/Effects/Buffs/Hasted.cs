using UnityEngine;
using System.Collections;

public class Hasted : Effect
{
	//private float haste;
	
	// Update is called once per frame
	void Awake()
	{
		effectName = "Hasted";
		//haste = 4.0f;
		stackDuration = true;
		hasTrig = false;
	}
	
	void Start ()
	{
		targetUnit.statusEffects[6] = true;
		//targetUnit.somethingsomethinghaste;
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
		int num = targetUnit.GetComponents<Hasted>().Length;
		Debug.Log(num + "is the number of instances of hasted before expiring");
		//targetUnit.statusEffects[0] = false;
		//if (hasTrig) targetUnit.somethingsomethinghastereverse;
		targetUnit.ClearExpEffects();
		Destroy(this);
	}
	
}
