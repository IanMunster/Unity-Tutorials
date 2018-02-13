using UnityEngine;
using UnityEditor; //Required when using Editor-Components (and Overrides)

/// <summary>
/// Serialized property extensions.
/// Extences the SerializedProperty :
/// </summary>

//Does not inherite from anything
public static class SerializedPropertyExtensions {
	// 
	public static void AddToObjectArray <T> (this SerializedProperty arrayProperty, T elementToAdd) 
		where T : Object {
		//
		if (!arrayProperty.isArray) {
			// 
			throw new UnityException ("SerializedProperty " + arrayProperty.name + " is NOT an Array.");
		}
		// 
		arrayProperty.serializedObject.Update ();

		// 
		arrayProperty.InsertArrayElementAtIndex (arrayProperty.arraySize);
		// 
		arrayProperty.GetArrayElementAtIndex (arrayProperty.arraySize - 1).objectReferenceValue = elementToAdd;

		//
		arrayProperty.serializedObject.ApplyModifiedProperties ();
	}


	// 
	public static void RemoveFromObjectArrayAt (this SerializedProperty arrayProperty, int index) {
		// 
		if (index < 0) {
			//
			throw new UnityException ("SerializedProperty " + arrayProperty.name + "");
		}
		// 
		if (!arrayProperty.isArray) {
			// 
			throw new UnityException ("SerializedProperty " + arrayProperty.name + "");
		}
		// 
		if (index > arrayProperty.arraySize - 1) {
			// 
			throw new UnityException ("SerializedProperty " + arrayProperty.name + "" + "");
		}
	}
}
