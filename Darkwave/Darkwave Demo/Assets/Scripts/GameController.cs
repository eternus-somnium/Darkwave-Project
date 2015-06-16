using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
	public int round = 1, roundsInLevel, enemiesPerRound, enemiesLeft;
	public float roundTimer, timeLeft, sphereScale;
	public GameObject crystal, litSphere;
	public GameObject[] allyTargets, enemyTargets;


	// Use this for initialization
	void Start ()
	{
		timeLeft = roundTimer;
		enemiesLeft=enemiesPerRound;
	}

	// Update is called once per frame
	void Update ()
	{
		timeLeft-=Time.deltaTime;

		if(timeLeft < 0)
			RoundController();

		ListController();
		SphereController();

		if(crystal.GetComponent<Crystal>().health <=0)
			GameOver();
	}

	void RoundController()
	{
		round++;
		enemiesLeft += enemiesPerRound * round;
		timeLeft=roundTimer;
	}

	void ListController()
	{
		allyTargets = GameObject.FindGameObjectsWithTag("Enemy");
		enemyTargets = GameObject.FindGameObjectsWithTag("Ally").Concat(GameObject.FindGameObjectsWithTag("Player")).ToArray();
	}

	void SphereController()
	{
		sphereScale = 100 + ((round-1) * roundTimer + (roundTimer-timeLeft)) * 0.5f;
		litSphere.transform.localScale = new Vector3(sphereScale,sphereScale,sphereScale);
	}

	void GameOver()
	{
		Debug.Log("GAME OVER");//gameover
	}
}
