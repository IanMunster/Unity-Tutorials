using System.Collections;
using UnityEngine;

/// <summary>
/// Enemy.
/// Used to Move the Enemy and Do Damage to Player
/// </summary>

public class Enemy : MovingObject {

	// Number of FoodPoint subtracted from Player
	public int playerDamage;
	public int wallDamage;

	// Enemy Attack Sounds
	public AudioClip enemyAttackPlayer;
	public AudioClip enemyAttackWall;

	// Animator Component on Enemy
	private Animator anim;
	// Used to store PlayerPosition and Move towards
	private Transform target;
	// Bool to let Enemy Skip every other turn
	private bool skipMove = true;


// Use this for initialization
	protected override void Start () {
		// Add Enemy to List of Enemies in GameManager
		GameManager.instance.AddEnemyToList (this);
		// Get Animator Component
		anim = GetComponent <Animator> ();
		//Find the Player as Transform
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		// Call BaseClass Start
		base.Start ();
	}


// Function to Attempt to Move (override MovingObject)
	protected override void AttemptMove <T> (int xDir, int yDir) {
		// Should Enemy Skip a Turn
		if (skipMove) {
			// Dont Skip next Turn
			skipMove = false;
			// Do Nothing
			return;
		}
		// Call Base AttemptMove <Expect Player>
		base.AttemptMove <T> (xDir, yDir);
		// Since moved, skip next Turn
		skipMove = true;
	}


// Function to Move the Enemy (towards Player)
	public void MoveEnemy () {
		// Horizontal and Vertical Direction of Enemy
		int xDir = 0;
		int yDir = 0;
		// Check Position of Target and CurrentPosition, and Find Direction to Move 
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
			//if X is in same Column, Check if Y is bigger that Current (Yes: up, No: down)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		// Otherwise if X is not in same Column
		} else {
			// Check if X is bigger that Current (Yes : Right, No: Left)
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}
		// Attempt to Move and Expect Player
		AttemptMove <Player> (xDir, yDir);
	}


// Function when Enemy Cant Move (override MovingObject)
	protected override void OnCantMove <T> (T component) {
		// If Player Block path, Cast target as Player
		Player hitPlayer = component as Player;
		// Set Animate Enemy Attack
		anim.SetTrigger ("Attack");
		// Play the Enemy Attack Sfx
		SoundManager.instance.PlaySingleAudioClip (enemyAttackPlayer);
		// Damage the Player (Subtract playerDamage from FoodPoints)
		hitPlayer.LoseFood (playerDamage);
	}
}