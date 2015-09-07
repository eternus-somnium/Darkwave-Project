using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
	public GameObject parent;
	//Status Variables
	public float health;
	public float maxHealth;	//set in editor
	public int touchDamage;	//set in editor
	public int baseAggroValue;
	public int aggroValue;
	public int stun=0;

	//Status effects
	public float empowered; // Increases damage by 25%.
	public float regen; // Regenerates health by 1 point per second.
	public float degen; // Degenerates health by 1 point per second.
	public float burning; // Degenerates health by 1.5 points per second and worsens weapon accuracy.
	public float armored; // Decreases incoming damage by 50%.

	public void DamageController(int baseDamage, bool isBurning)
	{
		if(armored > 0) baseDamage /= 2;
		if(stun == 0) health -= baseDamage;
		if(isBurning) burning = 10;
	}
}
