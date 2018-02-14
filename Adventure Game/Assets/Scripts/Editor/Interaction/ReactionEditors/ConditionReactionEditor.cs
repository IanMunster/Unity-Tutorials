using UnityEditor;

/// <summary>
/// Condition reaction editor.
/// 
/// </summary>

[CustomEditor ( typeof (ConditionReaction) )]
public class ConditionReactionEditor : ReactionEditor {

	// 
	private SerializedProperty conditionProperty;
	//
	private SerializedProperty satisfiedProperty;

	//
	private const string conditionReactionPropConditionName = "condition";
	// 
	private const string conditionReactionPropSatisfiedName = "satisfied";


	//
	protected override void Init () {
		// 

	}

	protected override void DrawReaction () {
		// 

	}

	// 
	protected override string GetFoldoutLabel () {
		// 
		return "Condition Reaction";
	}
}