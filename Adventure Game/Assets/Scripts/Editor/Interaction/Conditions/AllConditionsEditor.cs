using UnityEngine;
using UnityEditor;

/// <summary>
/// All conditions editor.
/// 
/// </summary>

[CustomEditor ( typeof (AllConditions) )]
public class AllConditionsEditor : Editor {


	// 
	public static string[] AllConditionDescriptions {
		// 
		get {
			// 

		}
		// 
		private set {
			// 

		}
	}



	// 
	private static string[] allConditionDescriptions;

	// 
	private ConditionEditor[] conditionEditors;
	// 
	private AllConditions allConditions;
	// 
	private string newConditionDescription = "New Condition";

	//
	private const string creationPath = "Assets/Resources/AllConditions.asset";
	// 
	private const float buttonWidth = 30f;


	//
	private void OnEnable () {
		// 

	}



	// 
	private void OnDisable () {
		// 

	}


	//
	private static void SetAllConditionDescriptions () {
		// 

	}


	// 
	public override void OnInspectorGUI () {
		// 

	}


	// 
	private void CreateEditors () {
		// 

	}



	//
	[MenuItem("Assets/Create/AllConditions")]
	private static void CreateAllConditionsAsset () {
		// 

	}


	//
	private void AddCondition (string description) {
		// 

	}


	// 
	public static void RemoveCondition (Condition condition) {
		// 

	}


	// 
	public static int TryGetConditionIndex (Condition condition) {
		// 

	}


	// 
	public static Condition TryGetConditionAt (int index) {
		// 

	} 


	// 
	public static int TryGetConditionsLength () {
		// 

	}

}
