using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
	public int secondaryActionType; //0 Zoom, 1 Secondary Attack
	public float accuracy;
	public GameObject shot;
	private Vector3 bulletSpread;
	private Quaternion shotSpawnRotation;
	private Shot shotScript;
	private GameObject newShot;

	// Use this for initialization
	void Start ()
	{
		WeaponStart();
		secondaryPosition= new Vector3(0,-0.2f,0);

	}

	// Update is called once per frame
	void Update ()
	{
		MainAction();
		SecondaryAction();
		WeaponTime();
	}

	public void MainAction()
	{
		if(mainActionFlag)
		{
			AttackAnimation();
			if(Ready)
			{
				Vector3 shotSpawnPosition = gameObject.transform.position + gameObject.transform.forward * 1.25f;
				bulletSpread = new Vector3(Random.Range(-1f,1f)*
					(10-Mathf.Clamp(accuracy + entity.accMod,10,100)),Random.Range(-1f,1f)*
				    (10-Mathf.Clamp(accuracy + entity.accMod,10,100)),0);
				shotSpawnRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + bulletSpread);

				// Allows modifications to instanced shots.
				newShot = (GameObject)Instantiate(shot, shotSpawnPosition, shotSpawnRotation);
				shotScript = newShot.GetComponent<Shot>();
				shotScript.shooter = shooter;
				shotScript.shooterScript = entity;
				shotScript.maxHealth *= entity.dmgMod;
				shotScript.health *= entity.dmgMod;
				Ready=false;
				if (entity.haste > 0) currentCooldown = cooldown / 4;
				else currentCooldown=cooldown;
				energy -= energyDrain;
			}
		}
	}

	public void SecondaryAction()
	{
		transform.localPosition=Vector3.Lerp(gameObject.transform.localPosition,nextPosition, 5f*Time.deltaTime);
		if(secondaryActionFlag)
		{
			nextPosition=secondaryPosition;
			//gameObject.transform.localPosition = new Vector3(0,-0.7f,0);

		}
		else
		{
			nextPosition=DefaultPosition;
			//gameObject.transform.localPosition = DefaultPosition;
		}
	}
}
