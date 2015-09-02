//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
	public Character character;

	protected void Start()
	{
		animator = GetComponent<Animator> ();
		character = GetComponent<Character> ();
		animator.SetInteger("WeaponPose", 1);
		gripR = character.weapons [character.weaponChoice].transform.Find ("GripPointR");
		gripL = character.weapons [character.weaponChoice].transform.Find ("GripPointL");
		bones = GetComponentsInChildren<Rigidbody>(); 
		foreach (Rigidbody bone in bones) bone.isKinematic = true;
		
	}

	protected void Update() 
	{
		//CameraController();
		MoveController();
		
		
		
		// Runs WeaponController() if character is still alive. Else, it runs DeathController().
		if(character.health>0)
		{
			WeaponController();
		}
		if(!character.Dying)
		{
			animator.enabled = false;
			foreach (Rigidbody bone in bones)
				bone.isKinematic = false;
			IKActive = false;
			headIK = false;
			InvokeRepeating("DeathController",0,1);
		}
	}

	void AnimationController(Vector3 direction, float runSpeed, float speedWithDir, float turn, bool isGrounded, float pitch)
	{
		animator.SetFloat("Speed",runSpeed);
		animator.SetFloat("SpeedWithDir",speedWithDir);
		animator.SetFloat("Turn", turn);
		animator.SetBool("IsGrounded", isGrounded);
		animator.SetFloat("Pitch", pitch);
	}

	void MoveController()
	{	
		Vector3 moveDirection = Vector3.zero;
		
		CharacterController controller = GetComponent<CharacterController>();
		if(character.health > 0)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);// makes input directions camera relative
			moveDirection *= character.baseSpeed * character.speedMod;
			
			if (controller.isGrounded) 
			{
				animator.SetFloat("JumpPower",character.JumpCounter);
			}
		}
		Vector3 withoutGravity = new Vector3 (moveDirection.x, 0, moveDirection.z);
		if (character.weaponChoice == 1)
		{
			character.weapons [character.weaponChoice].transform.position = rightHand.position + (character.weapons [character.weaponChoice].transform.position - character.weapons [character.weaponChoice].transform.Find ("GripPoint").position);
			character.weapons [character.weaponChoice].transform.rotation = rightHand.rotation * character.weapons [character.weaponChoice].transform.Find ("GripPoint").localRotation;
		}
		AnimationController (withoutGravity, withoutGravity.magnitude / character.baseSpeed, transform.InverseTransformDirection (withoutGravity).z / character.baseSpeed, transform.InverseTransformDirection (withoutGravity).x / character.baseSpeed, controller.isGrounded,0);
	}

	/*void CameraController()
	{
		float pitchScale = Camera.main.transform.eulerAngles.x;
		if (pitchScale > 90)
			pitchScale -= 360;
		AnimationController (Vector3.zero, 0, 0, Mathf.Clamp (animator.GetFloat("Turn") - (Input.GetAxis("Mouse X") / (character.horizontalSpeed)),-0.5f,0.5f), true, pitchScale / 90);
	}*/

	void SwitchWeapon(int choice, int pose, bool hands)
	{
		character.weapons [character.weaponChoice].SetActive (false);
		character.weaponChoice = choice;
		character.weapons [character.weaponChoice].SetActive (true);
		animator.SetInteger ("WeaponPose", pose);
		IKActive = hands;
		if (hands)
		{
			gripR = character.weapons [character.weaponChoice].transform.Find ("GripPointR");
			gripL = character.weapons [character.weaponChoice].transform.Find ("GripPointL");
		}
	}
	
	void WeaponController()
	{
		//Weapon chooser
		if(Input.GetKeyDown(KeyCode.Alpha1)) 
		{
			SwitchWeapon (0,1,true);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)) 
		{
			SwitchWeapon(1,2,false);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			SwitchWeapon (2,1,true);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			SwitchWeapon(3,3,false);
		}

		//Attack controller
		if (Input.GetButton ("Fire1")) animator.SetTrigger("Primary");

		//if(Input.GetButton("Fire2")) animator.SetTrigger("Secondary");
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
				animator.SetLookAtWeight(1);
				animator.SetLookAtPosition(Camera.main.transform.position + Camera.main.transform.forward);
			}
		}
	}

	void DeathController()
	{
		if(character.health > 0)
		{
			animator.enabled = true;
			GetComponent<CharacterController>().enabled = true;
			foreach (Rigidbody bone in bones) bone.isKinematic = true;
			character.weapons[character.weaponChoice].SetActive(true);
			CancelInvoke("DeathController");
			IKActive = true;
		}
		else
		{
			animator.enabled = true;
			foreach (Rigidbody bone in bones) bone.isKinematic = true;
			character.weapons[character.weaponChoice].SetActive(true);
			CancelInvoke("DeathController");
			IKActive = true;
			headIK = true;
		}
	}
}
