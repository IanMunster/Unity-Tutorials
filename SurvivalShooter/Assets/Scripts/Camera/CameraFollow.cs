using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera follow.
/// 3rd Person Camera Follow, No Rotation
/// </summary>

public class CameraFollow : MonoBehaviour {

	//Target for Camera to follow (Serialized, to easely select Player)
	[SerializeField] private Transform target;
	//Camera movement smoothing (Serialized for tweaking)
	[SerializeField] private float smoothing = 5f;
	//Offset Camera from the player
	private Vector3 offset;

	//Use this for initialization
	void Start () {
		//the offset is the vector from the camera to the player
		offset = transform.position - target.position;
	}

	//FixedUpdate is called every Physics-Update
	void FixedUpdate (){
		//target position for the camera to reach
		Vector3 targetCamPos = target.position + offset;
		//Move the position of the Camera (Lerp smooths the transition)
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
