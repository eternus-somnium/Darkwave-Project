using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject[] enemies;
	public int enemyChoice, timeBetweenSpawns;
	Vector3 enemySpawnPosition;
	Quaternion enemySpawnRotation;


	// Use this for initialization
	void Start () 
	{
		enemySpawnPosition = this.transform.position;
		enemySpawnRotation = this.transform.rotation;
		InvokeRepeating("SpawnEnemies", 1, timeBetweenSpawns);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void SpawnEnemies()
	{
		Instantiate(enemies[enemyChoice], enemySpawnPosition, enemySpawnRotation);
	}
}
