using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
	public int secondaryActionType; //0 Zoom, 1 Secondary Attack
<<<<<<< HEAD
	public int baseAccuracy, augmentedAccuracy, ammoType;
	float zoom=0;
	bool aiming = false;
	public GameObject[] shot;
=======
	public float weaponAccuracy;
	public GameObject shot;
>>>>>>> Bring up to date
	private GameObject newShot;
	private Vector3 bulletSpread;


	// Use this for initialization
	void Start ()
	{
		WeaponStart();
		augmentedAccuracy = baseAccuracy;
		secondaryPosition = new Vector3(0,-0.2f,0);

	}

	// Update is called once per frame
	void Update ()
	{
		if(parent.GetComponentInChildren<Camera>()!=null)
			WeaponAim();

		MainAction();
		SecondaryAction();
	}

	void WeaponAim()
	{
		if(parent.GetComponent<FPSWithModel>().Target != Vector3.zero && !secondaryActionFlag)
			transform.LookAt(parent.GetComponent<FPSWithModel>().Target);
		else transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	public void MainAction()
	{
		if(mainActionFlag)
		{
			if(Ready && currentEnergy > energyDrain)
			{
				AttackAnimation();
				Vector3 shotSpawnPosition = gameObject.transform.position + gameObject.transform.forward * 1.25f;
				bulletSpread = new Vector3(
<<<<<<< HEAD
					Random.Range(-1f,1f)*(10-Mathf.Clamp(augmentedAccuracy + parent.GetComponent<Unit>().accMod,10,100)),
					Random.Range(-1f,1f)*(10-Mathf.Clamp(augmentedAccuracy + parent.GetComponent<Unit>().accMod,10,100)),
=======
					Random.Range(-1f,1f)*(10-Mathf.Clamp(weaponAccuracy + parent.GetComponent<Entity>().accMod,10,100)),
				    Random.Range(-1f,1f)*(10-Mathf.Clamp(weaponAccuracy + parent.GetComponent<Entity>().accMod,10,100)),
>>>>>>> Bring up to date
					0);
				Quaternion shotSpawnRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + bulletSpread);

				// Allows modifications to instanced shots.
<<<<<<< HEAD
				newShot = (GameObject)Instantiate(shot[ammoType], shotSpawnPosition, shotSpawnRotation);
				newShot.GetComponent<Shot>().touchDamage = Mathf.RoundToInt(touchDamage * parent.GetComponent<Unit>().dmgMod);
				newShot.GetComponent<Shot>().criticalMultiplier *= parent.GetComponent<Unit>().headShotMod;


				Ready=false;
				currentCooldown = augmentedCooldown;
				currentEnergy -= energyDrain;
=======
				newShot = (GameObject)Instantiate(shot, shotSpawnPosition, shotSpawnRotation);
				newShot.SendMessage("BulletModifications", parent);

				Ready=false;
				if (parent.GetComponent<Entity>().haste > 0) currentCooldown = cooldown / 4;
				else currentCooldown=cooldown;
				energy -= energyDrain;
>>>>>>> Bring up to date
			}
		}
	}

	public void SecondaryAction()
	{
		if(secondaryActionType==0)
		{
			transform.localPosition=Vector3.Lerp(gameObject.transform.localPosition,nextPosition, 5f*Time.deltaTime);
			if(secondaryActionFlag)
			{
				nextPosition=secondaryPosition;
				if(parent.GetComponentInChildren<Camera>()!=null && zoom < 1)
					parent.GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(60,30,zoom+=.05f);
				if(!aiming)
				{
					augmentedAccuracy++;
					parent.GetComponent<Unit>().augmentedSpeed *=.5f;
					aiming = true;
				}
				//gameObject.transform.localPosition = new Vector3(0,-0.7f,0);

			}
			else
			{
				nextPosition=DefaultPosition;
				if(parent.GetComponentInChildren<Camera>()!=null && zoom > 0)
					parent.GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(60,30,zoom-=.05f);
				if(aiming)
				{
					augmentedAccuracy--;
					parent.GetComponent<Unit>().augmentedSpeed *=2;
					aiming = false;
				}
				//gameObject.transform.localPosition = DefaultPosition;
			}
		}
	}
}
