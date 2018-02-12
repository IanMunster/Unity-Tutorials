using System; //No MonoBehaviour
using UnityEngine;

/// <summary>
/// Tank director.
/// Main Function to Manage Tank Settings
/// Works with GameDirector to Control TankBehaviour
/// Disables and Enables Tank Controls
/// </summary>

[Serializable] //When there is an instance, make it Serializalbe (Show up in Inspector)
public class TankDirector {

	//Color of Player/Tank
	public Color m_PlayerColor;
	//SpawnPoint of Player/Tank
	public Transform m_SpawnPoint;
	//Total Number of Players
	[HideInInspector] public int m_PlayerNumber;
	//Player UI Text (PlayerNumber Colored)
	[HideInInspector] public string m_ColoredPlayerText;
	//Reference of Tank Instance (when created)
	[HideInInspector] public GameObject m_Instance;
	//Total wins of Player
	[HideInInspector] public int m_Wins;

	//Reference to this Players MovementScript
	private TankMovement m_Movement;
	//Reference to this Platers ShootingScript
	private TankShooting m_Shooting;
	//Used to Disable Health&Aim UI
	private GameObject m_CanvasGameObject;

	//Function to Call for Tank Setup
	public void Setup () {
		//Get reference to the ScriptComponents
		m_Movement = m_Instance.GetComponent <TankMovement> ();
		m_Shooting = m_Instance.GetComponent <TankShooting> ();
		//Get the GameCanvas GameObject
		m_CanvasGameObject = m_Instance.GetComponentInChildren <Canvas> ().gameObject;
		//Set the PlayerNumbers to be Consistent across Scripts
		m_Movement.m_PlayerNumber = m_PlayerNumber;
		m_Shooting.m_PlayerNumber = m_PlayerNumber;
		//Create correct PlayerText (PlayerName + PlayerNumber in PlayerColor)
		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">Player " + m_PlayerNumber +"</color>";
		//Get all MeshRenders of Tank (in Children GameObjects)
		MeshRenderer[] renderers = m_Instance.GetComponentsInChildren <MeshRenderer> ();
		//Go through all MeshRenders
		for (int i = 0; i < renderers.Length; i++) {
			//Set Color of Tank to Color of Player
			renderers[i].material.color = m_PlayerColor;
		}
	}


	//Function to Enable/Disable Tank Controls
	public void EnableControls (bool isActive) {
		//Enable/Disable Movement&Shooting Script
		m_Movement.enabled = isActive;
		m_Shooting.enabled = isActive;
		//Enable/Disable Health&Aim UI
		m_CanvasGameObject.SetActive (isActive);
	}

	//Function to Reset the Tank to Default state
	public void Reset () {
		//Reset back to SpawnPoint Position and Rotation
		m_Instance.transform.position = m_SpawnPoint.position;
		m_Instance.transform.rotation = m_SpawnPoint.rotation;
		//Disable Tank
		m_Instance.SetActive (false);
		//Re-Enable Tank
		m_Instance.SetActive (true);
	}

}
