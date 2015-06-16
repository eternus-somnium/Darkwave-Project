using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
	public int secondaryActionType; //0 Zoom, 1 Secondary Attack
	public float weaponAccuracy;
	public GameObject shot;
	private GameObject newShot;
	private Vector3 bulletSpread;


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
				bulletSpread = new Vector3(
					Random.Range(-1f,1f)*(10-Mathf.Clamp(weaponAccuracy + parent.GetComponent<Entity>().accMod,10,100)),
				    Random.Range(-1f,1f)*(10-Mathf.Clamp(weaponAccuracy + parent.GetComponent<Entity>().accMod,10,100)),
					0);
				Quaternion shotSpawnRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + bulletSpread);

				// Allows modifications to instanced shots.
				newShot = (GameObject)Instantiate(shot, shotSpawnPosition, shotSpawnRotation);
				newShot.SendMessage("BulletModifications", parent);

				Ready=false;
				if (parent.GetComponent<Entity>().haste > 0) currentCooldown = cooldown / 4;
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
