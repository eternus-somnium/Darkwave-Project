using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterHUD : MonoBehaviour
{
	public Character characterScript;

	public Slider healthSlider;
	public Text healthText;
	public Image damageFlashImage;
	public float damageFlashSpeed;
	public Color damageFlashColor;
	public Text weaponText;
	public Text weaponEnergyText;
	public Text buffText;
	public Text debuffText;
	public GameObject gameController;
	public Text timerText;
	public Text shardsText;

	protected GameController gameControllerScript;
	protected Weapon weaponScript;

	protected float characterHealth;
	protected float oldCharacterHealth;
	protected float characterEmp;
	protected Empowered longestEmp;
	protected float characterFocus;
	protected Focused longestFocus;
	protected float characterHaste;
	protected Hasted longestHaste;
	protected float characterRegen;
	protected Regeneration longestReg;
	protected float characterSwift;
	protected Swiftness longestSwift;
	protected float characterArmor;
	protected Armored longestArmor;

	protected float characterDegen;
	protected Degeneration longestDegen;
	protected float characterBurn;
	protected Burning longestBurn;
	protected float characterCrip;
	protected Crippled longestCrip;

	protected int characterWeaponSlot;
	protected float characterWeaponEnergy;

	protected float characterShards;
	// Use this for initialization
	protected void Start ()
	{
		oldCharacterHealth = characterHealth = characterScript.health;
		gameControllerScript = gameController.GetComponent<GameController>();
	}

	/*
	 * Updates the counters for the effects currently on the player.
	 * Called in Character.cs's NewEffect(), an override of a method in Unit.cs.
	 */
	public void updateEffectTimers(Effect effect)
	{
		Debug.Log("Called updateEffectTimers. effect name is " + effect.effectName);
		switch(effect.effectName)
		{
		case "Empowered":
		{
			if (effect.isLongest) longestEmp = (Empowered)effect;
			Debug.Log ("Setting longestEmp.");
			break;
		}
		case "Regeneration":
		{
			if (effect.isLongest) longestReg = (Regeneration)effect;
			Debug.Log ("Setting longestReg.");
			break;
		}
		case "Degeneration":
		{
			if (effect.isLongest) longestDegen = (Degeneration)effect;
			Debug.Log ("Setting longestDegen.");
			break;
		}
		case "Burning":
		{
			if (effect.isLongest) longestBurn = (Burning)effect;
			Debug.Log ("Setting longestBurn.");
			break;
		}
		case "Armored":
		{
			if (effect.isLongest) longestArmor = (Armored)effect;
			Debug.Log ("Setting longestArmor.");
			break;
		}
		case "Focused":
		{
			if (effect.isLongest) longestFocus = (Focused)effect;
			Debug.Log ("Setting longestFocus.");
			break;
		}
		case "Hasted":
		{
			if (effect.isLongest) longestHaste = (Hasted)effect;
			Debug.Log ("Setting longestHaste.");
			break;
		}
		case "Swiftness":
		{
			if (effect.isLongest) longestSwift = (Swiftness)effect;
			Debug.Log ("Setting longestSwift.");
			break;
		}
		case "Crippled":
		{
			if (effect.isLongest) longestCrip = (Crippled)effect;
			Debug.Log ("Setting longestCrip.");
			break;
		}
		default:
		{
			Debug.Log("Invalid effect.");
			break;
		}
		}
	}

	protected void Update()
	{
		characterHealth = characterScript.health;
		if (longestEmp)characterEmp = longestEmp.duration;
		else characterEmp = 0;
		if (longestFocus) characterFocus = longestFocus.duration;
		else characterFocus = 0;
		if (longestHaste) characterHaste = longestHaste.duration;
		else characterHaste = 0;
		if (longestReg) characterRegen = longestReg.duration;
		else characterRegen = 0;
		if (longestSwift) characterSwift = longestSwift.duration;
		else characterSwift = 0;
		if (longestArmor) characterArmor = longestArmor.duration;
		else characterArmor = 0;
		if (longestDegen) characterDegen = longestDegen.duration;
		else characterDegen = 0;
		if (longestBurn) characterBurn = longestBurn.duration;
		else characterBurn = 0;
		if (longestCrip) characterCrip = longestCrip.duration;
		else characterCrip = 0;
		characterWeaponSlot = characterScript.WeaponChoice;
		characterShards = characterScript.treasures;

		weaponScript = characterScript.weapons[characterWeaponSlot].GetComponent<Weapon>();
		characterWeaponEnergy = weaponScript.currentEnergy;
		
		healthSlider.value = characterHealth;
		healthText.text = "HP: " + characterHealth.ToString("F2");
		
		buffText.text = "";
		if (characterEmp > 0) buffText.text += "Emp: " + characterEmp.ToString("F2") + " ";
		if (characterFocus > 0) buffText.text += "Focus: " + characterFocus.ToString("F2") + " ";
		if (characterHaste > 0) buffText.text += "Haste: " + characterHaste.ToString("F2") + " ";
		if (characterRegen > 0) buffText.text += "Regen: " + characterRegen.ToString("F2") + " ";
		if (characterSwift > 0) buffText.text += "Swift: " + characterSwift.ToString("F2") + " ";
		if (characterArmor > 0) buffText.text += "Armor: " + characterArmor.ToString("F2") + " ";
		
		debuffText.text = "";
		if (characterDegen > 0) debuffText.text += "Degen: " + characterDegen.ToString("F2") + " ";
		if (characterBurn > 0) debuffText.text += "Burn: " + characterBurn.ToString("F2") + " ";
		if (characterCrip > 0) debuffText.text += "Crip: " + characterCrip.ToString("F2") + " ";
		
		weaponText.text = "Weapon Slot: " + characterWeaponSlot;
		weaponEnergyText.text = "Weapon Energy: " + characterWeaponEnergy;
		
		timerText.text = "Round Time Left: " + Mathf.Floor(gameControllerScript.timeLeft/60).ToString("00") + 
			":" + (gameControllerScript.timeLeft%60).ToString("00") +
				" Round: " + gameControllerScript.round;
		
		shardsText.text = "Shards: " + characterShards;
		
		if (oldCharacterHealth > characterHealth)
		{
			damageFlashImage.color = damageFlashColor;
		}
		else
		{
			damageFlashImage.color = Color.Lerp (damageFlashImage.color, Color.clear, damageFlashSpeed * Time.deltaTime);
		}
		
		oldCharacterHealth = characterHealth;
	}
}
