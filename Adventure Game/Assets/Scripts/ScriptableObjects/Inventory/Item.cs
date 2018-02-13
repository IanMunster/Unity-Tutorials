using UnityEngine;

/// <summary>
/// Item.
/// Is of Class ScriptableObject, This can be Created as Asset.
/// Currently Hold Sprite of Item. Can be Modified for more Complex Inventory Systems
/// </summary>

[CreateAssetMenu]
public class Item : ScriptableObject {
	// Sprite of Item
	public Sprite sprite;
}