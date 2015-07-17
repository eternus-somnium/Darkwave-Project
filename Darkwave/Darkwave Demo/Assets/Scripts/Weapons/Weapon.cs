using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{

	public bool mainActionFlag, secondaryActionFlag;
	bool ready;
	public float cooldown, currentCooldown=0;//Measured in quarter seconds
	public float energy = 100;
	public float currentEnergy;
	public float energyDrain = 0;
	public GameObject parent; // Entity wielding the weapon.

	Vector3 defaultPosition;
	public Vector3 secondaryPosition;
	internal Vector3 nextPosition;

	// Use this for initialization
	public void WeaponStart () 
	{
		/*if(gameObject.layer != 8)
			parent = gameObject.transform.parent.gameObject;
		else
			parent = gameObject.transform.parent.parent.gameObject;*/
		parent = transform.root.gameObject;
		defaultPosition = transform.localPosition;
		nextPosition=defaultPosition;
		currentEnergy = energy;
		
	}

	// Controls the weapon's fire rate. Called in child script's Update().
	protected void WeaponTime()
	{
		if(currentEnergy < energy) currentEnergy+=Time.deltaTime;
		if(currentCooldown <= 0) ready=true;
		else currentCooldown = Mathf.Clamp(currentCooldown - Time.deltaTime, 0, cooldown);
		parent.GetComponent<Animator>().SetBool("Attack",!ready);
		if (mainActionFlag) parent.GetComponent<Animator>().SetTrigger("Primary");
		if (secondaryActionFlag) parent.GetComponent<Animator>().SetTrigger("Secondary");
		print (mainActionFlag);
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
