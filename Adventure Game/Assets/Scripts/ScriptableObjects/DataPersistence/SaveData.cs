using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save data.
/// 
/// </summary>

[CreateAssetMenu]
public class SaveData : ResettableScriptableObject {

	// 
	[Serializable]
	public class KeyValuePairLists <T> {
		// 
		public List<string> keys = new List<string> ();
		// 
		public List<T> values = new List<T> ();


		// 
		public void Clear () {
			
		}


		//
		public void TrySetValue (string key, T value) {
			// 

		}


		// 
		public bool TryGetValue (string key, T value) {
			// 
			return false;
		}

	}


	//
	public KeyValuePairLists<bool> boolKeyValuePairLists = new KeyValuePairLists<bool> ();
	//
	public KeyValuePairLists<int> intKeyValuePairLists = new KeyValuePairLists<int> ();
	//
	public KeyValuePairLists<string> stringKeyValuePairLists = new KeyValuePairLists<string> ();
	//
	public KeyValuePairLists<Vector3> vector3KeyValuePairLists = new KeyValuePairLists<Vector3> ();
	//
	public KeyValuePairLists<Quaternion> quaternionKeyValuePairLists = new KeyValuePairLists<Quaternion> ();


	//
	public override void Reset () {
		// 

	}


	//
	private void Save<T>(KeyValuePairLists<T> lists, string key, T value) {
		// 

	}



	//
	private bool Load<T>(KeyValuePairLists<T> lists, string key, T value) {
		// 

	}


	//
	public void Save (string key, bool value) {
		// 

	}



	//
	public void Save (string key, int value) {
		// 

	}


	//
	public void Save (string key, string value) {
		// 

	}


	//
	public void Save (string key, Vector3 value) {
		// 

	}


	//
	public void Save (string key, Quaternion value) {
		// 

	}



	//
	public bool Load (string key, ref bool value) {
		// 

	}


	//
	public bool Load (string key, ref int value) {
		// 

	}


	//
	public bool Load (string key, ref string value) {
		// 

	}


	//
	public bool Load (string key, ref Vector3 value) {
		// 

	}


	//
	public bool Load (string key, ref Quaternion value) {
		// 

	}

}
