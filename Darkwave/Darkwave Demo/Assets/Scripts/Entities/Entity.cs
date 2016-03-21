using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
	public GameObject parent;
	//Status Variables
	public float 
		health, //set to maxHealth on start
		maxHealth;	//set in editor
	public int 
		touchDamage, //Damage done to opponents on collision
		baseAggroValue,
		aggroValue;

	public void EntityStart()
	{
		health = maxHealth;
		aggroValue = baseAggroValue;
	}

	//Controls reactions to collisions
	void OnCollisionEnter(Collision col)
	{

		if(col.gameObject.tag != "Shot" &&
			((gameObject.layer == 8 && col.gameObject.layer == 9) ||
				(gameObject.layer == 9 && col.gameObject.layer == 8)))
		{
			col.transform.root.gameObject.GetComponent<Entity>().DamageController(touchDamage);
		}

		if(col.gameObject.tag == "Death") 
			health = 0;
	}

	/// <summary>
	///  DamageController is called by another function that deals damage to this Unit.
	/// </summary>
	/// <param name="baseDamage">Base damage.</param>
	/// <param name="isBurning">If set to <c>true</c> is burning.</param>
	public void DamageController(float baseDamage)
	{
		health -= baseDamage;
	}
}
