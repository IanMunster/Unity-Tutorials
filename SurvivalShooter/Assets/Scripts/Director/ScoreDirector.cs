using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Score director.
/// Main function to keep track of Player Score & Score UI
/// </summary>

public class ScoreDirector : MonoBehaviour {

	//Players Score
	public static int score;
	//The HUD Experience Text
	private Text scoreText;


	// Start at start of game
	void Awake () {
		//Set text reference
		scoreText = GetComponent <Text> ();
		//Reset Current games Score
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Update the score to UI
		scoreText.text = "+" + score + " Exp!";
	}
}
