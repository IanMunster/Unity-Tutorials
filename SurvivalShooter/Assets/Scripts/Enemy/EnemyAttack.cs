using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy attack.
/// 
/// </summary>

public class EnemyAttack : MonoBehaviour {

	//Time between attack from enemy
	[SerializeField] private float timeBetweenAttacks;
	//Total amount of Damage from attack
	[SerializeField] private int attackDamage = 10;

	//Enemy Animator Component
	private Animator anim;
	//Get the Player as GameObject
	private GameObject player;
	//Get the PlayerHealth Script
	private PlayerHealth playerHealth;
	//Get the Enemy's Health Script
//	EnemyHealth enemyHealth;
	//Boolean to check if the Player is within Attacking Range
	private bool isPlayerInRange;
	//Keep enemy attack on time (not too fast, slow etc)
	private float timer;

	// Use this for initialization
	void Awake () {
		//Find the Players GameObject by Searching for Player Tag
		player = GameObject.FindGameObjectWithTag ("Player");
		//Get the PlayersHealthScript Component from the player
		playerHealth = player.GetComponent <PlayerHealth> ();
		//Get the Enemy'sHealthScript Component from enemy
//		enemyHealth = GetComponent <EnemyHealth> ();
		//Get the Animator Component from enemy
		anim = GetComponent <Animator> ();
	}

	//When something enters the 3D Collision Trigger-box, this function is called
	void OnTriggerEnter (Collider other) {
		//When the other collider is the Player GameObject
		if (other.gameObject == player) {
			//The Player is within attack range
			isPlayerInRange = true;
		}
	}

	//When something exits the 3D Collision Trigger-box, this function is called
	void OnTriggerExit (Collider other) {
		//When the other collider is the Player GameObject
		if (other.gameObject == player) {
			//The Player is NOT in attack range
			isPlayerInRange = false;
		}
	}

	// Update is called once per frame
	void Update () {
		//
		timer += Time.deltaTime;
		//If the time between the last attack is greater than or equal to the TimeBetweenAttacks & 
		//the player is in Range & the Enemy is not Dead
		if (timer >= timeBetweenAttacks && isPlayerInRange /*&& enemyHealth != 0 */) {
			//Call the Attack function
			Attack ();
		}
		//If the Player is Dead
		if (playerHealth.currentHealth <= 0) {
			//Set the Enemy's Animation to PlayerDead aka Cheering
			anim.SetTrigger ("PlayerDead");
		}
	}

	//The Attack Function
	void Attack () {
		//Set the Attack-timer to zero
		timer = 0f;
		// If the player has any health
		if (playerHealth.currentHealth > 0) {
			//Apply the amount of attack Damage to the Players Health
			playerHealth.TakeDamage (attackDamage);
		}
	}
}
