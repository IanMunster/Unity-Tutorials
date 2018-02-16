using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game manager.
/// 
/// </summary>

public class GameManager : MonoBehaviour {

	// Reference to BoardScript Component
	public BoardManager boardScript;

	// Current Level of Game (current level 3 for testingpurpose)
	private int level = 3;

	// Use this for initialization
	private void Awake () {
		// Get BoardManager script
		boardScript = GetComponent<BoardManager> ();
		// Call Initialize Game Function
		InitGame ();
	}


	// Function to Initialize the Game
	private void InitGame () {
		// Setup the GameBoard for Correct level
		boardScript.SetupScene (level);

	}



	// Update is called once per frame
	// private void Update () {}
}
