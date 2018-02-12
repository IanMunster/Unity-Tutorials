using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor with sub editors.
/// This class acts as a base class for Editors that have Editors nested within them.
/// For example, the InteractableEditor has an array of ConditionCollectionEditors.
/// It's generic types represent the type of Editor array that are nested within this Editor and the target type of those Editors.
/// </summary>
 
public abstract class EditorWithSubEditors<TEditor, TTarget> : Editor {
	//
}
