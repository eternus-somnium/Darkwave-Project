using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{

	public bool mainActionFlag, secondaryActionFlag, particleFlag;
	public int touchDamage;
	bool ready;

	public float baseCooldown, augmentedCooldown, baseEnergy, augmentedEnergy, 
				 currentEnergy, currentCooldown=0, energyDrain = 0;
	public GameObject parent; // Entity wielding the weapon.

	Vector3 defaultPosition;
	public Vector3 secondaryPosition;
	internal Vector3 nextPosition;

	// Use this for initialization
	public void WeaponStart () 
	{
		if(this.transform.parent.gameObject.name != "Main Camera")
		{
			parent = gameObject.transform.parent.gameObject;
			particleFlag = false;
		}
		else
		{
			parent = gameObject.transform.parent.parent.gameObject;
			particleFlag = true;
		}

		defaultPosition = transform.localPosition;
		nextPosition=defaultPosition;
		augmentedEnergy=baseEnergy;
		augmentedCooldown=baseCooldown;
		currentEnergy = augmentedEnergy;
		InvokeRepeating("WeaponTime",0,.25f);

	}

	// Controls the weapon's fire rate and recharge
	protected void WeaponTime()
	{
		if(currentEnergy < augmentedEnergy) 
		{
			currentEnergy++;
			//if(particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isStopped) 
				//gameObject.GetComponentInChildren<ParticleSystem>().Play();
		}
		//else if(particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isPlaying) 
			//gameObject.GetComponentInChildren<ParticleSystem>().Stop();

		if(currentCooldown <= 0) ready=true;
		else if (parent.GetComponent<Entity>().haste > 0) currentCooldown -= 4;
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
