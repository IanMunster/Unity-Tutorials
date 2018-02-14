using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Reaction editor.
/// 
/// </summary>

public abstract class ReactionEditor : Editor {

	//
	public bool showReaction;
	// 
	public SerializedProperty reactionsProperty;

	//
	private Reaction reaction;

	//
	private const float buttonWidth = 30f;


	//
	private void OnEnable () {
		// 

	}


	//
	protected virtual void Init () {}


	//
	public override void OnInspectorGUI () {
		// 

	}


	// 
	public static Reaction CreateReaction (Type reactionType) {
		// 

	}


	// 
	protected virtual void DrawReaction () {}


	//
	protected abstract string GetFoldoutLabel ();

}
