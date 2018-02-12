using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// User interface direction control.
/// Main function to Make UI Element Dont Rotate (Stay static)
/// </summary>

public class UIDirectionControl : MonoBehaviour {

	//Should Relative Rotation be Used on this GameObject
	[SerializeField] private bool m_UsesRelativeRotation = true;
	//Local RotationValue at start of Game
	private Quaternion m_RelativeRotation;

	// Use this for initialization
	private void Start () {
		//Get the local RotationValue
		m_RelativeRotation = transform.parent.localRotation;
	}
	
	// Update is called once per frame
	private void Update () {
		//if this object should Rotate
		if(m_UsesRelativeRotation){
			//Rotate the object accordingly
			transform.rotation = m_RelativeRotation;
		}
	}
}
