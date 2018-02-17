using System.Collections;
using UnityEngine;

/// <summary>
/// Loader.
/// 
/// </summary>

public class Loader : MonoBehaviour {

	// Reference to GameManager
	public GameObject gameManager;

	// Use this for initialization
	private void Awake () {
		// If no GameManager found
		if (GameManager.instance == null) {
			// Create a new GameManager
			Instantiate (gameManager);
		}
	}
}
