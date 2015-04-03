using UnityEngine;
using System.Collections;

public class TurretBehavior : Weapon
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
		Target = null;
		time = 0;

        topTurret = transform.FindChild("TurretTop").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
        //adds floating effect
        Vector3 temp = topTurret.position;
        temp.y = temp.y + Mathf.Clamp(Mathf.Sin(Time.time) * 0.003f, -0.5f, 1f);
        topTurret.position = temp;

        if (Target != null)
        {
            // point at target
            topTurret.LookAt(Target.transform);
            time++;

            //lets time pass before another shot can be taken
            if (time > 10)
            {
                Instantiate(shot, topTurret.FindChild("bulletSpawn1").transform.position, topTurret.rotation);
                Instantiate(shot, topTurret.FindChild("bulletSpawn2").transform.position, topTurret.rotation);
                Instantiate(shot, topTurret.FindChild("bulletSpawn3").transform.position, topTurret.rotation);
                Instantiate(shot, topTurret.FindChild("bulletSpawn4").transform.position, topTurret.rotation);
                time = 0;
            }
        }
    }

	void OnTriggerStay(Collider other) 
	{
		if(other.gameObject.tag == "Enemy" && Target == null)
			Target = other.gameObject;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Target)
			Target = null;
	}
}
