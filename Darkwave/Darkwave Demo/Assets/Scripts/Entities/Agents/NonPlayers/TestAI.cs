using UnityEngine;
using System.Collections;

public class TestAI : NonPlayer {

	private float acceleration;
	private Vector3 targetDir;

	// Use this for initialization
	void Start () {
		NonPlayerStart();
	}
	
	// Update is called once per frame
	void Update () {

		NonPlayerUpdate();

		if (target != null) targetDir = transform.InverseTransformPoint (target.transform.position);
		else return;
		
		RaycastHit hit;
		
		if(target != null && targetDistance <= sensorRange)
		{
			transform.Rotate(new Vector3(0,targetDir.x,0).normalized);
			/*if(Mathf.Abs(Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg) < 60 * Mathf.Abs(GetComponent<Rigidbody>().angularVelocity.y))
			{
				GetComponent(ShipNav).directionVector.y = 0;
			}*/
			
			if(inSight && Random.value >= 0.5) WeaponMainAction(WeaponChoice);
		}
		if(GetComponent<Rigidbody>().SweepTest(transform.forward, out hit, (sensorRange / 5)) && hit.transform.gameObject.tag != "Shot")
		{
			var avoidDir = transform.InverseTransformPoint(hit.transform.position);
			transform.Rotate(new Vector3(0,-avoidDir.x,0).normalized);
			acceleration = 0;
		}

		acceleration = augmentedSpeed * Time.deltaTime;

		transform.Translate(new Vector3(0,0,acceleration));
	}
}