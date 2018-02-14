using UnityEngine;
using UnityEditor; //Required when using Editor-Components (and Overrides)

/// <summary>
/// Data resetter editor.
/// 
/// </summary>

[CustomEditor (typeof (DataResetter) )]
public class DataResetterEditor : Editor {

	// 
	private DataResetter dataResetter;
	// 
	private SerializedProperty resetterProperty;

	// 
	private const float buttonWidth = 30f;
	//
	private const string dataResetterPropResettableScriptableObjectsName = "resettableScriptableObjects";


	// Called when Script is Enabled
	private void OnEnable () {
		// 

	}


	// Called when Inspector is Open (every frame)
	public override void OnInspectorGUI () {
		// 

	}
}
