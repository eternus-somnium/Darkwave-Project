using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{

	public bool mainActionFlag, secondaryActionFlag;
	bool ready;
	public int cooldown, currentCooldown=0;//Measured in quarter seconds
	public int energy = 100;
	public int currentEnergy;
	public int energyDrain = 0;

	Vector3 defaultPosition;
	public Vector3 secondaryPosition;
	internal Vector3 nextPosition;

	// Use this for initialization
	public void WeaponStart () 
	{
		defaultPosition = transform.localPosition;
		nextPosition=defaultPosition;
		currentEnergy = energy;
		InvokeRepeating("WeaponTime",0,.25f);
	}

	void WeaponTime()
	{
		if(currentEnergy < energy) currentEnergy++;
		if(currentCooldown == 0) ready=true;
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
