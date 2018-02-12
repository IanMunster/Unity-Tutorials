using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Required when using UI-Components

/// <summary>
/// Tank health.
/// Main function to keep track of PlayerHealth, Health UI and Death Animations
/// </summary>

public class TankHealth : MonoBehaviour {

	//Start health of the Player
	[SerializeField] private float m_StartingHealth = 100f;
	//HealthUI Slider componenet
	[SerializeField] private Slider m_Slider;
	//HealthUI FillImage component
	[SerializeField] private Image m_FillImage;
	//HealthUI FullHealth Color
	[SerializeField] private Color m_FullHealthColor = Color.green;
	//HealthUI ZeroHealth Color
	[SerializeField] private Color m_ZeroHealthColor = Color.red;
	//Explosion on Dead Prefab reference
	[SerializeField] private GameObject m_ExplosionPrefab;

	//Explosion AudioSource
	private AudioSource m_ExplosionAudio;
	//Explosion ParticleSystem
	private ParticleSystem m_ExplosionParticles;
	//Player Current Health
	private float m_CurrentHealth;
	//is the Player Dead?
	private bool m_isDead;


	//Starts at Game Start
	private void Awake () {
		//Instantiate the ExplosionPrefab and get ParticlesSystem from ExplosionPrefab
		m_ExplosionParticles = Instantiate (m_ExplosionPrefab).gameObject.GetComponent <ParticleSystem> (); 
		//Get AudioSource from ExplosionPrefab
		m_ExplosionAudio = m_ExplosionParticles.GetComponent <AudioSource> ();
		//Set the ExplosionPrefab to Inactive
		m_ExplosionPrefab.gameObject.SetActive (false);
	}

	//Starts when ParentObject is Enabled
	private void OnEnable () {
		//Set the Player to Alive and 100% Health
		m_CurrentHealth = m_StartingHealth;
		m_isDead = false;
		//Call HealthUI Function
		SetHealthUI();
	}

	//Function for Taking Damage (amount of Damage)
	public void TakeDamage (float amount) {
		//Subtrackt the Amount of Damage from the Players Current Health
		m_CurrentHealth -= amount;
		//Change HealthUI appropriately
		SetHealthUI ();
		//If the Current Health of the Player is 0 or Below 0, Player is Dead
		if (m_CurrentHealth <= 0f && !m_isDead) {
			//Call the onDeath Function
			OnDeath();
		}

	}

	//Function for Setting UI CurrentHealth
	private void SetHealthUI () {
		//Set the HealthSlider to CurrentHealth value
		m_Slider.value = m_CurrentHealth;
		//Linear-Interpolate the HealthUI Colour between two chosen colors, based on CurrentHealth percentage
		m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
	}

	//Function for Death
	private void OnDeath () {
		//Set the isDead to true
		m_isDead = true;
		//Move the ExplosionParticles to DeadPlayers Position, and Activate
		m_ExplosionParticles.transform.position = transform.position;
		m_ExplosionParticles.gameObject.SetActive (true);
		//Play the Explosion ParticleSystem
		m_ExplosionParticles.Play ();
		//Play the Explosion Sound
		m_ExplosionAudio.Play ();
		//Remove the Player
		gameObject.SetActive (false);
	}

}
