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
}
