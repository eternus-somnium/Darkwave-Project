using UnityEngine;
using System.Collections;

public class CharacterAnimations : MonoBehaviour
{
	private Rigidbody[] bones;
	private Animator animator;
	public Transform head;
	public Transform rightHand;
	public bool IKActive;
	public bool headIK;
	public Transform gripR;
	public Transform gripL;
	private Character character;

	protected void Start()
	{
		animator = GetComponent<Animator> ();
		character = GetComponent<Character> ();
		animator.SetInteger("WeaponPose", 1);
		gripR = character.weapons [character.WeaponChoice].transform.Find ("GripPointR");
		gripL = character.weapons [character.WeaponChoice].transform.Find ("GripPointL");
		bones = GetComponentsInChildren<Rigidbody>(); 
		foreach (Rigidbody bone in bones) bone.isKinematic = true;
		
	}

	protected void Update() 
	{
		CameraController();
		MoveController();

		// Runs WeaponController() if character is still alive. Else, it runs DeathController().
		if(character.health>0)
		{
			//WeaponController();
			if(character.WeaponChoice == 0 || character.WeaponChoice == 2) SwitchWeapon (1,true);
			else if(character.WeaponChoice == 1) SwitchWeapon(2,false);
			else if(character.WeaponChoice == 3) SwitchWeapon(3,false);

			//Attack controller
			if (Input.GetButton ("Fire1") && animator.GetBool("Attack") == false) {
				animator.SetTrigger ("Primary");
				animator.SetBool ("Attack", true);
			}
			
			if (Input.GetButton ("Fire2")) {
				animator.SetTrigger ("Secondary");
				animator.SetBool ("Attack", true);
			}
			//animator.ResetTrigger ("Primary");
		}
		else if(character.Dying)
		{

			//InvokeRepeating("DeathController",0,1);
			//print ("Blech");
		}
	}

	void AnimationController(Vector3 direction, float runSpeed, float speedWithDir, float turn, bool isGrounded)
	{
		animator.SetFloat("Speed",runSpeed);
		animator.SetFloat("SpeedWithDir",speedWithDir);
		animator.SetFloat("Turn", turn);
		animator.SetBool("IsGrounded", isGrounded);
	}

	void MoveController()
	{	
		CharacterController controller = GetComponent<CharacterController>();
		if(character.health > 0)
		{
			if (controller.isGrounded) 
			{
				animator.SetFloat("JumpPower",character.JumpCounter);
			}
		}
		if (character.WeaponChoice == 1)
		{
			character.weapons [character.WeaponChoice].transform.position = rightHand.position + (character.weapons [character.WeaponChoice].transform.position - character.weapons [character.WeaponChoice].transform.Find ("GripPoint").position);
			character.weapons [character.WeaponChoice].transform.rotation = rightHand.rotation * character.weapons [character.WeaponChoice].transform.Find ("GripPoint").localRotation;
		}
		Vector3 withoutGravity = new Vector3(character.MoveDirection.x, 0, character.MoveDirection.z);
		AnimationController (withoutGravity, withoutGravity.magnitude / character.baseSpeed, transform.InverseTransformDirection (withoutGravity).z / character.baseSpeed, transform.InverseTransformDirection (withoutGravity).x / character.baseSpeed, controller.isGrounded);
	}

	void CameraController()
	{
		Camera.main.transform.position = head.position + head.forward * 0.1f + head.transform.up * 0.15f;
	}

	void SwitchWeapon(int pose, bool hands)
	{
		animator.SetInteger ("WeaponPose", pose);
		IKActive = hands;
		if (hands)
		{
			gripR = character.weapons [character.WeaponChoice].transform.Find ("GripPointR");
			gripL = character.weapons [character.WeaponChoice].transform.Find ("GripPointL");
		}
	}
	
	void WeaponController()
	{
		//Weapon chooser
		if(Input.GetKeyDown(KeyCode.Alpha1)) 
		{
			SwitchWeapon (1,true);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)) 
		{
			SwitchWeapon(2,false);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			SwitchWeapon (1,true);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			SwitchWeapon(3,false);
		}
	}

	void OnAnimatorIK()
	{
		if (animator) {
			if (IKActive) {
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
			if(headIK)
			{
				animator.SetLookAtWeight(1,0.5f);
				animator.SetLookAtPosition(Camera.main.transform.position + Camera.main.transform.forward);
			}

		}
	}

	public Rigidbody[] Bones {
		get {
			return bones;
		}
		set {
			bones = value;
		}
	}
}
