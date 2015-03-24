using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	public float roundTimer, timeLeft;
	public GameObject crystal;
	public GameObject litSphere;
	public GameObject[] allyTargets;
	public GameObject[] enemyTargets;
	public float sphereScale;

	// Use this for initialization
	void Start () 
	{
		roundTimer = timeLeft = 30f * 60;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeLeft-=Time.deltaTime;
		sphereScale = 100 + (roundTimer-timeLeft)*0.5f;
		litSphere.transform.localScale = new Vector3(sphereScale,sphereScale,sphereScale);
		allyTargets = GameObject.FindGameObjectsWithTag("Enemy");
		enemyTargets = GameObject.FindGameObjectsWithTag("Ally");
		if(crystal.GetComponent<Crystal>().health <=0)
			;//gameover
	}
}