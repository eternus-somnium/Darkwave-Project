using UnityEngine;
using System.Collections;

public class TraitCriticalFinish : Trait
{
	public float critIncrease;
	public float healthThreshold;

	public float Effect(Agent foe)
	{
		if (foe.health <= foe.maxHealth * healthThreshold) return critIncrease;
		return 0;
	}
}
