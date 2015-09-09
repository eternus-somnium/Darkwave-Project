using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	// Starts the demo level.
	public void StartDemo()
	{
		Debug.Log("Button was pressed.");
		Application.LoadLevel ("DarkwaveDemo");
	}

	// Closes the game.
	public void CloseGame()
	{
		Debug.Log("This should quit the game.");
		Application.Quit();
	}
}
