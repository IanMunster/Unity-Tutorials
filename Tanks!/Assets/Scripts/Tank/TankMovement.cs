using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tank movement.
/// Main function to Move & Turn the Tank, Get Input and Movement Audio
/// </summary>

public class TankMovement : MonoBehaviour {

	//number of player to indentify Tank (Set by this Tanks Director)
	public int m_PlayerNumber;
	//maximum acceleration&deceleration speed
	[SerializeField] private float m_Speed = 12f;
	//maximum turning speed over time
	[SerializeField] private float m_TurnSpeed = 180f;
	//Audio Source of TankEngine
	[SerializeField] private AudioSource m_MovementAudio;
	//AudioClip of TankEngineIdle
	[SerializeField] private AudioClip m_EngineIdle;
	//AudioClip of TankEngineRunning (aka driving)
	[SerializeField] private AudioClip m_EngineRunning;
	//Pitch of the AudioClip
	[SerializeField] private float m_PitchRange = 0.2f;

	//Input for accel&decel
	private string m_MovementAxisName;
	//Input for turning
	private string m_TurnAxisName;
	//Rigidbody Component used to move the Tank
	private Rigidbody m_Rigidbody;
	//Current accel&decel value
	private float m_MovementInputValue;
	//Current turning value
	private float m_TurnInputValue;
	//Audio origional Pitch (Audio pitch at start of Game)
	private float m_OriginalPitch;


	//Starts at start of Game
	private void Awake (){
		//Get the reference to the Rigidbody-Component
		m_Rigidbody = GetComponent <Rigidbody> ();
	}


	//Starts when parentObject is Enabled
	private void OnEnable () {
		//When Tank enabled; Turn off Kinematic Rigidbody
		m_Rigidbody.isKinematic = false;
		//Reset current movement values
		m_MovementInputValue = 0f;
		m_TurnInputValue = 0f;
	}


	//Starts when parentObject is Disabled
	private void OnDisable () {
		//When Tank disabled; Turn on Kinematic Rigidbody (Disables Movement)
		m_Rigidbody.isKinematic = true;
	}


	// Use this for initialization
	private void Start () {
		//Set correct Player Input to Tank Input
		m_MovementAxisName = "Vertical" + m_PlayerNumber;
		m_TurnAxisName = "Horizontal" + m_PlayerNumber;
		//Set origional Pitch of TankEngineAudio
		m_OriginalPitch = m_MovementAudio.pitch;
	}


	// Update is called once per frame
	private void Update () {
		//Get Movement Input
		m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
		//Play Engine Audio
		EngineAudio ();
	}


	//Function for Tank Engine Audio
	private void EngineAudio () {
		//When there is No Input absolute values (Idle Tank)
		if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f) {
			//And the TankEngineAudio is playing TankEngineRunning
			if (m_MovementAudio.clip == m_EngineRunning) {
				//Change Audio to TankEngineIdle
				m_MovementAudio.clip = m_EngineIdle;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		//Otherwise; If the is Input (Moving Tank)
		} else {
			//And the TankEngineAudio is playing TankEngineIdle
			if (m_MovementAudio.clip == m_EngineIdle) {
				//Change Audio to TankEgineRunning
				m_MovementAudio.clip = m_EngineRunning;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		}
	}


	//FixedUpdate is called every Physics-step
	private void FixedUpdate () {
		//Update Movement (accel&decel and turning)
		Move ();
		Turn ();
	}


	//Function for Moving the Tank
	private void Move () {
		//Set vector to "Front" of Tank, and Accel the tank with the Input*MaxSpeed per second (time-between-frames)
		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
		//Move the Tank
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
		
	}


	//Function for Turning the Tank
	private void Turn (){
		//Set number to Turn the Tank, based on Input*Speed per second (time-between-frames)
		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
		//Set Turn number to Rotation on the Y-Axis
		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
		//Turn the Tank
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
	}
}
