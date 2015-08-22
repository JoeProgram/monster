using UnityEngine;
using System.Collections;

public class HeadPlane : MonoBehaviour {

	private static HeadPlane _instance;

	public static HeadPlane instance{

		get{
			if( _instance == null ){
				_instance = FindObjectOfType<HeadPlane>();
			}
			return _instance;
		}
	}
	
}
