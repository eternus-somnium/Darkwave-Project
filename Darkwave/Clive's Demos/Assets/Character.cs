using UnityEngine;
using System.Collections;

public class Character : Entity 
{
	float jumpCounter = 0.0F;//Used for MoveExample()
	float hRotation = 0F, vRotation = 0F;//Used in CameraControlExample()

	int weaponChoice = 1;
	public GameObject weapon1;
	public GameObject weapon2;
	public GameObject weapon3;
	public GameObject weapon4;

	void Start()
	{
		EntityStart();
	}

	void Update() 
	{
		EntityUpdate();
		CameraControlExample();
		MoveExample();
		if(Input.GetButton("Fire1")) AttackExample();
	}

	void SimpleMoveExample()
	{
		float speed = 3.0F;
		float rotateSpeed = 3.0F;

		CharacterController controller = GetComponent<CharacterController>();
		transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		float curSpeed = speed * Input.GetAxis("Vertical");
		controller.SimpleMove(forward * curSpeed);
	}

	void MoveExample()
	{
		float walkSpeed = 6.0F;
		float jumpSpeed = 20.0F;
		float jumpPower = 1.0F;

		float gravity = 500.0F;
		Vector3 moveDirection = Vector3.zero;

		CharacterController controller = GetComponent<CharacterController>();

		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= walkSpeed;

		if (controller.isGrounded) 
		{
			jumpCounter = jumpPower;
			moveDirection *= 2;
		}
		else if(!controller.isGrounded && !Input.GetButton("Jump")) jumpCounter = 0;
		else if(jumpCounter > 0) jumpCounter -= 1*Time.deltaTime;

		if (Input.GetButton("Jump") && jumpCounter > 0) moveDirection.y = jumpSpeed;
			
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		
	}

	void CameraControlExample()
	{
		float horizontalSpeed = 7.0F;
		float verticalSpeed = 7.0F;

		//Rotates Player on "X" Axis Acording to Mouse Input
		hRotation = (hRotation + horizontalSpeed * Input.GetAxis("Mouse X"))%360;
		transform.localEulerAngles = new Vector3(0, hRotation, 0);
		
		//Rotates Player on "Y" Axis Acording to Mouse Input
		vRotation = Mathf.Clamp(vRotation - verticalSpeed * Input.GetAxis("Mouse Y"), -90,90);
		Camera.mainCamera.transform.localEulerAngles = new Vector3(vRotation, 0, 0);

	}

	void AttackExample()
	{
		switch(weaponChoice)
		{
		case 1:
			break;
		case 2:

			break;
		case 3:

			break;
		case 4:

			break;
		}
	}
}
