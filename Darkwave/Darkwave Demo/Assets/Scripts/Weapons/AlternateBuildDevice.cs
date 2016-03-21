using UnityEngine;
using System.Collections;

public class AlternateBuildDevice : Weapon 
{
	GameObject hitObject;
	Vector3 
	objectPosition;
	Quaternion objectRotation;
	
	public GameObject[] 
		placementObjects;
	public int[] objectCosts;
	public int selectedObject=0;
	
	public int 
		type=0, 
		range=30;
	
	// Use this for initialization
	void Start () 
	{
		WeaponStart();
		/*
		foreach(GameObject item in buildableObjects[1])
			item.transform.rotation = Quaternion.identity;
			*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		type = CheckPlacement();
		
		if(mainActionFlag) MainAction();
		if(secondaryActionFlag) SecondaryAction();
	}
	
	public void MainAction()
	{
		GameObject obj;
		if (Ready)
		{
			if(type == 1)
			{
				obj = GameObject.Instantiate(placementObjects[selectedObject], objectPosition, objectRotation) as GameObject;
				obj.GetComponent<Collider>().enabled = true;
				obj.layer = 8;
			}
			else if(type == 2)
			{
				hitObject.GetComponent<PlacementObject>().currentBuild++;
			}
			
			Ready=false;
			currentCooldown=baseCooldown;
		}
		mainActionFlag=false;
	}
	
	public void SecondaryAction()
	{
		if(Ready)
		{
			placementObjects[selectedObject].SetActive(false);
			
			selectedObject = ++selectedObject % placementObjects.Length;
			
			placementObjects[selectedObject].SetActive(true);
			
			Ready=false;
			currentCooldown=baseCooldown;
		}
		secondaryActionFlag=false;
	}
	
	int CheckPlacement()
	{

		RaycastHit hit;


		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, range)) //If the ray hit something
		{
			if(hit.normal.y >= 0 && // hit.normal.x + hit.normal.z && //Surface is mostly upright
				hit.transform.root.gameObject == GameObject.Find("Terrain")) //Surface is part of the terrain
			{
				objectPosition = placementObjects[selectedObject].transform.position = 
					hit.point + placementObjects[selectedObject].GetComponent<BuildableObject>().offsetPosition;
				objectRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

				placementObjects[selectedObject].transform.position = objectPosition;
				placementObjects[selectedObject].transform.rotation = objectRotation;

				placementObjects[selectedObject].SetActive(true);
				return 1;
			}
			else if(hit.transform.gameObject.GetComponent<PlacementObject>() != null)
			{
				placementObjects[selectedObject].SetActive(false);
				hitObject = hit.collider.gameObject;
				return 2;
			}
		}

		placementObjects[selectedObject].SetActive(false);
		return 0;//No position detected or position invalid
	}
}
