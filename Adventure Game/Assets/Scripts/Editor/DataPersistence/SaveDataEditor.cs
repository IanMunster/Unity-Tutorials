using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Save data editor.
/// 
/// </summary>

[CustomEditor (typeof (SaveData) )]
public class SaveDataEditor : Editor {
	
	// 
	private SaveData saveData;



	// 
	private void OnEnable () {
		// 

	}


	// 
	public override void OnInspectorGUI () {
		// 

	}


	//
	private void KeyValuePairListGUI <T> ( string label, SaveData.KeyValuePairLists<T> keyValuePairList, Action<T> specificGUI) {
		
	}

}
