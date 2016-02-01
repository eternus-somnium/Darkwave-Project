using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
	protected Unit sourceUnit; // The entity that created the effect.
	protected Unit targetUnit; // The entity that is being affected by the effect.
	protected Effect thisEffect;
	public string effectName; // Name of the effect.

	public float duration; // How long the effect lasts in seconds.
	public bool isLongest; // Is this the longest instance of the effect.
	public bool stackDuration;
	protected bool hasTrig;

	// Construcor to start a new effect.
	public void EffectStart (float newDuration, Unit newSourceUnit, Unit newTargetUnit)
	{
		duration = newDuration;
		sourceUnit = newSourceUnit;
		targetUnit = newTargetUnit;
	}

	// Updates the existing effect.
	public void EffectUpdate (float dur, Unit srcUnit)
	{
		duration += dur;
		sourceUnit = srcUnit;
	}

	public void SetHasTrigTrue()
	{
		hasTrig = true;
	}

	public Unit GetSrcUnit()
	{
		return sourceUnit;
	}
	
	public Unit GetTrgtUnit()
	{
		return targetUnit;
	}

	// Compares durations
	public int CompareTo(Effect other)
	{
		if (other == null) return 1;
		else if (duration > other.duration) return 1;
		else if (duration < other.duration) return -1;
		else return 0;
	}

	// Used in child class to stop the effect.
	public virtual void EffectStop (){}
}
