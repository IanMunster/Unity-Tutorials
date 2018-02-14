using UnityEngine;
using UnityEditor; // Required when using Editor-Components (and Override)

/// <summary>
/// Condition editor.
/// 
/// </summary>

[CustomEditor ( typeof (Condition) )]
public class ConditionEditor : Editor {

	// 
	public enum EditorType {
		// 

	}


	// 
	public EditorType editorType;
	// 
	public SerializedProperty conditionsProperty;

	//
	private SerializedProperty descriptionProperty;
	// 
	private SerializedProperty satisfiedProperty;
	//
	private SerializedProperty hashProperty;
	//
	private Condition condition;

	//
	private const float conditionButtonWidth = 30f;
	//
	private const float toggleOffset = 30f;
	//
	private const string conditionPropDescriptionName = "description";
	//
	private const string conditionPropSatisfied = "satisfied";
	//
	private const string conditionPropHashName = "hash";
	//
	private const string blackDescription = "No conditions set.";


	// 
	private void OnEnable () {
		// 

	}


	// 
	public override void OnInspectorGUI () {
		// 

	}


	// 
	private void AllConditionsAssetGUI () {
		// 

	}


	// 
	private void ConditionAssetGUI () {
		// 

	}


	//
	private void InteractableGUI () {
		// 

	}


	// 
	public static Condition CreateCondition () {
		// 

	}


	// 
	public static Condition CreateCondition (string description) {
		// 

	}


	//
	private static void SetHash (Condition condition) {
		// 

	}

}
