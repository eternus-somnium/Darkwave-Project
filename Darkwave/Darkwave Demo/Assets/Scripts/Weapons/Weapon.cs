using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{

	public bool 
		mainActionFlag,
		secondaryActionFlag,
		particleFlag,
		gridLinesFlag;
	public int touchDamage;
	bool ready;
<<<<<<< HEAD

	public float baseCooldown, augmentedCooldown, baseEnergy, augmentedEnergy, 
				 currentEnergy, currentCooldown=0, energyDrain = 0;
=======
	public float cooldown, currentCooldown=0;//Measured in quarter seconds
	public float energy = 100;
	public float currentEnergy;
	public float energyDrain = 0;
>>>>>>> Bring up to date
	public GameObject parent; // Entity wielding the weapon.

	Vector3 defaultPosition;

	public Entity charComponent;
	public Vector3 secondaryPosition;
	internal Vector3 nextPosition;

	// Use this for initialization
	public void WeaponStart () 
	{
<<<<<<< HEAD
<<<<<<< HEAD
		if(this.transform.parent.gameObject.name != "Main Camera")
		{
			parent = gameObject.transform.parent.gameObject;
		}
		else
		{
			parent = gameObject.transform.parent.parent.gameObject;
		}

		particleFlag = gameObject.GetComponentInChildren<ParticleSystem>() != null;
		defaultPosition = transform.localPosition;
		nextPosition=defaultPosition;
		augmentedEnergy=baseEnergy;
		augmentedCooldown=baseCooldown;
		currentEnergy = augmentedEnergy;
		InvokeRepeating("WeaponTime",0,.25f);

=======
		if(gameObject.layer != 8)
=======
		/*if(gameObject.layer != 8)
>>>>>>> TPS Update
			parent = gameObject.transform.parent.gameObject;
		else
			parent = gameObject.transform.parent.parent.gameObject;*/
		parent = transform.root.gameObject;
		defaultPosition = transform.localPosition;
		nextPosition=defaultPosition;
		currentEnergy = energy;
		
>>>>>>> Bring up to date
	}

	// Controls the weapon's fire rate and recharge
	protected void WeaponTime()
	{
<<<<<<< HEAD
<<<<<<< HEAD

		//Continuous energy recharge
		if(currentEnergy < augmentedEnergy) 
=======
		/*if(currentEnergy < augmentedEnergy) 
>>>>>>> Finished FPS Update
		{
			currentEnergy++;
			if(gameObject.activeSelf && particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isStopped) 
				gameObject.GetComponentInChildren<ParticleSystem>().Play();
		}
<<<<<<< HEAD
		else if(gameObject.activeSelf && particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isPlaying) 
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();
=======
		else if(particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isPlaying) 
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();*/
>>>>>>> Finished FPS Update
=======
		if(currentEnergy < augmentedEnergy) 
		{
			currentEnergy++;
			//if(particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isStopped) 
				//gameObject.GetComponentInChildren<ParticleSystem>().Play();
		}
		//else if(particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isPlaying) 
			//gameObject.GetComponentInChildren<ParticleSystem>().Stop();
>>>>>>> Update

		//Controls time between shots
		if(currentCooldown <= 0) ready=true;
		else if (parent.GetComponent<Unit>().statusEffects[6]) currentCooldown -= 4; //statusEffects[6] is haste
		else currentCooldown--;
	}

	public void AttackAnimation()
	{
		//Trigger animation built into weapon object
	}

	public void MainActionController(bool value)
	{
		mainActionFlag = value;
	}

	public void SecondaryActionController(bool value)
	{
		secondaryActionFlag = value;
	}

	public Vector3 DefaultPosition {
		get {
			return defaultPosition;
		}
		set {
			defaultPosition = value;
		}
	}

	public bool Ready {
		get {
			return ready;
		}
		set {
			ready = value;
		}
	}
}
