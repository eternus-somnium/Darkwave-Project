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

		//if (target != null) targetDir = transform.InverseTransformDirection (target.transform.position - transform.position);
		//if (target != null) targetDir = transform.position - target.transform.position;
		if (target != null) targetDir =new Vector3( Vector3.Angle (transform.position, target.transform.position),0,0);
		else return;
		
		RaycastHit hit;

		if(Physics.Raycast(transform.position,transform.forward, out hit, (sensorRange / 5)) && hit.transform.gameObject.tag != "Shot")
		//if(GetComponent<Rigidbody>().SweepTest(transform.forward, out hit, 4) && hit.transform.gameObject.tag != "Shot")
		{
			//Vector3 avoidDir = transform.InverseTransformDirection(hit.point - transform.position);
			Vector3 avoidDir = transform.position - hit.point;

			print (hit.collider.name + ", " + avoidDir);
			Debug.DrawLine (transform.position, transform.TransformDirection(avoidDir), Color.red);
			transform.Rotate(new Vector3(0,avoidDir.x,0).normalized);
			if(avoidDir.x > -1 && avoidDir.x < 0) transform.Rotate(new Vector3(0,-1,0));
			else if(avoidDir.x < 1 && avoidDir.x > 0) transform.Rotate(new Vector3(0,1,0));
			acceleration = 0;
		}
		else if(target != null)
		{
			transform.Rotate(new Vector3(0,targetDir.x,0).normalized);
			/*if(Mathf.Abs(Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg) < 60 * Mathf.Abs(GetComponent<Rigidbody>().angularVelocity.y))
			{
				transform.Rotate(Vector3.zero);
			}*/
			
			//if(inSight && Random.value >= 0.5) WeaponMainAction(WeaponChoice);

			acceleration = augmentedSpeed * Time.deltaTime;
		}

		transform.Translate(new Vector3(0,0,acceleration));
		Debug.DrawLine (transform.position, transform.TransformDirection(targetDir), Color.green);
	}
}