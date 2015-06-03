using UnityEngine;
using System.Collections;

public class BuildDevice : MonoBehaviour 
{

	GameObject hitObject;
	Vector3 m_pos;

	public GameObject[] previewObjects, buildableObjects;
	public int[] objectCosts;
	int selectedObject=0;

	public int placeable=0, range;
	public bool mainActionFlag, secondaryActionFlag;
	bool ready;
	int cooldown=1, currentCooldown;

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating("CooldownTime",0,.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(GameObject item in previewObjects)
			item.transform.rotation = Quaternion.identity;
		placeable = CheckPlacement();
		if(ready)
		{
			MainAction();
			SecondaryAction();
		}
	}

	public void MainActionController(bool value)
	{
		mainActionFlag = value;
	}
	
	public void SecondaryActionController(bool value)
	{
		secondaryActionFlag = value;
	}

	public void MainAction()
	{
		if(mainActionFlag)
		{
			GameObject obj;

			if(placeable == 1)
			{
				obj = GameObject.Instantiate(buildableObjects[selectedObject], m_pos, Quaternion.identity) as GameObject;
				GameObject.Find("Ground").GetComponent<Grid>().placeInGrid(obj);

				previewObjects[selectedObject].SetActive(false);
			}
			else if(placeable == 2)
			{
				obj = GameObject.Instantiate(buildableObjects[selectedObject], m_pos, Quaternion.identity) as GameObject;
				
				hitObject.GetComponent<Wall>().addTurret(obj);//gives object a reference of turret
				
				previewObjects[selectedObject].SetActive(false);
			}

			ready=false;
			currentCooldown=cooldown;
		}
	}
	
	public void SecondaryAction()
	{
		if(secondaryActionFlag)
		{
			previewObjects[selectedObject].SetActive(false);

			if(selectedObject == buildableObjects.Length-1)
				selectedObject=0;
			else selectedObject++;

			previewObjects[selectedObject].SetActive(true);

			ready=false;
			currentCooldown=cooldown;
		}
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
				m_pos.y = hit.point.y + .5f;
				previewObjects[selectedObject].transform.position = m_pos;
				return 1;//On grid and empty
				#endregion
			}
			else if (hit.transform.tag == "Wall" && selectedObject == 0)
			{
				#region Wall Turret

				hitObject = hit.transform.gameObject;
				
				if (!hitObject.GetComponent<Wall>().m_hasTurret && selectedObject == 0)
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

	void CooldownTime()
	{
		if(currentCooldown == 0) ready=true;
		else currentCooldown--;
	}
}