using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Interactable editor.
/// 
/// </summary>

[CustomEditor (typeof (Interactable) )]
public class InteractableEditor : EditorWithSubEditors <ConditionCollectionEditor, ConditionCollection> {

	// 
	private Interactable interactable;

	//
	private SerializedProperty interactionLocationProperty;
	//
	private SerializedProperty collectionProperty;
	//
	private SerializedProperty defaultReactionProperty;

	//
	private const float collectionButtonWidth = 125f;
	//
	private const string interactablePropInteractionLocationName = "interactionLocation";
	//
	private const string interactablePropConditionCollectionName = "conditionCollection";
	//
	private const string InteractablePropDefaultReactionCollectionName = "defaultReactionCollection";


	// 
	private void OnEnable () {
		// 

		// Cache the SerializeProperties
		interactionLocationProperty = serializedObject.FindProperty (interactablePropInteractionLocationName);
		collectionProperty = serializedObject.FindProperty (interactablePropConditionCollectionName);
		defaultReactionProperty = serializedObject.FindProperty (InteractablePropDefaultReactionCollectionName);

		// 
		interactable = (Interactable)target;
		//
		CheckAndCreateSubEditors (interactable.conditionCollections);
	}


	//
	private void OnDisable () {
		// 
		CleanupEditors ();
	}

	// 
	protected override void SubEditorSetup (ConditionCollectionEditor editor) {
		// 
//		editor.collectionsProperty = collectionProperty;

	}



	// 
	public override void OnInspectorGUI () {
		// 
		serializedObject.Update ();

		//
		CheckAndCreateSubEditors (interactable.conditionCollections);
		// 
		EditorGUILayout.PropertyField (interactionLocationProperty);
/*	
		// 
		for (int i = 0; i < subEditors.Length; i++) {
			// 
			subEditors[i].OnInspectorGUI ();
			//
			EditorGUILayout.Space ();
		}
*/
		// 
		EditorGUILayout.BeginHorizontal ();
		// 
		GUILayout.FlexibleSpace ();
		// 
		if (GUILayout.Button ("Add Collection", GUILayout.Width (collectionButtonWidth) ) ) {
			// 
			ConditionCollection newCollection = ConditionCollectionEditor.CreateConditionCollection ();
			// 
			collectionProperty.AddToObjectArray (newCollection);
		}
		//
		EditorGUILayout.EndHorizontal ();

		// 
		EditorGUILayout.Space ();
		// 
		EditorGUILayout.PropertyField (defaultReactionProperty);

		// 
		serializedObject.ApplyModifiedProperties ();
	}

}
