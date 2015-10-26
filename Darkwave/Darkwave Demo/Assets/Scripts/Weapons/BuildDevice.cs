using UnityEngine;
using System.Collections;

public class BuildDevice : Weapon 
{

	GameObject hitObject;
	Vector3 m_pos;

	public GameObject[] previewObjects, buildableObjects;
	public int[] objectCosts;
	int selectedObject=0;

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

			if(selectedObject == buildableObjects.Length-1)
				selectedObject=0;
			else selectedObject++;

			previewObjects[selectedObject].SetActive(true);

			Ready=false;
			currentCooldown=baseCooldown;
		}
		secondaryActionFlag=false;
	}

	int CheckPlacement()
	{
		RaycastHit hit;

		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, range))
		{
			if (hit.transform.gameObject.GetComponent<Grid>() != null && hit.transform.gameObject.GetComponent<Grid>().canPlace(hit.point))//Checks to see if hit.point is occupied on grid
			{
				#region can place
				previewObjects[selectedObject].SetActive(true);
				m_pos = hit.transform.gameObject.GetComponent<Grid>().getVector3(hit.point);//runs function to find vector to place
				m_pos.y = hit.point.y + buildableObjects[selectedObject].transform.position.y;
				previewObjects[selectedObject].transform.position = m_pos;
				return 1;//On grid and empty
				#endregion
			}
			else if (hit.transform.tag == "Wall" && selectedObject == 0)
			{
				#region Wall Turret

				hitObject = hit.transform.gameObject;
				
				if (!hitObject.GetComponent<BuildableObject>().canBeStackedOn && buildableObjects[selectedObject].GetComponent<BuildableObject>().canStackOn)
				{
					previewObjects[selectedObject].SetActive(true);
					m_pos = GameObject.Find("Ground").GetComponent<Grid>().getVector3(hit.point);//runs function to find vector to place
					//m_pos.y = hit.point.y + 1;
					previewObjects[selectedObject].transform.position = m_pos;
					return 2;//On grid and occupied by a wall 
				}
				else previewObjects[selectedObject].SetActive(false);
				#endregion
			}
			else
			{
				previewObjects[selectedObject].SetActive(false);
			}
		}
		return 0;//No position detected or position invalid
	}
}