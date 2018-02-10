using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy director.
/// Used to Spawn enemy's if the player is alive (uses Instatiate)
/// </summary>

public class EnemyDirector : MonoBehaviour {

	//reference to the players health
	[SerializeField] PlayerHealth playerHealth;
	//prefab on Enemy to Spawn
	[SerializeField] GameObject enemy;
	//SpawnTime between each spawn
	[SerializeField] float spawnTime = 3f;
	//Array of multiple SpawnPoints on Level
	[SerializeField] Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
		//Repeatingly invoke the Spawn function
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	//Spawn Function is invoked at start and starts at each set interval
	void Spawn (){
		//if the player has no health
		if( playerHealth.currentHealth <=0f ){
			//Stop with spawning
			return;
		}
		Debug.Log ("SPAWN");
		//Get a random SpawnPointLocation from the SpawnPointsArray
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		//Instantiate a Enemy Prefab on the SpawnPoint
		Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
	}
}
