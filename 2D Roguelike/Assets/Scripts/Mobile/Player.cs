using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required when using SceneManagers
using UnityEngine.UI; // Required when using UI

/// <summary>
/// MobilePlayer. Inherets from MovingObjects
/// 
/// ! SceneManagement and GameOver & RestartGame Function should NOT be handled here !
/// </summary>
namespace MobilePlayer {
public class Player : MovingObject {

	// Restart Level Delay
	public float restartLevelDelay = 1f;
	// Damage Player applies to Walls (when attack)
	public int wallDamage = 1;
	// Number of Points for Food when PickedUp
	public int pointPerFood = 10;
	// Number of Points for Soda when PickedUp
	public int pointPerSoda = 20;
	// UI Text of Current Food number
	public Text foodText;

	// SoundFX of Player
	public AudioClip moveSound0;
	public AudioClip moveSound1;
	public AudioClip eatSound0;
	public AudioClip eatSound1;
	public AudioClip drinkSound0;
	public AudioClip drinkSound1;
	// SoundFX when Player GameOver
	public AudioClip gameOverSound;

	// Reference to Animator Component of Player
	private Animator anim;
	// Stores PlayersFood before passing to GameManager
	private int food;
	// Vector2 for TouchInput on Mobile
	private Vector2 touchOrigin = -Vector2.one;


	// Use this for initialization
	protected override void Start () {
		// Get Animator Component
		anim = GetComponent <Animator> ();
		// Get the GameManagers FoodPoints at Start of Level
		food = GameManager.instance.playerFoodPoints;
		// Set foodText to loaded FoodPoints
		foodText.text = "Food & Drink: " + food + " left.";
		// Call Start of BaseClass (MovingObjects)
		base.Start ();
	}


	// Called when GameObject is Disabled
	private void OnDisable () {
		// Store Food Value to GameManager
		GameManager.instance.playerFoodPoints = food;
	}


	// Update is called once per frame
	private void Update () {
		// Check if Not Currently PlayersTurn
		if (!GameManager.instance.playersTurn || GameManager.instance.doingSetup) {
			// Do nothing
			return;
		}

		// Int to Store Movement (1 or -1) in Horizontal & Vertical Axis
		int horizontal = 0;
		int vertical = 0;
	
	
	// Check to make sure Input is from MobileDevice
	// Standalone or webGL, use Default Input
#if UNITY_STANDALONE

		// Get the Input for Horizontal & Vertical
		horizontal = (int) Input.GetAxisRaw ("Horizontal");
		vertical = (int) Input.GetAxisRaw ("Vertical");
		// Check if Moving Horizontal
		if (horizontal != 0) {
			// Prevent Player from moving Diagonally
			vertical = 0;
		}

	//Otherwise use MobileInput
#else

		if (Input.touchCount > 0) {
			Touch myTouch = Input.touches [0];
			if (myTouch.phase == TouchPhase.Began) {
				touchOrigin = myTouch.position;
			} else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
				Vector2 touchEnd = myTouch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;
				if (Mathf.Abs (x) > Mathf.Abs (y)) {
					horizontal = x > 0 ? 1 : -1;
				} else {
					vertical = y > 0 ? 1 : -1;
				}
			}
		}

#endif

		// Check for PlayerMovement
		if (horizontal != 0 || vertical != 0) {
			// If player moving call Attempt to Move <Can Expect Wall>
			AttemptMove<Wall> (horizontal, vertical);
		}
	}


	// Function to Attempt to Move (Override MoveObject)
	protected override void AttemptMove <T> (int xDir, int yDir) {
		// Call Attempt Move from MoveObject
		base.AttemptMove <T> (xDir, yDir);
		// Raycast for LineCast to move
		RaycastHit2D hit;

		// Check if Move was Possible
		if (Move(xDir, yDir, out hit) ) {
			// Evertime Player moves, Loses 1 FoodPoint
			food--;
			// Play Move sFX
			SoundManager.instance.RandomizeSfx (moveSound0, moveSound1);
		}
		// Set foodText to current FoodPoints
		foodText.text = "Food & Drink: " + food + " left.";

		// Check if Player is GameOver
		CheckIfGameOver ();
		// Players turn has Ended
		GameManager.instance.playersTurn = false;
	}


	// Function when Player Cant Move (Implementation From MoveObject)
	protected override void OnCantMove<T> (T component) {
		// If wall Blockes Path Cast the given Component as a Wall
		Wall hitwall = component as Wall;
		// Damaged the Wall
		hitwall.DamageWall (wallDamage);
		// Animate the Players Attack
		anim.SetTrigger ("Attack");
	}


	// Function to Check Collisions
	private void OnTriggerEnter2D (Collider2D other) {
		// Check OtherCollider Tag
		switch (other.tag) {
		case "Exit": 
			// Call Restart to make new Level
			Invoke ("Restart", restartLevelDelay);
			// Disable the player from moving
			enabled = false;
			break;
		case "Food":
			// Add Found Food to FoodPoints
			food += pointPerFood;
			// Display foodText FoodPickedUp
			foodText.text = "Found " + pointPerFood + " Food! \n" + food + " Food & Drink left.";
			// Play EatFood sFX
			SoundManager.instance.RandomizeSfx (eatSound0, eatSound1);
			// Disable Food Item
			other.gameObject.SetActive (false);
			break;
		case "Soda":
			// Add Found Soda to FoodPoints
			food += pointPerSoda;
			// Display foodText SodaPickedUp
			foodText.text = "Found " + pointPerSoda + " Soda! \n" + food + " Food & Drink left.";
			// Play DrinkSoda sFX
			SoundManager.instance.RandomizeSfx (drinkSound0, drinkSound1);
			// Disable Soda Item
			other.gameObject.SetActive (false);
			break;
		default:
			foodText.text = "Que esse cue ce'est?";
			break;
		}
	}


	// Function to LoseFood when Attacked
	public void LoseFood (int loss) {
		// Set Animator to Hit Animation
		anim.SetTrigger ("Hit");
		// Subtract Amount of Loss from Food
		food -= loss;
		// Display lost Food
		foodText.text = "You dropped " + loss + " Food! \n Only " + food + " Food & Drink left!"; 
		// Check if Player is GameOver
		CheckIfGameOver ();
	}


	// Function to Restart Game
	private void Restart () {
		// If player Reached Exit Level 
		SceneManager.LoadScene (0);
	}


	// Function to Check if Player is GameOver (Dead)
	private void CheckIfGameOver () {
		// If Player has No Food
		if (food <= 0) {
			// Play GameOver Sfx
			SoundManager.instance.PlaySingleAudioClip (gameOverSound);
			// Stop Background Music
			SoundManager.instance.musicSource.Stop();
			// Call GameOver function in GameManager
			GameManager.instance.GameOver ();
		}
	}
}
}