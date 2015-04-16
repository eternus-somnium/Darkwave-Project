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
	public GameObject playerOne;
	private Character playerOneScript;

	public GUIText playerHealth;
	public GUIText playerBuffs;
	public GUIText playerDebuffs;
	public GUIText currentWeapon;

	// Use this for initialization
	void Start () 
	{
		playerOneScript = playerOne.GetComponent<Character>();
		timeLeft = roundTimer = roundTimer*60f;
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

	void OnGUI()
	{
		if (playerOneScript.enabled == true)
		{
			playerHealth.text = "Health: " + playerOneScript.health;
			playerBuffs.text = "Focus: " + playerOneScript.focus + 
				" Haste: " + playerOneScript.haste + 
				" Regen: " + playerOneScript.regen;
			playerDebuffs.text = "Degen: " + playerOneScript.degen;
			currentWeapon.text = "Current Weapon: " + playerOneScript.weaponChoice;
		}
	}

	void RoundController()
	{
		round++;
		enemiesLeft = enemiesPerRound * round;
		timeLeft=roundTimer;
	}

	void ListController()
	{
		allyTargets = GameObject.FindGameObjectsWithTag("Enemy");
		enemyTargets = GameObject.FindGameObjectsWithTag("Ally").Concat(GameObject.FindGameObjectsWithTag("Player")).ToArray();
	}

	void SphereController()
	{
		sphereScale = 100 + (roundTimer-timeLeft)*0.5f;
		litSphere.transform.localScale = new Vector3(sphereScale,sphereScale,sphereScale);
	}

	void EnemySpawner()
	{

	}

	void GameOver()
	{
		Debug.Log("GAME OVER");//gameover
	}
}