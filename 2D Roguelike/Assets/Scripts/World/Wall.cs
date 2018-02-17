using System.Collections;
using UnityEngine;

/// <summary>
/// Wall.
/// 
/// </summary>

public class Wall :  MonoBehaviour {

	// Reference to Sprite to Show Damage of Wall (Wall Tile 'Crumbles')
	public Sprite dmgSprite;
	// Hitpoints of the Wall
	public int hitPoints = 2;

	// Sound Fx for WallDamage
	public AudioClip dmgWall0;
	public AudioClip dmgWall1;

	// Reference to SpriteRenderer of Wall
	private SpriteRenderer spriteRender;


	// Use this for initialization
	private void Awake () {
		// Get Reference of SpriteRenderer
		spriteRender = GetComponent <SpriteRenderer> ();
	}


	// Function when Player DamagesWall
	public void DamageWall (int loss) {
		// Play Random WallDamage Sfx
		SoundManager.instance.RandomizeSfx (dmgWall0, dmgWall1);
		// Subtract given Loss from current Hitpoints
		hitPoints -= loss;
		// If lost half of HitPoints
		if (hitPoints < hitPoints/2) {
			// Set Sprite to Damaged wall Sprite
			spriteRender.sprite = dmgSprite;
		}
		// Check if Wall is Destroyed
		if (hitPoints <= 0) {
			// Destroy the wall
			gameObject.SetActive (false);
		}
	}
}
