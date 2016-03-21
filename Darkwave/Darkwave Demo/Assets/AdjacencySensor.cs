using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AdjacencySensor : MonoBehaviour 
{
	public List<SensedAgent> sensedPillars = new List<SensedAgent>();
	float adjacencySensorRange;

	// Use this for initialization
	void Start () 
	{
		adjacencySensorRange = transform.parent.GetComponent<Pillar>().wallRange;
	}
	
	// Update is called once per frame
	void Update () 
	{
		sensor();
	}

	void sensor()
	{
		Vector3 aSensorRange = new Vector3(adjacencySensorRange*2,.1f,adjacencySensorRange*2);
		
		if(gameObject.transform.localScale != aSensorRange)
			gameObject.transform.localScale = aSensorRange;
	}	

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != transform.parent.gameObject &&
			other.gameObject.GetComponent<Pillar>() != null &&
			(sensedPillars.FirstOrDefault(o => o.agent == other.gameObject) == null))
				sensedPillars.Add (new SensedAgent(other.gameObject, this.gameObject));
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Wall")
			sensedPillars.Remove (sensedPillars.FirstOrDefault(o => o.agent == other.gameObject));
	}
}

public class SensedAgent
{
	public GameObject 
		agent,
		sensor;
	float 
		distance,
		relativeHeading;

	public SensedAgent(GameObject a, GameObject s)
	{
		agent = a;
		sensor = s;
	}

	public float Distance()
	{
		return distance = Vector3.Distance(agent.transform.position, sensor.gameObject.transform.position);
	}
}

