using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Required when using UI components

/// <summary>
/// Player health.
/// Function to keep track of Player Health & Damage & Death
/// </summary>

public class PlayerHealth : MonoBehaviour {

	//Starting Health at beginning of each game
	private int startingHealth = 100;
	//Current Health of the player
	public int currentHealth;
	//Health UI Slider to notify player of current health
	[SerializeField] private Slider healthSlider;
	//Damage UI Image to notify player of incoming damage
	[SerializeField] private Image damageImage;
	//Audio clip on Players Death
	[SerializeField] private AudioClip deathAudio;
	//Flash Speed of Damage UI Image
	[SerializeField] private float flashSpeed = 5f;
	//Flash Colour of Damage UI Image
	[SerializeField] private Color flashColour = new Color(1f, 0f, 0f, 0.5f);
	//Animator component of the player for Damage and Death
	private Animator anim;
	//AudioSource of the Player
	private AudioSource playerAudio;
	//get PlayerMovement script
	private PlayerMovement playerMovement;
	//get PlayerShooting script
//	PlayerShooting playerShooting;
	//Boolean if Player is Dead = True & Alive = False
	bool isDead;
	//Boolean if Player got Damage = True & No Damage = False
	bool isDamaged;

	//Start at beginning of game
	void Awake () {
		//get Animator-component from the Player
		anim = GetComponent <Animator> ();
		//get AudioSource-component from the Player
		playerAudio = GetComponent <AudioSource> ();
		//get PlayerMovement Script-component from the Player
		playerMovement = GetComponent <PlayerMovement> (); 
		//get PlayerShooting Script-component from the Player
	//	playerShooting = GetComponent <PlayerShooting> ();
		//Set the current health of the player to the startinghealth of the player.
		currentHealth = startingHealth;
	}

	// Use this for initialization
	//void Start () {
	//}
	
	// Update is called once per frame
	void Update () {
		//When Player is Damaged
		if (isDamaged) {
			//Flash the Damage Image (def Red-Screen)
			damageImage.color = flashColour;
		} else {
			//Remove the Damage Image to Alpha state with nice transition
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		//Set isDamaged to false
		isDamaged = false;
	}

	//Function for player taking damage
	public void TakeDamage (int amount){
		//isDamaged is true on damage
		isDamaged = true;
		//Subtrackt the amount of damage from the current health
		currentHealth -= amount;
		//Set healthSlider UI to the Current Health
		healthSlider.value = currentHealth;
		//Play the Player Hurt Audio
		playerAudio.Play ();
		//Check if the currentHealth and check if Player is dead
		if (currentHealth <= 0 && !isDead) {
			//When Player is Dead call the Death Function
			Death ();
		}
	}

	//Function for when the player is Dead
	void Death () {
		//Set the isDead boolean to true
		isDead = true;
		//Disable the Players Shooting Effects
	//	playerShooting.DisableEffects ();
		//Set the Death Animator trigger
		anim.SetTrigger ("Die");
		//Set the Audio for the Player to his Death-Audio
		playerAudio.clip = deathAudio;
		//Play the Players Death Audio
		playerAudio.Play ();
		//Disable the Players Movement
		playerMovement.enabled = false;
		//Disable the Player Ability to Shoot
	//	playerShooting.enabled = false;
	}
}
