using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement.
/// Main function for:  Input; Movement; GroundCollision; CameraRayLength
/// </summary>

public class PlayerMovement : MonoBehaviour {
	//Movement speed of the Player (Serialized for tweaking)
	[SerializeField] private float speed = 6f;
	//Vector of the player movement (x,y,z)
	private Vector3 movement;
	//The animator component on the Player for Movement
	private Animator anim;
	//Rigidbody component for movement
	private Rigidbody playerRigidbody;
	//Floormask for GroundCollision
	private int floorMask;
	//CameraRayLength total lenght of Ray (to rotate the player according to mouseposition.)
	private float camRayLength = 100f;

	//Start at Game Start
	void Awake (){
		//Get the "Floor"-layer from Layermask function
		floorMask = LayerMask.GetMask ("Floor");
		//Get the Animator-component from the player
		anim = GetComponent <Animator> ();
		//Get the Rigidbody-component from the player
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	//FixedUpdate is called every Physics-Update
	void FixedUpdate () {
		//Get input from horizontal and vertical Axis . Raw -1,0 or 1.
		float inputHoriz = Input.GetAxisRaw ("Horizontal");
		float inputVerti = Input.GetAxisRaw ("Vertical");
		//call the MovementFunction
		Move(inputHoriz, inputVerti);
		//call the TurningFunction
		Turning();
		//Call the AnimationFunction
		Animating(inputHoriz, inputVerti);
	}

	//Movement function with Horizontal and Vertical input as arguments
	void Move (float horiz, float verti) {
		//set the movement vector3 of player according to Input
		movement.Set (horiz, 0f, verti);
		//Disable diagnal movement advantadge by Normalizing the Input values
		//Apply speed to the movement vector
		//And Multiply by Delta Time (time between each update-call) to set to normal update speed
		movement = movement.normalized * speed * Time.deltaTime;
		//Apply the movement to the position of the player
		playerRigidbody.MovePosition (transform.position + movement);
	}

	//Turning function with Mouse movement, Applies to Player and Camera
	void Turning (){
		//Get the Ray casted from the MainCamera into the scene
		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Get information back from raycast
		RaycastHit floorHit;
		//Preform the Raycast action(returns boolean), with the Ray to Mouseposition (cameraRay),
		//Toward the Hitpoint of the ray (floorHit), Set ray Lenght (camRayLength) & Only hit the FloorMask (floorMask)
		if ( Physics.Raycast (cameraRay, out floorHit, camRayLength, floorMask) ) {
			//If Raycast hit something:
			//Vector from player to mouse hit
			Vector3 playerToMouse = floorHit.point - transform.position;
			//Don't allow the Player to "Lean" forth and back
			playerToMouse.y = 0f;
			//Quaternion is used to store rotations. Cant set rotation base on Vector
			//Set the "Z"-Axis as the "Forward"-Axis (With the mouse movement)
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			//Apply the rotation to the Player to make him turn
			playerRigidbody.MoveRotation (newRotation);
		}
	}

	//Animation function
	void Animating (float horiz, float verti) {
		//Check if the player is walking
		bool walking = horiz != 0f || verti != 0;
		//Set the booling of walking to the Animator paramater IsWalking
		anim.SetBool("IsWalking", walking);
	}
}
