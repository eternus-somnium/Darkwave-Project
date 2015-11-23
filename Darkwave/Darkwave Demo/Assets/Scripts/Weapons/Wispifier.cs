using UnityEngine;
using System.Collections;

public class Wispifier: Weapon 
{
	
	// Use this for initialization
	void Start () 
	{
		WeaponStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mainActionFlag) MainAction();
	}
	
	public void MainAction()
	{

	}
}
