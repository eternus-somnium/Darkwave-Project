using UnityEngine;
using System.Collections;

public class FPSWithModel : Entity 
{
	public int treasures=0;
	//Used in healthController()
	public int inLitArea=0;
	bool dying=false;
	//Used for MoveController()
	float jumpPower, jumpCounter = 0.0F;
	//Used in CameraController()
	float hRotation = 0F, vRotation = 0F;
	//Used in DeathController()
	int deathCounter = 0;
	float respawnTimer = -99;
	Vector3 respawnPoint;
	//Used in WeaponController()
	public int weaponChoice = 0;
	private Rigidbody[] bones;
	public GameObject[] weapons;
	public Vector3 target;
	private Animator animator;
	public Transform head;
	public Transform rightHand;
	public bool ikActive;
	public Transform gripR;
	public Transform gripL;
	
	protected void Start()
	{
		EntityStart();
		animator = GetComponent<Animator> ();
		animator.SetInteger("CurrWeap", 1);
		gripR = weapons [weaponChoice].transform.Find ("GripPointR");
		gripL = weapons [weaponChoice].transform.Find ("GripPointL");
		// Spawn point of the character.
		respawnPoint = new Vector3(
			GameObject.FindGameObjectWithTag("Respawn").transform.position.x+Random.Range(-1,1)*5,
			GameObject.FindGameObjectWithTag("Respawn").transform.position.y,
			GameObject.FindGameObjectWithTag("Respawn").transform.position.z+Random.Range(-1,1)*5);
		InvokeRepeating("healthRegenController",1,1);
		bones = GetComponentsInChildren<Rigidbody>(); 
		foreach (Rigidbody bone in bones) bone.isKinematic = true;
		
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
			dying = false;
			aggroValue = baseAggroValue + treasures;
			WeaponController();
		}
		else if(!dying)
		{
			dying=true;
			aggroValue = 0;
			GetComponent<CharacterController>().enabled = false;
			animator.enabled = false;
			foreach (Rigidbody bone in bones)
				bone.isKinematic = false;
			weapons[weaponChoice].SendMessage("MainActionController", false);
			weapons[weaponChoice].SendMessage("SecondaryActionController", false);
			weapons[weaponChoice].SetActive(false);
			ikActive = false;
			CancelInvoke("healthRegenController");
			InvokeRepeating("DeathController",0,1);
		}
	}
	
	// Controls Movement(old)
	/*
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
*/
	void MoveController()
	{	
		Vector3 moveDirection = Vector3.zero;
		
		CharacterController controller = GetComponent<CharacterController>();
		if(health > 0)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);// makes input directions camera relative
			moveDirection *= baseSpeed * speedMod;
			
			if (controller.isGrounded) 
			{
				if (Input.GetButton("Jump"))
				{
					if(jumpCounter == 0)
						jumpCounter = 1.5f;
					else
						jumpCounter+=Time.deltaTime; 
					if(jumpCounter > 2) jumpCounter=2;
				}
				else
				{
					jumpPower = jumpCounter*10;
					jumpCounter=0;
				}
				animator.SetFloat("JumpPower",jumpCounter);
			}
			else
			{
				jumpPower-=Time.deltaTime*5;
				moveDirection /=2;
			}
		}
		moveDirection.y = jumpPower + Physics.gravity.y;
		controller.Move(moveDirection * Time.deltaTime);
		Vector3 withoutGravity = new Vector3 (moveDirection.x, 0, moveDirection.z);
		if (weaponChoice == 1)
		{
			weapons [weaponChoice].transform.position = rightHand.position + (weapons [weaponChoice].transform.position - weapons [weaponChoice].transform.Find ("GripPoint").position);
			weapons [weaponChoice].transform.rotation = rightHand.rotation * weapons [weaponChoice].transform.Find ("GripPoint").localRotation;
		}
		float runSpeed = withoutGravity.magnitude/baseSpeed;
		animator.SetFloat("Speed",runSpeed);
		animator.SetFloat("SpeedWithDir",transform.InverseTransformDirection (withoutGravity).z/baseSpeed);
		animator.SetFloat("Turn", transform.InverseTransformDirection (withoutGravity).x/baseSpeed);
		animator.SetBool("IsGrounded", controller.isGrounded);
		
	}
	
	void CameraController()
	{
		float horizontalSpeed = 7.0F;
		float verticalSpeed = 7.0F;
		
		//Rotates Player on "X" Axis Acording to Mouse Input
		hRotation = (hRotation + horizontalSpeed * Input.GetAxis("Mouse X"))%360;
		transform.localEulerAngles = new Vector3(0, hRotation, 0);
		animator.SetFloat("Turn", Mathf.Clamp (animator.GetFloat("Turn") - (Input.GetAxis("Mouse X") / (horizontalSpeed)),-0.5f,0.5f));
		//Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y + 2,transform.position.z) - (Camera.main.transform.forward * 5);
		Camera.main.transform.position = head.position + head.forward * 0.1f + head.transform.up * 0.15f;

		//Rotates Player on "Y" Axis Acording to Mouse Input
		vRotation = Mathf.Clamp(vRotation - verticalSpeed * Input.GetAxis("Mouse Y"), -90,90);
		//Camera.main.transform.localEulerAngles = new Vector3(vRotation,head.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y, 0);
		Camera.main.transform.localEulerAngles = new Vector3(vRotation,0, 0);
		float pitchScale = Camera.main.transform.eulerAngles.x;
		if (pitchScale > 90)
			pitchScale -= 360;
		animator.SetFloat("Pitch", pitchScale / 90);
		
		RaycastHit hit;
		
		if(Physics.Raycast(GetComponentInChildren<Camera>().transform.position, 
		                   GetComponentInChildren<Camera>().transform.forward, out hit) && hit.collider.gameObject != gameObject)
			target = hit.point;
		else target = Vector3.zero;
	}
	
	void WeaponController()
	{
		//Weapon chooser
		if(Input.GetKeyDown(KeyCode.Alpha1)) 
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=0;
			weapons[weaponChoice].SetActive(true);
			animator.SetInteger("CurrWeap", 1);
			ikActive = true;
			gripR = weapons [weaponChoice].transform.Find ("GripPointR");
			gripL = weapons [weaponChoice].transform.Find ("GripPointL");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)) 
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=1;
			weapons[weaponChoice].SetActive(true);
			animator.SetInteger("CurrWeap", 2);
			ikActive = false;
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=2;
			weapons[weaponChoice].SetActive(true);
			animator.SetInteger("CurrWeap", 1);
			ikActive = true;
			gripR = weapons [weaponChoice].transform.Find ("GripPointR");
			gripL = weapons [weaponChoice].transform.Find ("GripPointL");
			
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			weapons[weaponChoice].SetActive(false);
			weaponChoice=3;
			weapons[weaponChoice].SetActive(true);
			animator.SetInteger("CurrWeap", 3);
			ikActive = false;
		}
		
		//Grid controller
		if(weaponChoice == 3) gameObject.GetComponentInChildren<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("GridLines");
		else gameObject.GetComponentInChildren<Camera>().cullingMask &=  ~(1 << LayerMask.NameToLayer("GridLines"));
		
		//Attack controller
		if (Input.GetButton ("Fire1")) {
			weapons [weaponChoice].SendMessage ("MainActionController", true);
			animator.SetTrigger("Primary");
		}
		else weapons[weaponChoice].SendMessage("MainActionController", false);
		
		if(Input.GetButton("Fire2")) weapons[weaponChoice].SendMessage("SecondaryActionController", true);
		else weapons[weaponChoice].SendMessage("SecondaryActionController", false);
	}

	void OnAnimatorIK()
	{
		if (animator) {
			if (ikActive) {
				animator.SetLookAtWeight(1);
				animator.SetLookAtPosition(Camera.main.transform.position + Camera.main.transform.forward);
				// Set the right hand target position and rotation, if one has been assigned
				if (gripR != null) {
					animator.SetIKPositionWeight (AvatarIKGoal.RightHand, 1);
					animator.SetIKRotationWeight (AvatarIKGoal.RightHand, 1);  
					animator.SetIKPosition (AvatarIKGoal.RightHand, gripR.position);
					animator.SetIKRotation (AvatarIKGoal.RightHand, gripR.rotation);
				}
				if (gripL != null) {
					animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
					animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1);  
					animator.SetIKPosition (AvatarIKGoal.LeftHand, gripL.position);
					animator.SetIKRotation (AvatarIKGoal.LeftHand, gripL.rotation);
				}
			}
		
		//if the IK is not active, set the position and rotation of the hand and head back to the original position
			else {          
				animator.SetIKPositionWeight (AvatarIKGoal.RightHand, 0);
				animator.SetIKRotationWeight (AvatarIKGoal.RightHand, 0);
				animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0);
				animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, 0);
				animator.SetLookAtWeight(0);
			}
		}
	}
	
	// Regenerates health based on distance from crystal. Separate from and stacks with an Entity's regen float.
	void healthRegenController()
	{
		float counter = (GameObject.Find("Game Controller").GetComponent<GameController>().sphereScale/2)-
			Vector3.Distance(gameObject.transform.position, 
			                 GameObject.Find("Game Controller").GetComponentInChildren<Crystal>().transform.position);
		
		if(inLitArea >= 1 && health < maxHealth)
			health += counter / 1000;
		else if (inLitArea < 1)
			health += counter / 100;
	}
	
	//Controls respawn timer and respawn position.
	void DeathController()
	{
		if(respawnTimer == -99) 
		{
			Debug.Log ("you are dying");
			respawnTimer = deathCounter+1 * 10f;
		}
		else if(health > 0)
		{
			respawnTimer = -99;
			animator.enabled = true;
			GetComponent<CharacterController>().enabled = true;
			foreach (Rigidbody bone in bones) bone.isKinematic = true;
			deathCounter++;
			weapons[weaponChoice].SetActive(true);
			dying=false;
			Debug.Log("someone helped you up");
			InvokeRepeating("healthRegenController",1,1);
			CancelInvoke("DeathController");
			ikActive = true;
		}
		else if( respawnTimer > 0) respawnTimer--;
		else
		{
			respawnTimer = -99;
			animator.enabled = true;
			GetComponent<CharacterController>().enabled = true;
			foreach (Rigidbody bone in bones) bone.isKinematic = true;
			deathCounter++;
			this.transform.position = respawnPoint;
			weapons[weaponChoice].SetActive(true);
			treasures = 0;
			health = maxHealth;
			dying=false;
			Debug.Log("you got better");
			InvokeRepeating("healthRegenController",1,1);
			CancelInvoke("DeathController");
			ikActive = true;
		}
	}
	
	public Vector3 Target 
	{
		get 
		{
			return target;
		}
		set 
		{
			target = value;
		}
	}
	
	// OnTriggerEnter and Exit are called when entering and leaving triggers.
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "LitArea")
		{
			inLitArea++;
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
			inLitArea--;
		}
	}
}
