using UnityEngine;
using System.Collections;

public class BuildDevice : Weapon 
{
	GameObject hitObject;
	Vector3 m_pos;

	public GameObject[] previewObjects, buildableObjects;
	public int[] objectCosts;
	public int selectedObject=0;

	public int placeable=0, range;

	// Use this for initialization
	void Start () 
	{
		WeaponStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(GameObject item in previewObjects)
			item.transform.rotation = Quaternion.identity;
		placeable = CheckPlacement();

		if(mainActionFlag) MainAction();
		if(secondaryActionFlag) SecondaryAction();
	}

	public void MainAction()
	{
		GameObject obj;
		if (Ready)
		{
			if(placeable == 1)
			{
				obj = GameObject.Instantiate(buildableObjects[selectedObject], m_pos, Quaternion.identity) as GameObject;
				GameObject.Find("Ground").GetComponent<Grid>().editGrid(obj, true);

				previewObjects[selectedObject].SetActive(false);
			}
			else if(placeable == 2)
			{
				obj = GameObject.Instantiate(buildableObjects[selectedObject], m_pos, Quaternion.identity) as GameObject;
				
				previewObjects[selectedObject].SetActive(false);
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
			previewObjects[selectedObject].SetActive(false);

			selectedObject = ++selectedObject % buildableObjects.Length;

			previewObjects[selectedObject].SetActive(true);

			Ready=false;
			currentCooldown=baseCooldown;
		}
		secondaryActionFlag=false;
	}

	int CheckPlacement()
	{
		RaycastHit hit;

		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, range) && //If the ray hit something
		    hit.transform.gameObject.GetComponent<Grid>() != null) //If the thing the ray hit had a grid component
		{
		    if(hit.transform.gameObject.GetComponent<Grid>().openSpace(hit.point))//If the hit.point is unoccupied on grid
			{
				#region can place
				previewObjects[selectedObject].SetActive(true);
				m_pos = hit.transform.gameObject.GetComponent<Grid>().getVector3(hit.point);//runs function to find vector to place
				m_pos.y = hit.point.y + buildableObjects[selectedObject].transform.position.y;
				previewObjects[selectedObject].transform.position = m_pos;
				return 1;//On grid and empty
				#endregion
			}
			else if (hit.transform.gameObject.GetComponent<BuildableObject>().canBeStackedOn && 
		         	 buildableObjects[selectedObject].GetComponent<BuildableObject>().canStackOn) //If the hit point is occupied but stackable
			{
				#region Wall Turret
				previewObjects[selectedObject].SetActive(true);
				m_pos = hit.transform.gameObject.GetComponent<Grid>().getVector3(hit.point); //WRONG
				//m_pos.y = hit.point.y + 1;
				previewObjects[selectedObject].transform.position = m_pos;
				return 2;//On grid and occupied by a wall 
				#endregion
				}
		}

		previewObjects[selectedObject].SetActive(false);
		return 0;//No position detected or position invalid
	}
}