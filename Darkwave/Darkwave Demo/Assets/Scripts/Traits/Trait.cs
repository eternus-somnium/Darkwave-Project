using UnityEngine;
using System.Collections;

public class Trait : MonoBehaviour
{
	protected GameObject player; // The Architect, in this case. Set in Start();
	protected Entity playerScript; // Set to the Architect's Architect.cs script.
	// Use this for initialization
	void Start()
	{
		player = this.gameObject;
		playerScript = player.GetComponent<Entity>();
	}
}
