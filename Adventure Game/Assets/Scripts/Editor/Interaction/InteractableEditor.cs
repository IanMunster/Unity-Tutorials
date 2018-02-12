using UnityEngine;
using UnityEditor;

/// <summary>
/// Interactable editor.
/// This is the Editor for the Interactable MonoBehaviour.
/// However, since the Interactable contains many sub-objects, it requires many sub-editors to display them.
/// For more details see the EditorWithSubEditors class.
/// </summary>

public class InteractableEditor : EditorWithSubEditors<ConditionCollectionEditor, ConditionCollection> {
	//
}
