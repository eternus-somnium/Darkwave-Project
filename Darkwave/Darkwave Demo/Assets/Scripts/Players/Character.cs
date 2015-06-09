using UnityEngine;
using System.Collections;

public class Character : Entity 
{
	public int treasures=0;
	//Used in OnTriggerEnter() and healthController()
	bool inLitArea = true;
	float counter;
	//Used for MoveController()
	float jumpCounter = 0.0F;
	//Used in CameraController()
	float hRotation = 0F, vRotation = 0F;
	//Used in DeathController()
	int deathCounter = 0;
	float respawnTimer = 0;
	Vector3 respawnPoint;
	//Used in WeaponController()
	public int weaponChoice = 0;
	public GameObject[] weapons;

	protected void Start()
	{
		EntityStart();
		// Spawn point of the character.
		respawnPoint = new Vector3(
			GameObject.FindGameObjectWithTag("Respawn").transform.position.x+Random.Range(-1,1)*5,
			GameObject.FindGameObjectWithTag("Respawn").transform.position.y,
			GameObject.FindGameObjectWithTag("Respawn").transform.position.z+Random.Range(-1,1)*5);
		InvokeRepeating("healthRegenController",1,1);

	}

	// Called every frame.
	protected void Update() 
	{
		EntityUpdate();
		CameraController();

		MoveController();

		// Runs WeaponController() if character is still alive. Else, it runs DeathController().
		if(health>0)
		{ 
			WeaponController();
		}
		else DeathController();
	}

	// Controls Movement
	void MoveController()
	{
		float jumpSpeed = 20.0F;
		float jumpPower = .5F;

		Vector3 moveDirection = Vector3.zero;

		CharacterController controller = GetComponent<CharacterController>();
		if(health > 0)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);// makes input directions camera relative
			moveDirection *= baseSpeed * speedMod;

			if (controller.isGrounded) 
			{
				jumpCounter = jumpPower;
				moveDirection *= 2;
			}
			else if(!controller.isGrounded && !Input.GetButton("Jump")) jumpCounter = 0;
			else if(jumpCounter > 0) jumpCounter -= 1*Time.deltaTime;

			if (Input.GetButton("Jump") && jumpCounter > 0) moveDirection.y = jumpSpeed;
		}
		moveDirection.y += Physics.gravity.y;
		controller.Move(moveDirection * Time.deltaTime);
		
	}

	void CameraController()
	{
		float horizontalSpeed = 7.0F;
		float verticalSpeed = 7.0F;

		//Rotates Player on "X" Axis Acording to Mouse Input
		hRotation = (hRotation + horizontalSpeed * Input.GetAxis("Mouse X"))%360;
		transform.localEulerAngles = new Vector3(0, hRotation, 0);
		
		//Rotates Player on "Y" Axis Acording to Mouse Input
		vRotation = Mathf.Clamp(vRotation - verticalSpeed * Input.GetAxis("Mouse Y"), -90,90);
		Camera.main.transform.localEulerAngles = new Vector3(vRotation, 0, 0);

	}

	void WeaponController()
	{
		//Weapon chooser
		if(Input.GetKeyDown(KeyCode.Alpha1)) 
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=0;
			weapons[weaponChoice].SetActive(true);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)) 
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=1;
			weapons[weaponChoice].SetActive(true);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=2;
			weapons[weaponChoice].SetActive(true);

		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=3;
			weapons[weaponChoice].SetActive(true);
		}

		//Grid controller
		if(weaponChoice == 3) gameObject.GetComponentInChildren<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("GridLines");
		else gameObject.GetComponentInChildren<Camera>().cullingMask &=  ~(1 << LayerMask.NameToLayer("GridLines"));

		//Attack controller
		if(Input.GetButton("Fire1")) weapons[weaponChoice].SendMessage("MainActionController", true);
		else weapons[weaponChoice].SendMessage("MainActionController", false);
		
		if(Input.GetButton("Fire2"))
		{
			weapons[weaponChoice].SendMessage("SecondaryActionController", true);
			aiming = true;
		}
		else
		{
			weapons[weaponChoice].SendMessage("SecondaryActionController", false);
			aiming = false;
		}
	}

	// Regenerates health based on distance from crystal. Separate from and stacks with an Entity's regen float.
	void healthRegenController()
	{
		counter = (GameObject.Find("Game Controller").GetComponent<GameController>().sphereScale/2)-
					Vector3.Distance(gameObject.transform.position, 
			                 GameObject.Find("Game Controller").GetComponentInChildren<Crystal>().transform.position);

		if(inLitArea && health > 0 && health < maxHealth)
			health += counter / 1000;
		else if (!inLitArea && health > 0)
			health += counter / 100;
	}

	//Controls respawn timer and respawn position.
	void DeathController()
	{
		aggroValue = 0;
		if(respawnTimer == 0) respawnTimer = ++deathCounter * 10f;
		else if( respawnTimer > 0) respawnTimer-=Time.deltaTime;
		else
		{
			respawnTimer = 0;
			this.transform.position = respawnPoint;
			health = maxHealth;
		}
	}

	// OnTriggerEnter and Exit are called when entering and leaving triggers.

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "LitArea")
		{
			inLitArea=true;
		}
		if(col.gameObject.tag == "Treasure")
		{
			treasures++;
			Destroy(col.gameObject);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "LitArea")
		{
			inLitArea=false;
		}
	}
}
