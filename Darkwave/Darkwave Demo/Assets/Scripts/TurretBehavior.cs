using UnityEngine;
using System.Collections;

public class TurretBehavior : Turret
{
	public GameObject Target;
	public Transform shot;
	private int time;
	private float turnSpeed;
    private Transform topTurret;
    private RaycastHit laser;
    private LineRenderer line;

    // Use this for initialization
	void Start () 
	{
		TurretStart();
        topTurret = transform.FindChild("TurretTop").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		TurretFixedUpdate();
		//spawns bullets in from topTurret object
		shotSpawnPosition = topTurret.position;
		//rotates topTurret toward target
		topTurret.rotation = shotSpawnRotation;
        //adds floating effect
        Vector3 temp = topTurret.position;
        temp.y = temp.y + Mathf.Clamp(Mathf.Sin(Time.time) * 0.003f, -0.5f, 1f);
        topTurret.position = temp;

        if (Target != null)
        {
            // point at target
            topTurret.LookAt(Target.transform);
        }
    }
}
