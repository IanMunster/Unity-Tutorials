using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

	//damage per Pistol Shot
	[SerializeField] private int damagePerShot = 20;
	//time between shots (reload)
	[SerializeField] private float timeBetweenBullets = 0.15f;
	//range of pistol
	[SerializeField] private float range = 100f;

	//Set timer for Reloadtime
	private float timer;
	//get a reference to the RayCaster from gun
	private Ray shootRay;
	//reference to the RaycastHit object
	private RaycastHit shootHit;
	//number of the layer "Shootable"
	private int shootalbeMask;
	//get the smokeParticleSystems from gun
	private ParticleSystem gunParticles;
	//get the LineRenderer from gun
	private LineRenderer gunLine;
	//get the AudioSource from gun
	private AudioSource gunAudio;
	//get the Lighting from gun
	private Light gunLight;
	//total amount of time of Effect on screen
	[SerializeField] private float effectsDisplayTime = 0.5f;

	//Starts at Game Start
	void Awake () {
		//Set the shootable-layer from LayerMask (returns Int)
		shootalbeMask = LayerMask.GetMask ("Shootable");
		//Set the references
		gunParticles = GetComponent <ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent <AudioSource> ();
		gunLight = GetComponent <Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		//set timer to current time (...game is played)
		timer += Time.deltaTime;
		//If the FireButton is pressed & the reload time is over
		if (Input.GetButton("Fire1") && timer >= timeBetweenBullets ) {
			Shoot ();
		}

		//After displaying the Effects, check timer
		if(timer >= timeBetweenBullets * effectsDisplayTime){
			//disable the Effects
			DisableEffects ();
		}
	}

	//Function to Disable Gun Effects (Line & Light)
	public void DisableEffects () {
		//Disable the LineRenderer
		gunLine.enabled = false;
		//Disable the Light
		gunLight.enabled = false;
	}

	//Function for player Shooting Logic & Effects
	void Shoot () {
		//Reset the timer
		timer = 0f;
		//Play the Shoot Audio
		gunAudio.Play();
		//Enable the "Muzzle-Flash" Light
		gunLight.enabled = true;
		//Stop previously running "Smoke" Particles
		gunParticles.Stop ();
		//Start new "Smoke" Particles
		gunParticles.Play ();
		//Enable the Gun LineRenderer
		gunLine.enabled = true;
		//Set the Gun LineRenderer to MuzzleTip position
		gunLine.SetPosition (0, transform.position);
		//Set the Shootray at MuzzleTip
		shootRay.origin = transform.position;
		//Set the Direction of the Shootray forward from Barrel
		shootRay.direction = transform.forward;
		//Check the RayCasting against Objects on the "Shootable"-Layer
		if (Physics.Raycast (shootRay, out shootHit, range, shootalbeMask)) {
			//Check the object for Health-Script
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
			//If it has a Health-Script
			if (enemyHealth != null) {
				//give the enemy damage
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
			//otherwise (if no Health) set the second point of the Line to where the Ray Hit.
			gunLine.SetPosition (1, shootHit.point);
		//If the raycast didn't hit the Shootable layer
		} else {
			//set the Second position of the Line to the Max Range distance
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}

	}
}
