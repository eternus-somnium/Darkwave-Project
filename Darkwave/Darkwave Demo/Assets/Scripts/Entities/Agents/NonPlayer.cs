using UnityEngine;
using System.Collections;

public class NonPlayer : Unit
{
	//Behavior variables (set in editor)
	public GameObject[] targetList;
	public GameObject 
		target = null,
		treasure;
	public float targetDistance;
	public int 
		behavior,
		sensorRange,
		engagementRange;
	public bool inSight;
	
	
	
	// Use this for initialization
	public void NonPlayerStart()
	{
		UnitStart();
		InvokeRepeating("ChooseTarget",0,2f);
	}
	
	// Update is called once per frame
	public void NonPlayerUpdate()
	{
		UnitUpdate();
		
		if(gameObject.tag == "Ally")
			targetList = GameObject.Find("Game Controller").GetComponent<GameController>().allyTargets;
		else targetList = GameObject.Find("Game Controller").GetComponent<GameController>().enemyTargets;
		
		if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, sensorRange))
		{
			inSight = true;
		}
		else inSight = false;
		
		if(health < 1)
		{
			if(treasure != null)
				Instantiate(treasure, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
			//Stub for destruction animation control
		}
	}
	
	void ChooseTarget()
	{
		if(targetList.Length != 0)
		{
			if(target == null)
			{
				target = targetList[0];
			}
			
			targetDistance = Vector3.Distance(gameObject.transform.position, target.transform.position);
			
			foreach (GameObject possibleTarget in targetList)
			{
				if(possibleTarget != null)
				{
					float possibleTargetDistance = Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position);
					if((possibleTarget.GetComponent<Entity>().aggroValue / possibleTargetDistance) >
					   (target.GetComponent<Entity>().aggroValue / targetDistance))
					{
						target = possibleTarget;
						targetDistance = possibleTargetDistance;
					}
				}
			}
		}
	}
	
	
	//Function controlling the usage of shot attacks. May eventually be expanded control of melee attacks.
	//Called by the child function when the conditions have been.
	public void MainAction()
	{
		weapons[WeaponChoice].SendMessage("MainActionController", true);
	}
}
