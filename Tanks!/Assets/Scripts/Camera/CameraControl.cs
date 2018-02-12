using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera control.
/// Main Function for the Camera to Zoom and Follow the Tanks
/// </summary>

public class CameraControl : MonoBehaviour {

	//time before Camera refocuses
	[SerializeField] private float m_DampTime = 0.2f;
	//BufferSpace between Top/Bottom Target and ScreenSize
	[SerializeField] private float m_ScreenEdgeBuffer = 4f;
	//Minimal Zoom Size
	[SerializeField] private float m_MinSize = 6.5f;
	//Targets of the Camera
	[HideInInspector] public Transform[] m_Targets;

	//Reference to MainCamera
	private Camera m_Camera;
	//Smooth Zoom Speed
	private float m_ZoomSpeed;
	//Smooth Velocity of Positioning
	private Vector3 m_MoveVelocity;
	//Position Camera is Moving Towards
	private Vector3 m_DesiredPosition;


	//Starts at Game Start
	private void Awake () {
		//Get the MainCamera Reference
		m_Camera = GetComponentInChildren <Camera> ();
	}


	//FixedUpdate is called every Physics-step
	private void FixedUpdate () {
		//Move the Camera
		Move();
		//Zoom the Camera
		Zoom();
	}


	//Function to move the Camera
	private void Move () {
		//Find the Avarage positions of All Players
		FindAveragePosition ();
		//Smoothly move the Camera
			//From the origional position, to desired position, take reference to MoveVelocity and Damp by maxDist. 
		transform.position = Vector3.SmoothDamp (transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); 
	}


	//Function to find AveragePosition between All Players
	private void FindAveragePosition () {
		//Current Average position Between Players
		Vector3 averagePosition = new Vector3 ();
		//Current Number of Players
		int numberTargets = 0;

		//Find all Players and Add their Position
		for (int i = 0; i < m_Targets.Length; i++) {
			//If found target is not Active, go to next
			if (!m_Targets[i].gameObject.activeSelf) {
				continue;
			}
			//otherwise: Add the Player and add Position to Average
			averagePosition += m_Targets[i].position;
			numberTargets++;
		}

		//If there are Players, defide their Position between Number of Players (average)
		if (numberTargets > 0) {
			averagePosition /= numberTargets;
		}

		//Keep the Y-Axis value (Keep camera "High")
		averagePosition.y = transform.position.y;
		//Set the DesiredPosition to the AveragePosition (between Players)
		m_DesiredPosition = averagePosition;
	}


	//Function to Zoom the Camera
	private void Zoom () {
		//Set the Required Zoom Size
		float requiredSize = FindRequiredSize ();
		//Smoothly Zoom the Camera
		//From the origional position, to desired position, take reference to MoveVelocity and Damp by maxDist. 
		m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}


	//Function to Find the required Zoom size between All Players
	private float FindRequiredSize () {
		//Find the CameraRigs Current DesiredPosition in it LocalSpace
		Vector3 desiredLocalPosition = transform.InverseTransformPoint (m_DesiredPosition);
		//Start Camera Size Calculation at Zero
		float size = 0f;

		//Go through All Players
		for (int i = 0; i < m_Targets.Length; i++) {
			//if inactive fo to next Player
			if (!m_Targets[i].gameObject.activeSelf) {
				continue;
			}
			//Otherwise: Find the Position of Target in CameraRigs LocalSpace
			Vector3 targetLocalPosition = transform.InverseTransformPoint (m_Targets[i].position);
			//Find the Distance of Player - DesiredPosition of CameraRigs LocalSpace
			Vector3 desiredPosToTarget = targetLocalPosition - desiredLocalPosition;
			//Choose Largest of Current Size and Distance of Tank from Up-Down (Y-Axis) CameraScreenEdge
			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y) );
			//Choose Largers of Current Size and Distance of Tank from Left-Right (X-Axis) CameraScreenEdge
			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
		}

		//Add the EdgeBuffer to the Size
		size += m_ScreenEdgeBuffer;
		//Make sure Camera is not below Minimum Size
		size = Mathf.Max (size, m_MinSize); 
		//Return the new Size Value
		return size;
	}


	//Function to Set the Camera's Start Position (Game Start Position)
	public void SetStartPositionAndSize () {
		//Find the DesiredPosition for Camera
		FindAveragePosition ();
		//Set the CameraRigs Position to DesiredPosition without Damping
		transform.position = m_DesiredPosition;
		//Find and Set the Required Size of the Camera
		m_Camera.orthographicSize = FindRequiredSize ();
	}
}
