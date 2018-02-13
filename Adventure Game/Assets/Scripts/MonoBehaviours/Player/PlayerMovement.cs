﻿using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;				//Required when using AI-Components (NavMeshAgent)
using UnityEngine.EventSystems;		//Required when using EventSystems

/// <summary>
/// Player movement.
/// Function to Enable/Disable NavMeshAgent form Moving Player
/// Function to Smooth Animation on Entering Interactions (Disable Players Rotation & Snap to DesiredPosition
/// </summary>

public class PlayerMovement : MonoBehaviour {

	//Animator-Component on Player
	[SerializeField] private Animator animator;
	//NavMeshAgent-Component on Player
	[SerializeField] private NavMeshAgent agent;
	//Delay Input for Animations
	[SerializeField] private float inputHoldDelay = 0.5f;
	//Treshold before PlayerMoves Fast Enough
	[SerializeField] private float turnSpeedThreshold = 0.5f;
	//Animator Speed Damp value (Amount of time Speed will change to new Value)
	[SerializeField] private float speedDampTime = 0.1f;
	//Speed to Slow the Players Speed
	[SerializeField] private float slowingSpeed = 0.175f;
	//Smooth Turn Enalbe to Desired Rotation
	[SerializeField] private float turnSmoothing = 15f;

	//Delay Input in WaitForSeconds
	private WaitForSeconds inputHoldWait;
	//Determent Destination
	private Vector3 destinationPosition;
	//The current Interactable
	//private Interactable currentInteractable;


	//Constant value to Proportion Stopping Distance (1/10 of StoppingDistance)
	private const float stopDistanceProportion = 0.1f;
	//Distance the SamplePosition can Hit
	private const float navMeshSampleDistance = 4f;

	//Needed to check appropriate speed for Animator
	private readonly int hashSpeedPara = Animator.StringToHash("Speed");


	//Use this for initialisation
	private void Start () {
		//NavMesh not Rotate Player
		agent.updateRotation = false;
		//Wait to interaction to finish, set new WaitForSeconds for InputDelay
		inputHoldWait = new WaitForSeconds (inputHoldDelay);
		//Set first destination to Start position
		destinationPosition = transform.position;
	}


	//Make Player Move (For Interactions) setSpeed of NavMeshAgent based on Animator
	private void OnAnimatorMove (){
		//Set agents velocity to Animators position (Speed = Distance / Time)
		agent.velocity = animator.deltaPosition / Time.deltaTime;
	}

	//Updates every Frame
	private void Update (){
		//Function Calls: Stopping, Slowing and Moving
		//Wait for PathFinding (pathPending mean function busy to Find the Pathing)
		if (agent.pathPending) {
			return;
		}

		//Check how fast Player WANTS to move
		float speed = agent.desiredVelocity.magnitude;
		//Check if Destination is within proportion of StoppingDistance
		if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion) {
			//Stop Player 
			Stopping (out speed);
		//Otherwise if Player is within StoppingDistance
		} else if (agent.remainingDistance <= agent.stoppingDistance) {
			//Slow Player (with speed based on RemainingDistance)
			Slowing (out speed, agent.remainingDistance);
		//Otherwise Check if Moving Fast Enough
		} else if (speed > turnSpeedThreshold) {
			Moving ();
		}

		//Give Animator appropriate Speed
		animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
	}

	//Function to Stop Player based on Interactions (and Animations) "out" to Effect the Speed within Function
	private void Stopping (out float speed) {
		//prevent NavMeshAgent from Moving Player
		agent.isStopped = true;
		//snap Player to Destination Position
		transform.position = destinationPosition;
		//Stop Player
		speed = 0f;

	}


	//Function to Slow Player based on Interactions (and Animations) "out" to Effect Speed within Function
	//Slow based on Distance of Destination
	private void Slowing (out float speed, float distanceToDestination) {
		//prevent NavMeshAgent from Moving Player
		agent.isStopped = true;
		//Define Players new Position (gradually move player)
		transform.position = Vector3.MoveTowards (transform.position, destinationPosition, slowingSpeed * Time.deltaTime);
		//Distance from Destination and StoppingDistance (Proportional)
		float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
		//Calculate Players Speed (Interpolate)
		speed = Mathf.Lerp (slowingSpeed, 0f, proportionalDistance);
	}


	//Function to (Allow the Player to) Move (General Case)
	private void Moving () {
		//Enable Player to Rotate

		//Rotation Player wants to move
		Quaternion targetRotation = Quaternion.LookRotation (agent.desiredVelocity);
		//Set Desired Rotation
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
	}

	//Gets Called when GROUND is Clicked On (To move the Player)
	//BaseEventData gives Data about Current Event (when event happens) as PointerEvent
	public void OnGroundClick (BaseEventData data) {
		//Make PointerEventData by Casting data as PointerEventData
		PointerEventData pData = (PointerEventData)data;
		//Find NavMesh closed to Click
		NavMeshHit hit;
		//Sample the Position (Point where RayCast Hit in the World, Return to Hit, Give Distance, Set the Areas) 
		if (NavMesh.SamplePosition (pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas)) {
			//Set the Destination to NavMesh Hit Position
			destinationPosition = hit.position;
		//If RayCast didn't Hit, find location nearby
		} else {
			//Find where Pointer Clicked
			destinationPosition = pData.pointerCurrentRaycast.worldPosition;
		}

		//Set the NavMeshAgent its Destination
		agent.SetDestination (destinationPosition);
		//Let NavMeshAgent resume with Moving Player
		agent.isStopped = false;
		//Check Interactable heading towards
	}

}
