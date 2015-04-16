using UnityEngine;
using System.Collections;

public class AttributeHuntersMomemtum : MonoBehaviour {

	public float cooldown; // The trait's base cooldown.

	public float focusIncrease; // How many seconds of focus is granted per periodica activation.

	private GameObject player; // The Architect, in this case. Set in Start();
	private Architect architect; // Set to the Architect's Architect.cs script.

	// Use this for initialization
	void Start ()
	{
		player = this.gameObject;
		architect = player.GetComponent<Architect>();
	}

	public void StartEffect()
	{
		InvokeRepeating("Effect",0,cooldown);
	}

	public void StopEffect()
	{
		CancelInvoke("Effect");
	}

	// Called every cooldown seconds.
	void Effect()
	{
		architect.focus += focusIncrease; // Increases the Architect's focus by focusIncrease.
	}
}
