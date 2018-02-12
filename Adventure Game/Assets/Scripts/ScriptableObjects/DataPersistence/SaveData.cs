using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Save data.
/// Instance of this Class can be Created as Assets.
/// Each Instance Contains Collection of Data from "Saver"MonoBehaviour references.
/// Scince Assets exits outside of Scene, Data will be Peristant between Scenes
/// 
/// *Note that these assets DO NOT persist between loads of a build
///  and can therefore NOT be used for saving the gamestate to disk.*
/// </summary>

[CreateAssetMenu]
public class SaveData : ResettableScriptableObject {
	//
}
