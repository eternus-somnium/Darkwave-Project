using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	bool inLitArea = false;
	public GameObject[] spawn;
	public int spawnChoice, timeBetweenSpawns;
	Vector3 spawnPosition;
	Quaternion spawnRotation;


	// Use this for initialization
	void Start () 
	{

		spawnRotation = transform.rotation;
		InvokeRepeating("SpawnEnemies", 1, timeBetweenSpawns);
	}

	void SpawnEnemies()
	{
		/*
		//Semi-random spawn positioning
		enemySpawnPosition = transform.position + new Vector3(Random.Range(-2,2),0,Random.Range(-2,2));
		if(gameObject.GetComponentInParent<GameController>().enemiesLeft > 0  && !inLitArea)
		{
			gameObject.GetComponentInParent<GameController>().enemiesLeft--;
			Instantiate(enemies[enemyChoice], enemySpawnPosition, enemySpawnRotation);
		}
		*/
		//Spawns within spawner
		spawnPosition = transform.position;
		if(gameObject.GetComponentInParent<GameController>().enemiesLeft > 0  && !inLitArea)
		{
			gameObject.GetComponentInParent<GameController>().enemiesLeft--;
			Instantiate(spawn[spawnChoice], spawnPosition, spawnRotation);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "LitArea")
		{
			Destroy (this);
		}
	}
}
