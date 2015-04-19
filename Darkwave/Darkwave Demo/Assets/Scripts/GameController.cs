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
	public GUIText timer;
	public GUIText shards;

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
			playerHealth.text = "Health: " + playerOneScript.health.ToString("F2");
			playerBuffs.text = "Empowered: " + playerOneScript.empowered.ToString("F2") + 
				" Focus: " + playerOneScript.focus.ToString("F2") + 
				" Haste: " + playerOneScript.haste.ToString("F2") + 
				" Regen: " + playerOneScript.regen.ToString("F2") +
				" Swift: " + playerOneScript.swift.ToString("F2") +
				" Armored: " + playerOneScript.armored.ToString ("F2");
			playerDebuffs.text = "Degen: " + playerOneScript.degen.ToString("F2") +
				" Burning: " + playerOneScript.burning.ToString("F2") +
				" Crippled: " + playerOneScript.crippled.ToString("F2");
			currentWeapon.text = "Current Weapon: " + playerOneScript.weaponChoice;
			timer.text = "Round Time Left: " + Mathf.Floor(timeLeft/60).ToString("00") + 
				":" + (timeLeft%60).ToString("00") +
				" Round: " + round;
			shards.text = "Sha- Treasure?: " + playerOneScript.treasures;
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