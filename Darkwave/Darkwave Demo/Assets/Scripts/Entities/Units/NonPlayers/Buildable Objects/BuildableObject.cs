using UnityEngine;
using System.Collections;

public class BuildableObject : NonPlayer 
{
	public bool 
		canStackOn = false, 
		canBeStackedOn = false,
		isPillar = false;
	public Vector3 offsetPosition;
	Grid placementGrid;

	public void BuildableObjectStart()
	{
		offsetPosition = transform.localPosition;
		NonPlayerStart();
		placementGrid = GameObject.Find("Ground").GetComponent<Grid>();
	}
	public void BuildableObjectUpdate()
	{
		if(health <=0) placementGrid.editGrid(this.gameObject, false);
		NonPlayerUpdate();
	}

	public Grid PlacementGrid {
		get {
			return placementGrid;
		}
		set {
			placementGrid = value;
		}
	}
}
