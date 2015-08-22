using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyManager : MonoBehaviour {

	private static KeyManager _instance;

	public static KeyManager instance
	{
		get
		{
			if(_instance == null ){
				GameObject gameObject = new GameObject();
				_instance = gameObject.AddComponent<KeyManager>();
			}
			return _instance;

		}
	}

	protected List<KeyCode> keys = new List<KeyCode>{
		KeyCode.G,
		KeyCode.H,
		KeyCode.F,
		KeyCode.T,
		KeyCode.B,
		KeyCode.V,
		KeyCode.Y,
		KeyCode.R,
		KeyCode.N,
		KeyCode.D,
		KeyCode.J,
		KeyCode.C,
		KeyCode.U,
	};

	public KeyCode GetKey(){
		KeyCode key = keys [0];
		keys.RemoveAt (0);
		return key;
	}

}
