using System.Collections;
using UnityEngine;

/// <summary>
/// Sound manager.
/// 
/// </summary>

public class SoundManager : MonoBehaviour {

	// Create new Static Instance of SoundManager
	public static SoundManager instance = null;
	// Reference to First AudioSource Component on SoundManager
	public AudioSource sfxSource;
	// Reference to Second AudioSource Component on SoundManager
	public AudioSource musicSource;
	// Lowest Pitch for AudioPitch Change (5% of totalPitch)
	public float lowPitchRange = 0.95f;
	// Highest Pitch for AudioPitch Change  (5% of totalPitch)
	public float highPitchRange = 1.05f;


	// Use this for initialization
	private void Awake () {
		// Check if there is and Instance of SoundManager
		if (instance == null) {
			// No Instance of SoundManager Found, set this to Instance
			instance = this;
			musicSource.Play ();
		// Otherwise if SoundManager Instance Found, but not Instance of tihs
		} else if (instance != this) {
			// Destroy the Instance
			Destroy (gameObject);
		}

		// Dont Destroy the SoundManager on LevelLoad
		DontDestroyOnLoad (gameObject);
	}


	// Function used to Play Single AudioClips
	public void PlaySingleAudioClip (AudioClip clip) {
		// Set given Clip to FX Clip
		sfxSource.clip = clip;
		// Play the FX clip
		sfxSource.Play ();
	}


	// Function to Randomize FX Clips and Pitch (Can take multiple AudioClips, spererated by comma's)
	public void RandomizeSfx (params AudioClip [] clips) {
		// Get randomIndex for 0 to length of given ClipsArray
		int randomIndex = Random.Range (0, clips.Length);
		// Get randomPitch for PitchChange of sFX
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
		// Set sFX pitch to RandomPitch
		sfxSource.pitch = randomPitch;
		// Set sFX Clip to RandomClip
		sfxSource.clip = clips[randomIndex];
		// Play the sFX Clip
		sfxSource.Play ();
	}
}