using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //required to use NavMeshAgents (and other AI components)

/// <summary>
/// Enemy movement.
/// Uses NavMeshAgent to find the Player
/// </summary>

public class EnemyMovement : MonoBehaviour {

	//get the Players position
	private Transform player;
	//get the PlayerHealth script from the player
	private PlayerHealth playerHealth;
	//get the EnemyHealth script from enemy
	private EnemyHealth enemyHealth;
	//get the NavMeshAgent component from enemy
	private NavMeshAgent navigation;

	//Starts on Game Start
	void Awake () {
		//Get the position of the player
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		//Get the Health of the player
		playerHealth = player.GetComponent <PlayerHealth> ();
		//Get the Health of the enemy
		enemyHealth = GetComponent <EnemyHealth> ();
		//Get the NavMeshAgent of the enemy
		navigation = GetComponent <NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		//If the Enemy is Alive & player currently has health
		if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) {
			//Update the NavMeshAgent with Players Position
			navigation.SetDestination (player.position);
		//If the player has no health left
		} else {
			//Disable the NavMeshAgent
			navigation.enabled = false;
		}
	}
}
