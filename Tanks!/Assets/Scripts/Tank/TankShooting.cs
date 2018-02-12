using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Required when using UI Components

/// <summary>
/// Tank shooting.
/// 
/// </summary>

public class TankShooting : MonoBehaviour {

	//number of player to indentify Tank (Set by this Tanks Director)
	public int m_PlayerNumber;
	//Prefab of the Shell (bullet) Rigidbody Component
	[SerializeField] private Rigidbody m_Shell;
	//End of the Barrel (Place where Shell should start)
	[SerializeField] private Transform m_FireTransform;
	//AimSlider UI component
	[SerializeField] private Slider m_AimSlider;
	//Shooting Audio Source
	[SerializeField] private AudioSource m_ShootingAudio;
	//Shot Charge Audio Clip
	[SerializeField] private AudioClip m_ChargingClip;
	//Shot Fire Audio Clip
	[SerializeField] private AudioClip m_FireClip;
	//Minimal Fire Launch Force
	[SerializeField] private float m_MinLaunchForce = 15f;
	//Maximal Fire Launch Force
	[SerializeField] private float m_MaxLaunchForce = 30f;
	//Maximal Fire Charge Time
	[SerializeField] private float m_MaxChargeTime = 0.75f;

	//Name of the Players FireButton
	private string m_FireButton;
	//Force to be given to Shell on Fire
	private float m_CurrentLaunchForce;
	//How fast LaunchForce Increases, based on Max Charge Time
	private float m_ChargeSpeed;
	//Is the Shell Fired?
	private bool m_isFired;


	//Starts when ParentObject is Enabled
	private void OnEnable () {
		//Reset Aim UI
		//Set the Current Force to Minimal Force
		m_CurrentLaunchForce = m_MinLaunchForce;
		//Set the AimSlider value to Minimal Force
		m_AimSlider.value = m_MinLaunchForce;
	}


	// Use this for initialization
	private void Start () {
		//Assign the Player FireButton with Player Number
		m_FireButton = "Fire" + m_PlayerNumber;
		//Define the ChargeSpeed (Range of Possible Forces divided by Charge time)
		m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
	}


	// Update is called once per frame
	private void Update () {
		//Set slider to Minimal Force
		m_AimSlider.value = m_MinLaunchForce;
		//If the CurrentForce exceeds or is equal to the MaxForce And the Shell has not been Fired
		if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_isFired) {
			//Charged to max; Launch Shell with Max Force
			m_CurrentLaunchForce = m_MaxLaunchForce;
			Fire ();

		//Otherwise Is Button Pressed Fire for First Time
		} else if (Input.GetButtonDown(m_FireButton) ) {
			//Not yet Fired, set LaunchForce to Minimum
			m_isFired = false;
			m_CurrentLaunchForce = m_MinLaunchForce;

			//Start Playing FireCharging Audio
			m_ShootingAudio.clip = m_ChargingClip;
			m_ShootingAudio.Play ();

		//Otherwise Is the Button Currently Held & Not Fired
		} else if (Input.GetButton(m_FireButton) && !m_isFired ) {
			//Update value of Charge
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
			//Set Aimslider to Current Force
			m_AimSlider.value = m_CurrentLaunchForce;

		//Otherwise Is the Button Released & Not Fired
		} else if (Input.GetButtonUp(m_FireButton) && !m_isFired ) {
			//Fire Shell
			Fire();
		}
	}

	//Function to Fire a Shell
	private void Fire () {
		//Set is Fired to true
		m_isFired = true;
		//Create a Shell (Instantiate: Shell, at the BarrelEnd Position, With BarrelEnd Rotation)
		Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
		//Set Shells velocity in correct direction
		shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
		//Play ShootAudio
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();
		//Reset the LaunchForce after shooting (in case of missed buttonpress)
		m_CurrentLaunchForce = m_MinLaunchForce;
	}
}
