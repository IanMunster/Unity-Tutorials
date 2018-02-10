using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Reguired when using AI components

/// <summary>
/// Enemy health.
/// Main function for the Enemy's Health, Death and player EXP score (on Enemy's Death)
/// </summary>

public class EnemyHealth : MonoBehaviour {
	//Set a reference for the CurrentHealth of the Enemy
	public int currentHealth;
	//Set the Starting Health of the Enemy (Serialized for tweaking)
	[SerializeField] private int startingHealth = 100;
	//Set sink speed (on Death sink through level) (Serialized for tweaking)
	[SerializeField] private float sinkSpeed = 2.5f;
	//Set amount of EXP
	[SerializeField] private int scoreValue = 10;
	//Set the audio of the Enemy on Death
	private AudioClip deathClip;
	//Get the AudioSource component from enemy
	private AudioSource enemyAudio;
	//Get the AnimatorComponent from enemy
	private Animator anim;
	//Get the Rigidbody Component from enemy
	private Rigidbody rigid;
	//Get the NavMeshAgent from enemy
	private NavMeshAgent navigation;
	//Get the BloodParticles from enemy
	private ParticleSystem hitParticles;
	//Get the Collider from enemy
	private CapsuleCollider capsuleCollider;
	//Set a bool for If the Enemy is dead (True) and Alive (False)
	private bool isDead;
	//Set a bool for If the Enemy is Sinking (True)
	private bool isSinking;


	// Starts at beginning of game
	void Awake () {
		//Get the AudioSource Component
		enemyAudio = GetComponent <AudioSource> ();
		//Get the Animator Component
		anim = GetComponent <Animator> ();
		//get the Rigidbody Component
		rigid = GetComponent <Rigidbody> ();
		//Get the NavMeshAgent Component
		navigation = GetComponent <NavMeshAgent> ();
		//Get the ParticleSystem Component from the Child-gameobject
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		//Get the CapsuleCollider
		capsuleCollider = GetComponent <CapsuleCollider> ();
		//set the currentHealth to the startingHealth
		currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		//If the enemy is dead & should be removed (Sink through level)
		if(isSinking){
			//Move the enemy down through the level * the sinkSpeed * PerSecond (instead per frame)
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}

	//Function when the Enemy takes Damage, (The amount of DMG and The location)
	public void TakeDamage (int amount, Vector3 hitPoint) {
		//if the enemy is dead
		if(isDead){
			//no longer need to take damage
			return;
		}
		//Otherwise:
		//play the Enemy Hit Audio
		enemyAudio.Play ();
		//Subtrackt the amount of Damage from the enemy's current Health
		currentHealth -= amount;
		//Set the position for the ParticleSystem to location of hit
		hitParticles.transform.position = hitPoint;
		//Play the ParticleEffect
		hitParticles.Play ();
		//if the amount of health is 0 or less than 0
		if (currentHealth <= 0) {
			//Call the Death Function
			Death();
		}
	}

	//Function for when the enemy is Dead
	void Death () {
		//Set isDead to true
		isDead = true;
		//Disable the Collision of the Enemy
		capsuleCollider.isTrigger = true;
		//Play the EnemyDead animation
		anim.SetTrigger ("Die");
		//Change the AudioSource to the DeathAudio
		enemyAudio.clip = deathClip;
		//Play the DeathAudio
		enemyAudio.Play ();

		StartSinking ();
	}

	//Function for sinking the dead Enemy
	public void StartSinking () {
		//disable the NavMeshAgent
		navigation.enabled = false;
		//find the RigidBodyComponent and set the Kinematic true (Used to Translate the Enemy)
		rigid.isKinematic = true;
		//Set the enemy isSinking to true
		isSinking = true;
		Debug.Log ("Sinking");
		//Add the ScoreAmount to players EXPamount
		ScoreDirector.score += scoreValue;
		//Destroy the enemy
		Destroy (gameObject, 2f);
	}

}
