using UnityEngine;
using System.Collections;

public class TurretBehavior : Turret
{
	private int time;
	private float turnSpeed;
    private Transform topTurret;

    // Use this for initialization
	void Start () 
	{
		TurretStart();
        topTurret = transform.FindChild("TurretTop").transform;
	}
	
	// Update is called once per frame
	void Update()
	{

		//rotates topTurret toward target
		//topTurret.rotation = shotSpawnRotation;
		//adds floating effect
		Vector3 temp = topTurret.position;
		temp.y = temp.y + Mathf.Clamp(Mathf.Sin(Time.time) * 0.003f, -0.5f, 1f);
		topTurret.position = temp;

		//spawns bullets from topTurret object
		//shotSpawnPosition = topTurret.position;
		//if(target != null)
			//shotSpawnRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

		TurretUpdate();
	}
}
