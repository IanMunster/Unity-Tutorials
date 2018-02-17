using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game manager.
/// 
/// ! Should not have Enemy Behaviour (Maybe EnemyManager Needed?)
/// </summary>

public class GameManager : MonoBehaviour {

	// Singleton
	public static GameManager instance = null;
	// Reference to BoardScript Component
	public BoardManager boardScript;
	// Delay to show LevelImage and Text
	public float levelStartDelay = 2f;
	// Player Health (measured in FoodPoints)
	public int playerFoodPoints = 100;
	// Delay between Turns
	public float turnDelay = 0.1f;
	// Is it Players Turn to Move
	[HideInInspector] public bool playersTurn = true;

	// Current Level of Game (current level 3 for testingpurpose)
	private int level = 0;
	// Text to Diplay current level
	private Text levelText;
	// Reference to LevelImage
	private GameObject levelImage;
	// Is Game busy with Setup
	private bool doingSetup;
	// List of all Enemies to keep track and Move
	private List<Enemy> enemies;
	// Are the Enemies Moving
	private bool enemiesMoving;

	// Use this for initialization
	private void Awake () {
		// Check if no GameManager Instance
		if (instance == null) {
			// set this to Instance
			instance = this;
		// Otherwise if previous GameManager found
		} else if (instance != this) {
			// Destroy the GameManager
			Destroy (gameObject);
		}
		// Dont destroy GameManager on Scene load
		DontDestroyOnLoad (gameObject);
		// Create a new List for Enemies
		enemies = new List<Enemy> ();
		// Get BoardManager script
		boardScript = GetComponent<BoardManager> ();
		// Call Initialize Game Function
		if (level == 0) {
			InitGame ();
		}
	}


	// Function to Initialize the Game
	private void InitGame () {
		// Setup is busy
		doingSetup = true;
		// Find LevelImage
		levelImage = GameObject.Find("LevelImage");
		// Find LevelText
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		// Set levelText to Current LevelNumber
		levelText.text = "Day " + level;
		// Display LevelText and Image
		levelImage.SetActive (true);
		// Invoke the HideLevelImage function with delay
		Invoke ("HideLevelImage", levelStartDelay);
		// Clear previous List of Enemies
		enemies.Clear ();
		// Setup the GameBoard for Correct level
		boardScript.SetupScene (level);
	}


	// Called each time Scene is Loaded
	private void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode) {
		// Add level to LevelNumber
		level ++;

		// Call Game Init
		if (level != 1) {
			InitGame ();
		}

	}

	// Called when GameObject is Enabled
	private void OnEnable () {
		// Subscribe LevelFinishedLoading to SceneManager
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}


	// Called when GameObject is Disabled
	private void OnDisable () {
		// Unsubscribe LevelFinishedLoading from SceneManager
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}


	// Function to Hide the LevelImage
	private void HideLevelImage () {
		// Hide LevelImage
		levelImage.SetActive (false);
		// Setup is Done
		doingSetup = false;
	}


	// Function to Add Enemy to EnemiesList
	public void AddEnemyToList (Enemy script) {
		// Add the given Script to EnemiesList
		enemies.Add (script);
	}


	// Update is Called every Frame
	private void Update () {
		// Check if PlayerTurn or Enemies already Moving or Busy doing Setup
		if (playersTurn || enemiesMoving || doingSetup) {
			// Do nothing
			return;
		}
		// If its EnemiesTurn
		StartCoroutine (MoveEnemies() );
	}

	// Function to Call Move on all Enemies in List
	private IEnumerator MoveEnemies () {
		// The Enemies are Moving
		enemiesMoving = true;
		// Wait for Delay
		yield return new WaitForSeconds(turnDelay);
		// Check if No Enemie has been Spawned
		if (enemies.Count == 0) {
			// Let Player wait for delay
			yield return new WaitForSeconds(turnDelay);
		}
		// Go through all Enemies in List
		for (int i = 0; i < enemies.Count; i++) {
			// Call Move Enemy on List Index
			enemies[i].MoveEnemy ();
			// Wait for Enemies MoveTime (makes enemies move after eachother
			yield return new WaitForSeconds(enemies[i].moveTime);
		}
		// Enemies are Finished, Player Turn
		playersTurn = true;
		enemiesMoving = false;
	}


	// Function when GameOver (Player dead)
	public void GameOver () {
		// Show how many day Player Survived
		levelText.text = "You died from Starvation \n after " + level + " days.";
		// Display LevelImage and Text
		levelImage.SetActive(true);
		// Disable the GameManager
		enabled = false;
	}
}