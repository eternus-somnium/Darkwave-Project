using UnityEngine;
using System.Collections;

public class PlacementObject : MonoBehaviour 
{
	public GameObject actualObject;
	public float 
		buildTime=1,
		previousBuild=0,
		currentBuild=0;
	public Material
		a,
		b;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		updateProgress();
	}

	void updateProgress()
	{

		if(currentBuild >= buildTime)
		{
			Instantiate(actualObject, transform.position, transform.rotation);
			Destroy(this.gameObject);
		}
		else if(previousBuild != currentBuild)
		{
			currentBuild = previousBuild;
			gameObject.GetComponent<Renderer>().material.color = 
				Color.Lerp(a.color, b.color, ((float)currentBuild/buildTime));
		}
	}

}
