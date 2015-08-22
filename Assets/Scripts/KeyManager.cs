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

	protected List<KeyCode> keys;
	protected List<KeyCode> masterKeys = new List<KeyCode>{
		KeyCode.Q,
		KeyCode.W,
		KeyCode.E,
		KeyCode.R,
		KeyCode.A,
		KeyCode.S,
		KeyCode.D,
		KeyCode.F,
		KeyCode.Z,
		KeyCode.X,
		KeyCode.C,
		KeyCode.V,
		KeyCode.T,
		KeyCode.G,
		KeyCode.B,
		KeyCode.Y,
		KeyCode.H,
		KeyCode.N,
		KeyCode.U,
		KeyCode.J,
		KeyCode.M,
	};

	public void Awake(){
		keys = new List<KeyCode> ( masterKeys );
	}


	public KeyCode GetKey(){

		if (keys.Count == 0) {
			keys = new List<KeyCode>( masterKeys );
		}
		KeyCode key = keys [0];
		keys.RemoveAt (0);
		return key;
	}

	public void ReturnKey( KeyCode key ){
		keys.Insert(0,key);
	}

}
