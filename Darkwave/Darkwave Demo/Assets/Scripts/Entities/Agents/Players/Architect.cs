using UnityEngine;
using System.Collections;

public class Architect : Character
{
	public int marksmanship; // Value from 0 to 9 determines how many attribute points are in Marksmanship.

	// References to Architect traits.
	private TraitHuntersMomemtum traitHuntersMomemtum;
	private TraitCriticalFinish traitCriticalFinish;

	public int structures; // Value from 0 to 9 determines how many attribute points are in Structures.
	public int perception; // Value from 0 to 9 determines how many attribute points are in Perception.

	/* Called on script initialization to setup the Architect player in the game world.
	 * Runs the parent Start() before initializng variables.
	 * It then runs the minor ability unlocking function to turn on minor unlocked abilities based on attribute points.
	 */
	new void Start()
	{
		base.Start();
		traitHuntersMomemtum = this.GetComponent<TraitHuntersMomemtum>();
		traitCriticalFinish = this.GetComponent<TraitCriticalFinish>();

		AbilityUnlockMinor(); // Sets the boolean minor traits to true or false based on attribute points.
	}

	/* Called when the player adjusts ability points just before level start.
	 * More attribute points mean more abilities
	 */
	void AbilityUnlockMinor()
	{
		// Sets abilities to true or false depending on how many points each trait line has.
		if (marksmanship >= 1) traitHuntersMomemtum.enabled = true;
		else traitHuntersMomemtum.enabled = false;
		if (marksmanship >= 4) traitCriticalFinish.enabled = true;
		else traitCriticalFinish.enabled = false;

		// AbilitySet(); // Activates the trait abilities.
	}

	// Called after setting ability points for looping attribute traits.
	void AbilitySet()
	{
		// Currently unused.
	}

	// Called when the player strikes a foe for ability effects that occur when striking a foe.
	public override Shot FoeDmgEffect(Shot shot, Unit foe)
	{
		Debug.Log("Architect FoeDmgEffect");
		if (traitCriticalFinish.enabled == true) shot.criticalMultiplier += traitCriticalFinish.Effect(foe);
		if (traitHuntersMomemtum.enabled == true) traitHuntersMomemtum.StartEffect();
		ResetHeadShot();
		return shot;
	}
}
