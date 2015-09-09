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
	public Text buffText;
	public Text debuffText;
	public GameObject gameController;
	public Text timerText;
	public Text shardsText;

	protected GameController gameControllerScript;

	protected float characterHealth;
	protected float oldCharacterHealth;
	protected float characterEmp;
	protected float characterFocus;
	protected float characterHaste;
	protected float characterRegen;
	protected float characterSwift;
	protected float characterArmor;

	protected float characterDegen;
	protected float characterBurn;
	protected float characterCrip;

	protected float characterWeaponSlot;

	protected float characterShards;
	// Use this for initialization
	protected void Start ()
	{
		oldCharacterHealth = characterHealth = characterScript.health;
		gameControllerScript = gameController.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	protected void Update ()
	{
		characterHealth = characterScript.health;
		characterEmp = characterScript.empowered;
		characterFocus = characterScript.focus;
		characterHaste = characterScript.haste;
		characterRegen = characterScript.regen;
		characterSwift = characterScript.swift;
		characterArmor = characterScript.armored;
		characterDegen = characterScript.degen;
		characterBurn = characterScript.burning;
		characterCrip = characterScript.crippled;
		characterWeaponSlot = characterScript.WeaponChoice;
		characterShards = characterScript.treasures;
		
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
