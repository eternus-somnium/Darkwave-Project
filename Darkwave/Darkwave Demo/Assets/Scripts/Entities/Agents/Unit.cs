using UnityEngine;
using System.Collections;

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
		defMod=1, // Defense modifier
		dmgMod=1, // Damage modifier
		headShotMod=0; // Extra critical damage

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

	public AudioClip[] painSounds;
	public AudioClip[] deathSounds;

	public void UnitStart()
	{
		EntityStart();

	}

	//Function used to update entity status. Called from the fixed update of the child object
	public void UnitUpdate()
	{
		if(stun > 0) stun--;
		augmentedSpeed = baseSpeed + speedMod;
	}

	// Updates current effects on entity.
	public void EffectsController(int effect, int duration)
	{
		switch(effect)
		{
		case 0://empowered
			if(!statusEffects[0])
				dmgMod += .25f;
			else
				CancelInvoke("Empowered");
			statusEffects[0] = true;
			Invoke ("Empowered", duration);
			break;
		case 1://regen
			if(!statusEffects[1])
				InvokeRepeating("Regen",0,1);
			else
				CancelInvoke("StopRegen");
			statusEffects[1] = true;
			Invoke ("StopRegen", duration);
			break;
		case 2://degen
			if(!statusEffects[2])
				InvokeRepeating("Degen",0,1);
			else
				CancelInvoke("StopDegen");
			statusEffects[2] = true;
			Invoke ("StopDegen", duration);
			break;
		case 3://burning
			if(!statusEffects[3])
				InvokeRepeating("Burning",0,1);
			else
				CancelInvoke("StopBurning");
			statusEffects[3] = true;
			Invoke ("StopBurning", duration);
			break;
		case 4://armored
			if(statusEffects[4])
				CancelInvoke("Armored");
			statusEffects[4] = true;
			Invoke ("Armored", duration);
			break;
		case 5://focus
			if(!statusEffects[5])
				accMod += 1;
			else
				CancelInvoke("Focus");
			statusEffects[5] = true;
			Invoke ("Focus", duration);
			break;
		case 6://haste
			if(statusEffects[6])
				CancelInvoke("Haste");
			statusEffects[6] = true;
			Invoke ("Haste", duration);
			break;
		}
			/*.
			haste=0; // Decreases weapon cooldown by 300%.
			*/

	}

	public void DamageController(int baseDamage, bool isBurning)
	{
		if(statusEffects[4]) baseDamage /= 2; //statusEffects[4] is armored
		if(stun == 0) health -= baseDamage;
		PlaySound(painSounds[Random.Range(0,painSounds.Length)]);
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

	public void PlaySound(AudioClip sound)
	{
		GetComponent<AudioSource> ().clip = sound;
		GetComponent<AudioSource> ().Play();
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
