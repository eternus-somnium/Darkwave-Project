using UnityEngine;
using System.Collections;

public class EnergySphere : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float scale = gameObject.GetComponent<Weapon>().currentEnergy / 
						gameObject.GetComponent<Weapon>().augmentedEnergy;
		gameObject.transform.localScale = new Vector3(scale,scale*10,scale);
			
	}
}
