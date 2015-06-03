using UnityEngine;
using System.Collections;

public class TrackingShot : Shot 
{
	public GameObject target = null;
	RaycastHit hit;
	public int maneuverability;
	public float sensorRange;

	// Use this for initialization
	void Start () 
	{
		ShotStart();
		touchDamage = Mathf.RoundToInt(health);

		if(target == null && Physics.Raycast(transform.position, transform.forward, out hit, sensorRange))
			if(hit.transform.gameObject.tag == "Enemy")
			{
				target = hit.transform.gameObject;
				Debug.Log("I SEE IT");
			}
		if(target != null) InvokeRepeating("CourseCorrection", .25f, .1f);
	}
	
	void Update()
	{
		ShotUpdate();
		touchDamage = Mathf.RoundToInt(health);
	}

	void CourseCorrection()
	{
		if(target != null)
		{
			transform.rotation = Quaternion.LookRotation(target.transform.position);

			gameObject.GetComponent<Rigidbody>().transform.position +=(transform.forward*baseSpeed);
		}
		else CancelInvoke("CourseCorrection");
	}

	//Controls the shot's behavior when it hits something
	void OnCollisionEnter(Collision col)
	{
		this.health = 0;
	}
	
}
