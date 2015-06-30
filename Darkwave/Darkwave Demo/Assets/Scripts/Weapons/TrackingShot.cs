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
		gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*baseSpeed);
		//DEBUG
		target=GameObject.Find("Enemy Cube");

		if(target == null && Physics.Raycast(transform.position, transform.forward, out hit, sensorRange))
			if(hit.transform.gameObject.tag == "Enemy")
			{
				target = hit.transform.gameObject;
				Debug.Log("I SEE IT");
			}
		if(target != null) InvokeRepeating("CourseCorrection", .25f, .5f);
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
			gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*-baseSpeed);
			transform.rotation = Quaternion.LookRotation(target.transform.position);
			gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*baseSpeed);
			Debug.Log ("Tracking");
		}
		else CancelInvoke("CourseCorrection");
	}

	//Controls the shot's behavior when it hits something
	void OnCollisionEnter(Collision col)
	{
		this.health = 0;
	}
	
}
