using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shell explosion.
/// Main Function for the Shell (Bullet) to Give Damage and Explode
/// </summary>

public class ShellExplosion : MonoBehaviour {

	//Layer of All Players
	[SerializeField] private LayerMask m_TankMask;
	//Explosion ParticlesSystem
	[SerializeField] private ParticleSystem m_ExplosionParticles;
	//Explosion Audio
	[SerializeField] private AudioSource m_ExplosionAudio;
	//Total Maximal Amount of Damage
	[SerializeField] float m_MaxDamage = 100f;
	//Explosion Force value
	[SerializeField] float m_ExplosionForce = 1000f;
	//Explosion Max LifeTime value
	[SerializeField] float m_ExplosionLifeTime = 2f;
	//Explosion Max Radius
	[SerializeField] float m_ExplosionRadius = 5f;

	// Use this for initialization
	private void Start () {
		//Destroy the GameObject after LifeTime
		Destroy (gameObject, m_ExplosionLifeTime);
	}

	//Starts when another Collider Enters the Trigger
	private void OnTriggerEnter (Collider other) {
		//Find all Players in ExplosionArea and get their ColliderComponent
		Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);
		//Go through the Colliders List
		for (int i = 0; i < colliders.Length; i++) {
			//Check if Target has a Rigidbody
			Rigidbody targetRigidbody = colliders[i].GetComponent <Rigidbody> ();
			//Check if Target has a Rigidbody
			if (!targetRigidbody) {
				//If no Rigidbody found, continue to next in list
				continue;
			}
			//Add force to found Rigidbody
			targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);
			//Targets TankHealth Script-Component
			TankHealth targetHealth = targetRigidbody.GetComponent <TankHealth> ();
			//Check if it has TankHealth
			if (!targetHealth) {
				//No TankHealth found, continue to next in list
				continue;
			}
			//If Target has TankHealth Calculate accumilated Damage
			float damage = CalculateDamage (targetRigidbody.position);
			//apply the Damage
			targetHealth.TakeDamage (damage);
		}
		//Remove ExplosionParticles from Parent (Un-Parent to Remove Tank)
		m_ExplosionParticles.transform.parent = null;
		//Play the Explosion Particles
		m_ExplosionParticles.Play ();
		//Play the Explosion Audio
		m_ExplosionAudio.Play();
		//Destroy the ExplosionParticles and Objects after Finished
		Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
		//Destroy the Shell (Bullet)
		Destroy (gameObject);
	}

	//Function to Calculate the accumilated Damage from Inpackt Position and Player Position
	private float CalculateDamage (Vector3 targetPosition) {
		//Set a vector for Distance from Target and Shell
		Vector3 explosionToTarget = targetPosition - transform.position;
		//Set the Distance form Target to Shell
		float explosionDistance = explosionToTarget.magnitude;
		//Relative distance between Target to Shell (returns 1 - 0)
		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
		//Amount of Damage
		float damage = relativeDistance * m_MaxDamage;
		//Make Sure Damage is not Negative
		damage = Mathf.Max (0f, damage);
		//Return the Calculated Damage
		return damage;
	}

}
