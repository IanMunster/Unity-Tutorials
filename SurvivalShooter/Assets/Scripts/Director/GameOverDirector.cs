using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Required for SceneManager Components

/// <summary>
/// Game over director.
/// Main function for GameOver on playerDeath and GameOver UI animations
/// </summary>

public class GameOverDirector : MonoBehaviour {
	//reference to playerHealth
	[SerializeField] private PlayerHealth playerHealth;
	//reference to UI Animator
	private Animator anim;
	//delay before Restart Game
	[SerializeField] private float restartDelay = 5f; 
	//Restart Game timer
	private float restartTimer;

	//Starts at start of game
	void Awake (){
		//set references
		anim = GetComponent <Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//If the player has no health
		if (playerHealth.currentHealth <= 0) {
			//Set Animator to GameOver
			anim.SetTrigger ("GameOver");
			//set the timer to current
			restartTimer += Time.deltaTime;
			//if the timer reaches Delay
			if (restartTimer >= restartDelay) {
				//Restart the game
				SceneManager.LoadScene (00);
			}
		}

	}
}
