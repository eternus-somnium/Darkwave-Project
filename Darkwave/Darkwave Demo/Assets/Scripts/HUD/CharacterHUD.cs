using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

	protected bool 
		characterEmp,
		characterFocus,
		characterHaste,
		characterRegen,
		characterSwift,
		characterArmor,
		characterDegen,
		characterBurn;
	protected float characterCrip;

	protected int characterWeaponSlot;
	protected float characterWeaponEnergy;

	protected float characterShards;
	// Use this for initialization
	protected void Start ()
	{
		healthSlider.maxValue = characterScript.maxHealth;
		oldCharacterHealth = characterHealth = characterScript.health;
		gameControllerScript = gameController.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	protected void Update ()
	{


		characterHealth = characterScript.health;
		characterEmp = characterScript.statusEffects[0];
		characterFocus = characterScript.statusEffects[5];
		characterHaste = characterScript.statusEffects[6];
		characterRegen = characterScript.statusEffects[1];
		//characterSwift = characterScript.swift;
		characterArmor = characterScript.statusEffects[4];
		characterDegen = characterScript.statusEffects[2];
		characterBurn = characterScript.statusEffects[3];
		characterCrip = characterScript.crippled;
		characterWeaponSlot = characterScript.WeaponChoice;
		characterShards = characterScript.treasures;

		weaponScript = characterScript.weapons[characterWeaponSlot].GetComponent<Weapon>();
		characterWeaponEnergy = weaponScript.currentEnergy;
		
		healthSlider.value = characterHealth;
		healthText.text = "HP: " + characterHealth.ToString("F2");
		
		buffText.text = "";
		if (characterEmp) buffText.text += "Emp: " ;//+ characterEmp.ToString("F2") + " ";
		if (characterFocus) buffText.text += "Focus: ";// + characterFocus.ToString("F2") + " ";
		if (characterHaste) buffText.text += "Haste: ";// + characterHaste.ToString("F2") + " ";
		if (characterRegen) buffText.text += "Regen: ";// + characterRegen.ToString("F2") + " ";
		if (characterSwift) buffText.text += "Swift: ";// + characterSwift.ToString("F2") + " ";
		if (characterArmor) buffText.text += "Armor: ";// + characterArmor.ToString("F2") + " ";
		
		debuffText.text = "";
		if (characterDegen) debuffText.text += "Degen: ";// + characterDegen.ToString("F2") + " ";
		if (characterBurn) debuffText.text += "Burn: ";// + characterBurn.ToString("F2") + " ";
		//if (characterCrip) debuffText.text += "Crip: ";// + characterCrip.ToString("F2") + " ";
		
		weaponText.text = "Weapon Slot: " + characterWeaponSlot;
		//weaponEnergyText.text = "Weapon Energy: " + characterWeaponEnergy;
		
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
