using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Required when using UI-Components
using UnityEngine.SceneManagement; //Required when using SceneManagement-Components

/// <summary>
/// Game director.
/// MainFunction for: 
/// Finding All Number of Players and start Creating them
/// Adding All Players to CameraTargets
/// Starting All the GameLoops (GameStarting,GamePlaying,GameEnding)
/// Checking for Round & Game Winners
/// Displaying Correct GameOver UI text
/// </summary>

public class GameDirector : MonoBehaviour {

	//Number of Rounds needed to Win to be GameWinner
	[SerializeField] private int m_NumRoundsToWin = 5;
	//Delay before Round Starting-Playing Phase
	[SerializeField] private float m_StartDelay = 3f;
	//Delay before Round Playing-Ending Phase
	[SerializeField] private float m_EndDelay = 3f;
	//Reference to CameraControl Script
	[SerializeField] private CameraControl m_CameraControl;
	//GameCanvas Text reference
	[SerializeField] private Text m_GameText;
	//Prefabs of Tanks (Players)
	[SerializeField] private GameObject m_TankPrefab;
	//List of TankManagers (Number of Players) for enable/disable tank
	[SerializeField] private TankDirector[] m_Tanks;

	//Current number of Game Rounds
	private int m_RoundNumber;
	//Delay before Round Start
	private WaitForSeconds m_StartWait;
	//Delay before Round/Game Ends
	private WaitForSeconds m_EndWait;
	//Reference to Winner of Current Round
	private TankDirector m_RoundWinner;
	//Reference to Winner of Current Game
	private TankDirector m_GameWinner;

	// Use this for initialization
	private void Start () {
		//Create the Delay Times
		m_StartWait = new WaitForSeconds (m_StartDelay);
		m_EndWait = new WaitForSeconds (m_EndDelay);
		//Call Spawn All Tanks Function
		SpawnAllTanks ();
		//Set the Camera Targets
		SetCameraTargets ();
		//Start the GameLoop Coroutine
		StartCoroutine (GameLoop() );
	}
	

	//Spawn all the Tanks at beginning of the Round
	private void SpawnAllTanks () {
		//For all the Player/Tanks in the Game
		for (int i = 0; i < m_Tanks.Length; i++) {
			//Create and Instance of the Tank, On the SpawnPoint as a GameObject
			m_Tanks[i].m_Instance = Instantiate (m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
			//Set the Correct Number of the Player for controls
			m_Tanks[i].m_PlayerNumber = i + 1;
			//And Call the Setup Function from the correct TankDirector
			m_Tanks[i].Setup ();
		}
	}


	//Setup the Camera to the Amount of Players
	private void SetCameraTargets () {
		//Create collection of Transform, size of Total Players
		Transform[] targets = new Transform[m_Tanks.Length];
		//For each of the Players
		for (int i = 0; i < targets.Length; i++) {
			//Set it to appropriate Tank Transform
			targets[i] = m_Tanks[i].m_Instance.transform;
		}

		//Set Camera's Targets
		m_CameraControl.m_Targets = targets;
	}


	//Called from the start and will run each phase of the game, one after another.
	//Function to Restart the Game untill there is a Winner
	private IEnumerator GameLoop () {
		//Start with RoundStart, dont return Until Finished
		yield return StartCoroutine (RoundStarting() );
		//Once Finished, start with RoundPlaying, dont return Until Finished
		yield return StartCoroutine (RoundPlaying() );
		//Once Finished, start with RoundEnding, dont return Until Finished
		yield return StartCoroutine (RoundEnding() );
		//After RoundEnding Finished, Check for GameWinner
		if (m_GameWinner != null) {
			//if there is a Winner, Restart the Game
			SceneManager.LoadScene (00);
		//If there is no winner
		} else {
			//Restart the GameLoop (Current GameLoop will end)
			StartCoroutine (GameLoop() );
		}
	}


	//Function to Reset to New Round
	private IEnumerator RoundStarting () {
		//Reset All the Tanks
		ResetAllTanks ();
		//Disable Tank Controls
		DisableTankControls ();
		//Set Camera to RoundStarting Position
		m_CameraControl.SetStartPositionAndSize ();
		//Increment round Number to Current Number
		m_RoundNumber++;
		//Display Round Number
		m_GameText.text = "ROUND " + m_RoundNumber; 
		//Wait before Game Start (Wait before Yielding control back to GameLoop)
		yield return m_StartWait;
	}


	//Function to Start the Round
	private IEnumerator RoundPlaying () {
		//Enable TankControls
		EnableTankControls ();
		//Clear Game text from screen
		m_GameText.text = string.Empty;
		//Aslong as not 1 Tank
		while (!OneTankLeft() ) {
			//Return on the next frame
			yield return null;
		}
	}


	//Function to End the Round
	private IEnumerator RoundEnding () {
		//Disable Tank Controls
		DisableTankControls();
		//Clear Previous RoundWinner
		m_RoundWinner = null;
		//Check for winners at Round End
		m_RoundWinner = GetRoundWinner();

		//If there is a Winner
		if (m_RoundWinner != null) {
			//Increment Winners Score
			m_RoundWinner.m_Wins++;
		}

		//Check for GameWinners
		m_GameWinner = GetGameWinner ();
		//Set GameText based on Score and Winners
		string message = EndMessage ();
		//Diplay GameText
		m_GameText.text = message;
		//Wait before Game Start (Wait before Yielding control back to GameLoop)
		yield return m_EndWait;
	}


	//Function to check is there are One Players Alive, (Round Winner)
	private bool OneTankLeft () {
		//Start count at 0
		int numTankleft = 0;

		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//If Tank is Active
			if (m_Tanks[i].m_Instance.activeSelf) {
				//Increment Counter
				numTankleft++;
			}
		}

		//if OneTank left return True, otherwise False
		return numTankleft <= 1;
	}


	//Function is called to find Winner of Round, Asuming there is 1 or less Tanks
	private TankDirector GetRoundWinner () {
		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//If Tank is Active
			if (m_Tanks [i].m_Instance.activeSelf) {
				//Return as RoundWinner
				return m_Tanks [i];
			}
		}
		//if no Tanks Active; Draw
		return null;
	}


	//Function is called to find Winner of Ga	me
	private TankDirector GetGameWinner () {
		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//if Tank has won enough rounds to WinGame
			if (m_Tanks[i].m_Wins == m_NumRoundsToWin) {
				//return Tank
				return m_Tanks[i];
			}
		}
		//No Tanks have enough Wins
		return null;
	}


	//Function to Set GameEnd Message
	private string EndMessage () {
		//Set Default to Draw (No Winners)
		string message = "Blimey a DRAW!";
		
		//If there is a Winner
		if (m_RoundWinner != null) {
			//Set Message to Display Winner and Score
			message = m_RoundWinner.m_ColoredPlayerText + " WINS The Round!";
		}
		//Add LineBreaks after first Message
		message += "\n\n\n\n";

		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//Add each Players Score
			message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " Wins\n";
		}
		//if there is a GameWinner
		if (m_GameWinner != null) {
			//Display GameWinner Message
			message = m_GameWinner.m_ColoredPlayerText + "WINS THE GAME! \n\n Tanks! For Playing!";
		}
		//Return the completed Message
		return message;
	}


	//Function to Reset All Tanks (Position and Properties)
	private void ResetAllTanks () {
		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//Call the Tanks Reset Function
			m_Tanks[i].Reset();
		}
	}


	//Function to Enable Tanks Controls
	private void EnableTankControls () {
		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//Call the Enable Tanks Controls Function
			m_Tanks[i].EnableControls(true);
		}
	}


	//Function to Disable Tanks Controls
	private void DisableTankControls () {
		//Go through all Players/Tanks
		for (int i = 0; i < m_Tanks.Length; i++) {
			//Call the Disable Tanks Controls Function
			m_Tanks[i].EnableControls(false);
		}
	}
}
