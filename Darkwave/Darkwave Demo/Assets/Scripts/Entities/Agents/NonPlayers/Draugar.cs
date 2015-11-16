using UnityEngine;
using System.Collections;

public class Draugar : NonPlayer {

	private float acceleration;
	private Vector3 targetDir;
	public bool isWisp;
	
	// Use this for initialization
	void Start () {
		NonPlayerStart();
	}
	
	// Update is called once per frame
	void Update () {
		
		NonPlayerUpdate();
		
		if (target != null) targetDir = transform.InverseTransformPoint (target.transform.position);
		//if (target != null) targetDir = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 1, 0.0F);
		//if (target != null) targetDir = transform.position - target.transform.position;
		//if (target != null) targetDir = new Vector3( Vector3.Angle (transform.position, target.transform.position),0,0);
		else return;
		
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position,transform.forward, out hit, 5) && hit.transform.gameObject.tag != "Shot"  && hit.transform.gameObject.tag != "LitArea" && !isWisp)
		{
			Vector3 avoidDir = transform.InverseTransformPoint(hit.point);

			//print (hit.collider.name + ", " + avoidDir);
			Debug.DrawLine (transform.position, transform.TransformDirection(avoidDir), Color.red);
			//transform.Rotate(new Vector3(0,-avoidDir.x,0).normalized);
			if(targetDir.y < 0) transform.Rotate(new Vector3(0,-1,0));
			else if(targetDir.y > 0) transform.Rotate(new Vector3(0,1,0));
			//acceleration = 0;
		}
		else if(target != null)
		{
			transform.Rotate(new Vector3(0,targetDir.x,0).normalized * 1);
			/*if(Mathf.Abs(Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg) < 60 * Mathf.Abs(GetComponent<Rigidbody>().angularVelocity.y))
			{
				transform.Rotate(Vector3.zero);
			}*/
			
			//if(target != null && Physics.Raycast (transform.position, target.transform.position - transform.position, out hit, engagementRange) && hit.transform.gameObject == target.gameObject && Random.value >= 0.5) WeaponMainAction(WeaponChoice);
			
			acceleration = augmentedSpeed * Time.deltaTime;
		}
		
		transform.Translate(new Vector3(0,0,acceleration));
		Debug.DrawLine (transform.position, transform.TransformDirection(targetDir), Color.green);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == 11 && isWisp)
			Physics.IgnoreCollision (GetComponent<Collider> (), col.gameObject.GetComponent<Collider> ());
	}
}
