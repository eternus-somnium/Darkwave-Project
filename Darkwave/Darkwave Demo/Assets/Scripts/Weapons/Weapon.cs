using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	
	public bool 
		mainActionFlag = false,
		secondaryActionFlag = false,
		particleFlag,
		gridLinesFlag;
	public int 
		baseDamage;
	bool ready;

	public AudioClip primarySound,
		secondarySound;  
	
	public float 
		baseCooldown,
		augmentedCooldown,
		baseEnergy,
		augmentedEnergy,
		currentEnergy,
		currentCooldown=0,
		energyDrain = 0;
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
		
	}
	
	// Controls the weapon's fire rate and recharge
	protected void WeaponTime()
	{
		//
		//Continuous energy recharge
		if(currentEnergy < augmentedEnergy) 
		{
			currentEnergy++;
			if(gameObject.activeSelf && particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isStopped) 
				gameObject.GetComponentInChildren<ParticleSystem>().Play();
		}
		else if(gameObject.activeSelf && particleFlag && gameObject.GetComponentInChildren<ParticleSystem>().isPlaying) 
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		
		//Controls time between attacks
		if(currentCooldown <= 0) ready=true;
		else if (parent.GetComponent<Unit>().statusEffects[6]) currentCooldown -= parent.GetComponent<Unit>().actMod; //statusEffects[6] is haste
		else currentCooldown--;
	}
	
	public void AttackAnimation()
	{
		//Trigger animation built into weapon object
		if (GetComponent<AudioSource> ())
		{
			if(mainActionFlag) PlaySound (primarySound);
			else if(secondaryActionFlag) PlaySound (secondarySound);
		}
	}
	
	public void MainActionController()
	{
		mainActionFlag = true;
	}
	
	public void SecondaryActionController()
	{
		
		secondaryActionFlag = true;
	}

	public void PlaySound(AudioClip sound)
	{
		GetComponent<AudioSource> ().clip = sound;
		GetComponent<AudioSource> ().Play();
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
