using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{

	public bool mainActionFlag, secondaryActionFlag;
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
		if(gameObject.layer != 8)
			parent = gameObject.transform.parent.gameObject;
		else
			parent = gameObject.transform.parent.parent.gameObject;

		defaultPosition = transform.localPosition;
		nextPosition=defaultPosition;
		augmentedEnergy=baseEnergy;
		Debug.Log("OK");
		augmentedCooldown=baseCooldown;
		currentEnergy = augmentedEnergy;
		InvokeRepeating("WeaponTime",1,1);

	}

	// Controls the weapon's fire rate. Called in child script's Update().
	protected void WeaponTime()
	{
		if(currentEnergy < augmentedEnergy) currentEnergy++;
		if(currentCooldown <= 0) ready=true;
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
