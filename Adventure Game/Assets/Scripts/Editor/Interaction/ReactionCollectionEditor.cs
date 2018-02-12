using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Reaction collection editor.
/// This is the Editor for the ReactionCollection MonoBehaviour.
/// However, since the ReactionCollection contains many Reactions, it requires many sub-editors to display them.
/// For more details see the EditorWithSubEditors class.
/// There are two ways of adding Reactions to the ReactionCollection: a type selection popup with confirmation button and a drag and drop area. 
/// Details on these are found below.
/// </summary>

public class ReactionCollectionEditor : EditorWithSubEditors<ReactionEditor, Reaction> {
	//
}
