using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This code serves as the base for all game assets that will change dynamically during play.
 * Any variables created but not assigned in this script are either set in the child scripts or in the editor.
 * At the moment energy and max energy are not implemented but the plan is to have each use of an attack drain
 * energy that will be replenished over time by the status update function.
*/
public class Unit : Entity
{
	//Status variables
	public float 
		stun=0,
		accMod=0, // Accuracy modifier
		defMod=0, // Defense modifier
		dmgMod=0, // Damage modifier
		actMod=0, // Action speed modifier (reloading, firing rate, ability activation, etc.)
		headShotMod=0; // Extra critical damage
	public bool dying=false;

	//Status effects
	public bool[] statusEffects = new bool[10];
	/*
		0-empowered:	Increases damage by 25%.
		1-regen:		Regenerates health by 1 point per second. 
		2-degen:		Degenerates health by 1 point per second. 
		3-burning:		Degenerates health by 1.5 points per second and worsens weapon accuracy.
		4-armored:		Decreases incoming damage by 50%. 
		5-focus:		Improves weapon accuracy by one unit.
		6-haste:		Decreases weapon cooldown to 1/4th.
	*/

	public Effect tempEff;
	public Effect longestEmp;

	public List<List<Effect>> effects = new List<List<Effect>>();

	/// <summary>
	/// Removes expired effects from the effects List.
	/// </summary>
	public void ClearExpEffects()
	{
		int i, j;
		string effectName = "";
		for (i = 0; i < effects.Count; i++)
		{
			if (effects[i][0]) effectName = effects[i][0].effectName;
			for (j = 0; j < effects[i].Count; j++)
			{
				if (effects[i][j].duration <= 0)
				{
					effects[i].RemoveAt(j);
				}
			}
			if (effects[i].Count == 0)
			{
				effects.RemoveAt(i);
				if (effectName != "") ClearExpEffectsSwitch(effectName);
			}
			effectName = "";
		}
	}

	/// <summary>
	/// Sets the correct status effect boolean to false.
	/// </summary>
	/// <param name="effectName">Effect name.</param>
	protected void ClearExpEffectsSwitch(string effectName)
	{
		switch (effectName)
		{
		case "Empowered":
		{
			statusEffects[0] = false;
			break;
		}
		case "Regeneration":
		{
			statusEffects[1] = false;
			break;
		}
		case "Degeneration":
		{
			statusEffects[2] = false;
			break;
		}
		case "Burning":
		{
			statusEffects[3] = false;
			break;
		}
		case "Armored":
		{
			statusEffects[4] = false;
			break;
		}
		case "Focused":
		{
			statusEffects[5] = false;
			break;
		}
		case "Hasted":
		{
			statusEffects[6] = false;
			break;
		}
		default:
		{
			Debug.Log("Invalid effect name in ClearExpEffectsBool");
			break;
		}
		}
	}

	public void NewEffectSwitch(string effectName, int duration, Unit sourceUnit, Unit targetUnit)
	{
		Effect newEff = null;
		switch (effectName)
		{
		case "Empowered":
		{
			newEff = gameObject.AddComponent<Empowered>();
			break;
		}
		case "Regeneration":
		{
			newEff = gameObject.AddComponent<Regeneration>();
			break;
		}
		case "Degeneration":
		{
			newEff = gameObject.AddComponent<Degeneration>();
			break;
		}
		case "Burning":
		{
			newEff = gameObject.AddComponent<Burning>();
			break;
		}
		case "Armored":
		{
			newEff = gameObject.AddComponent<Armored>();
			break;
		}
		case "Focused":
		{
			newEff = gameObject.AddComponent<Focused>();
			break;
		}
		case "Hasted":
		{
			newEff = gameObject.AddComponent<Hasted>();
			break;
		}
		case "Swiftness":
		{
			newEff = gameObject.AddComponent<Swiftness>();
			break;
		}
		case "Crippled":
		{
			newEff = gameObject.AddComponent<Crippled>();
			break;
		}
		default:
		{
			Debug.Log("ERROR:Invalid effect name in NewEffectSwitch");
			break;
		}
		}
		
		if (newEff != null) newEff.EffectStart(duration, sourceUnit, targetUnit);
		else Debug.Log ("ERROR: newEff is null.");
		if (this.GetComponent<Character>()) this.GetComponent<Character>().NewEffect(newEff);
		else NewEffect(newEff);
	}

	///Called by an ability from a unit that applies an effect to the target unit.
	///This adds the effect to the Unit's 2D effects List.
	public void NewEffect(Effect newEff)
	{
		bool noStack = false;
		int i = 0;
		int j = 0;
		int numTypesEffects = effects.Count;
		longestEmp = null;
		
		// Searches for the right effects List to Add the new effect if it exists.
		while (i < numTypesEffects && noStack == false)
		{
			if (effects[i] != null && effects[i][0].effectName == newEff.effectName)
			{
				if (!newEff.stackDuration)
				{
					effects[i].Add(newEff);
					newEff.SetHasTrigTrue();
				}
				else
				{
					effects[i][0].EffectUpdate(newEff.duration, newEff.GetSrcUnit());
					newEff.EffectStop();
				}
				noStack = true;
			}
			i++;
		}

		// If the new effect doesn't have its own List, create one in effects List and Add it.
		if (i == numTypesEffects && noStack == false)
		{
			effects.Add(new List<Effect>());
			effects[i].Add(newEff);
			newEff.SetHasTrigTrue();
		}

		if (noStack == true) i--;
		// Sets the longest effect.
		for (j = 0; j < effects[i].Count; j++)
		{
			if(effects[i][j].isLongest)
			{
				longestEmp = effects[i][j];
			}
		}
		if (!longestEmp)
		{
			longestEmp = newEff;
			longestEmp.isLongest = true;
		}
		else if (newEff.duration > longestEmp.duration)
		{
			longestEmp.isLongest = false;
			longestEmp = newEff;
			longestEmp.isLongest = true;
		}
	}

	//Movement variables
	public float 
		baseSpeed, 
		speedMod, 
		augmentedSpeed,
		swift, // Increases speed by 33%.
		crippled; // Decreases speed by 50%.
	public Vector3 moveDirection;
	internal float yMove = 0;

	//Attack variables
	int weaponChoice  = 0;
	public GameObject[] weapons;
	Vector3 focusPoint; //Point in space where a ray from the center of the object hits a target

	public void UnitStart()
	{
		EntityStart();
		speedMod = 1.0f;
	}

	//Function used to update entity status. Called from the fixed update of the child object
	public void UnitUpdate()
	{
		if(stun > 0) stun--;
		augmentedSpeed = baseSpeed * speedMod;
	}

	// Updates current effects on entity.
	public void EffectsController(int effect, int duration)
	{
		switch(effect)
		{
		case 0://empowered
			/*
			if(!statusEffects[0])
				dmgMod += .25f;
			else
				CancelInvoke("Empowered");
			statusEffects[0] = true;
			Invoke ("Empowered", duration);
			*/
			break;
		case 1://regen
			/*
			if(!statusEffects[1])
				InvokeRepeating("Regen",0,1);
			else
				CancelInvoke("StopRegen");
			statusEffects[1] = true;
			Invoke ("StopRegen", duration);
			*/
			break;
		case 2://degen
			/*
			if(!statusEffects[2])
				InvokeRepeating("Degen",0,1);
			else
				CancelInvoke("StopDegen");
			statusEffects[2] = true;
			Invoke ("StopDegen", duration);
			*/
			break;
		case 3://burning
			/*
			if(!statusEffects[3])
				InvokeRepeating("Burning",0,1);
			else
				CancelInvoke("StopBurning");
			statusEffects[3] = true;
			Invoke ("StopBurning", duration);
			*/
			break;
		case 4://armored
			/*
			if(statusEffects[4])
				CancelInvoke("Armored");
			statusEffects[4] = true;
			Invoke ("Armored", duration);
			*/
			break;
		case 5://focus
			/*
			if(!statusEffects[5])
				accMod += 1;
			else
				CancelInvoke("Focus");
			statusEffects[5] = true;
			Invoke ("Focus", duration);
			*/
			break;
		case 6://haste
			/*
			if(statusEffects[6])
				CancelInvoke("Haste");
			statusEffects[6] = true;
			Invoke ("Haste", duration);
			*/
			break;
		}
			/*.
			haste=0; // Decreases weapon cooldown by 300%.
			*/

	}

	/// <summary>
	///  DamageController is called by another function that deals damage to this Unit.
	/// </summary>
	/// <param name="baseDamage">Base damage.</param>
	/// <param name="isBurning">If set to <c>true</c> is burning.</param>
	public void DamageController(float baseDamage, bool isBurning)
	{
		baseDamage *= (1 - defMod); //statusEffects[4] is armored
		if(stun == 0) health -= baseDamage;
		if(isBurning) EffectsController(3,10);
	}

	protected void ResetHeadShot()
	{
		Debug.Log("On headshot triggers end");
	}

	// Parent method.
	public virtual Shot FoeDmgEffect(Shot shot, Unit foe)
	{
		Debug.Log("virtual FoeDmgEffect");
		return shot;
	}

	//Status effects
	/*
	void Empowered()
	{
		dmgMod-=.25f;
		statusEffects[0] = false;
	}

	void Regen()
	{
		if(health < maxHealth) 
			health++;
	}
	void StopRegen()
	{
		CancelInvoke("Regen");
		statusEffects[1] = false;
	}
	void Degen()
	{
		if(health > 0) 
			health--;
	}
	void StopDegen()
	{
		CancelInvoke("Degen");
		statusEffects[2] = false;
	}
	*/
	/*
	void Burning()
	{
		if(health > 0) 
			health -= 1.5f;
	}
	void StopBurning()
	{
		CancelInvoke("Burning");
		statusEffects[3] = false;
	}
	void Armored()
	{
		statusEffects[4] = false;
	}
	void Focus()
	{
		accMod-=1;
		statusEffects[5] = false;
	}
	void Haste()
	{
		statusEffects[6] = false;
	}
	*/

	//Function controlling the usage of shot attacks. May eventually be expanded control of melee attacks.
	//Called by the child function when the conditions have been.
	public void WeaponMainAction(int chosenWeapon)
	{
		weapons[chosenWeapon].SendMessage("MainActionController");
	}

	public void WeaponSecondaryAction(int chosenWeapon)
	{
		weapons[chosenWeapon].SendMessage("SecondaryActionController");
	}

	//Stub function for implementation of an animation controller
	protected virtual void AnimationController()
	{
		//if(stun) this.gameObject.
	}

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{
		
		if(col.gameObject.tag != "Shot" &&
		   ((gameObject.layer == 8 && col.gameObject.layer == 9) ||
		 (gameObject.layer == 9 && col.gameObject.layer == 8)))
		{
			col.gameObject.GetComponent<Unit>().DamageController(touchDamage, statusEffects[3]);//statusEffects[3] is burning
		}
		
		if(col.gameObject.tag == "Death") 
			health = 0;
	}

	public Vector3 MoveDirection {
		get {
			return moveDirection;
		}
		set {
			moveDirection = value;
		}
	}

	public int WeaponChoice
	{
		get
		{
			return weaponChoice;
		}
		set
		{
			weaponChoice = value;
		}
	}

	public Vector3 FocusPoint 
	{
		get 
		{
			return focusPoint;
		}
		set 
		{
			focusPoint = value;
		}
	}
}
