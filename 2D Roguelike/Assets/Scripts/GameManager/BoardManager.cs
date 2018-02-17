using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Board manager.
/// Layout Random Renegated GameBoard. Based on the Count of the Grid (count x count)
/// </summary>

public class BoardManager : MonoBehaviour {

	// Fuction to set Count of Grid
	[Serializable]
	public class Count {
		// Minimum Count of Grid
		public int minimum;
		// Maximum Count of Grid
		public int maximum;

		// Public function to Set Minimum and Maximum of GameBoard
		public Count (int min, int max) {
			// Set the Given Values
			minimum = min;
			maximum = max;
		}
	}


	// Amount of Columns of Grid
	public int columns = 8;
	// Amount of Rows of Grid
	public int rows = 8;
	// Specify RandomRange for Walls in Level (min 5, max 9)
	public Count wallCount = new Count (5, 9);
	// Specify RandomRange for FoodItems in Level (min 1, max 5)
	public Count foodCount = new Count (1, 5);
	// Hold prefab GameObject
	public GameObject exit;
	// Holds all Prefabs of Floors as GameObject
	public GameObject[] floorTiles;
	// Holds all Prefabs of Walls as GameObject
	public GameObject[] wallTiles;
	// Holds all Prefabs of OuterWalls as GameObject
	public GameObject[] outerWallTiles;
	// Holds all Prefabs of FoodItems as GameObject
	public GameObject[] foodTiles;
	// Holds all Prefabs of Enemy's as GameObject
	public GameObject[] enemyTiles;

	// Keep Hierarchy Clean by making Tiles Children of BoardHolder
	private Transform boardHolder;
	// Track all Possible Positions on Board
	private List <Vector3> gridPositions = new List <Vector3> ();


	// Function to Initialize the Grid (GameBoard)
	private void InitializeList () {
		// Clear Previous made GridPositions
		if (gridPositions != null) {
			gridPositions.Clear ();
		}

		// Go through all Possible Positions on X (-1 for OuterWall placement)
		for (int x = 0; x < columns - 1; x++) {
			// Go through all Possible Positions on Y (-1 for OuterWall placement)
			for (int y = 0; y < columns - 1; y++) { 
				// Add new Position to List
				gridPositions.Add ( new Vector3(x,y,0f) );
			}
		}
	}


	// Function to Setup the OuterWall and Floor of Grid (GameBoard0
	private void BoardSetup () {
		// Create a New BoardHolder
		boardHolder = new GameObject ("Board").transform;
		// Go through all Possible Positions on X (Build Edge)
		for (int x = -1; x < columns + 1; x++) {
			// Go through all Possible Positions on Y (Build Edge)
			for (int y = -1; y < columns + 1; y++) {
				// Set to Create a new FloorTile at Random
				GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length) ];
				// If X or Y is on the Outerline of GameBoard
				if (x == -1 || x == columns || y == -1 || y == rows) {
					// Set to Create a new OuterWall at Random
					toInstantiate = outerWallTiles[Random.Range (0, outerWallTiles.Length)];
				}
				// Instantiate given Tile
				GameObject instance = Instantiate (toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				// Set instatiated Tile to Child of BoardHolder
				instance.transform.SetParent (boardHolder);
			}
		}
	}


	// Function to give Walls, Enemy's and Food Tiles a Random Position
	private Vector3 RandomPosition () {
		// random Index between 0 and all Possible Grid Positions
		int randomIndex = Random.Range (0, gridPositions.Count);
		// Set a Random Postion on the Random Index
		Vector3 randomPosition = gridPositions[randomIndex];
		// Remove GripPoisition from List, so No dubble placing of Tiles
		gridPositions.RemoveAt (randomIndex);
		// Return RandomPosition
		return randomPosition;
	}


	// Spawn the Tiles at RandomPosition
	private void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum) {
		// Controlls how many of Given going to spawn
		int objectCount = Random.Range (minimum, maximum + 1);
		// Go through random generated ObjectCount
		for (int i = 0; i < objectCount; i++) {
			// Choose random Position to Spawn At
			Vector3 randomPosition = RandomPosition ();
			// Choose Tile to Spawn
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			// Instatiate Tile (spawn)
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}


	// Function called to Setup GameBoard in newScene (Called by GameManager)
	public void SetupScene (int level) {
		// Setup BoardGrip
		BoardSetup ();
		// Initialize Possible Positions
		InitializeList ();
		// Randomly Spawn WallTiles
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		// Randomly Spawn FoodTiles
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		// Set EnemyCount based on Level (lvl1 = 1, lvl2 = 2 etc)
		int enemyCount = (int)Mathf.Log(level, 2f);
		// Spawn the Enemy's 
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		// Create Level Exit (Always at same Place, upper Right)
		Instantiate (exit, new Vector3 (columns -1, rows -1, 0f) , Quaternion.identity);
	}
}