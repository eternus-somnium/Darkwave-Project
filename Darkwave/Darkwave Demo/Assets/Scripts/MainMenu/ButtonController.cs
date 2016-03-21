using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class ButtonController : MonoBehaviour
{
	// Starts the demo level.
	public void StartDemo()
	{
		Debug.Log("Button was pressed.");
		EditorSceneManager.LoadScene ("DarkwaveDemo");
	}

	// Closes the game.
	public void CloseGame()
	{
		Debug.Log("This should quit the game.");
		Application.Quit();
	}
}
