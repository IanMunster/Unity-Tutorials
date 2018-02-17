using System.Collections;
using UnityEngine;

/// <summary>
/// Wall.
/// 
/// </summary>

public class Wall : MonoBehaviour {

	// Reference to Sprite to Show Damage of Wall (Wall Tile 'Crumbles')
	public Sprite dmgSprite;
	// Hitpoints of the Wall
	public int hitPoints = 4;

	// Reference to SpriteRenderer of Wall
	private SpriteRenderer spriteRender;


	// Use this for initialization
	private void Awake () {
		// Get Reference of SpriteRenderer
		spriteRender = GetComponent <SpriteRenderer> ();
	}


	// Function when Player DamagesWall
	public void DamageWall (int loss) {
		// Set Sprite to Damaged wall Sprite
		spriteRender.sprite = dmgSprite;
		// Subtract given Loss from current Hitpoints
		hitPoints -= loss;
		// Check if Wall is Destroyed
		if (hitPoints <= 0) {
			// Destroy the wall
			gameObject.SetActive (false);
		}
	}
}
