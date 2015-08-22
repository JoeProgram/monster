using UnityEngine;
using System.Collections;

public class UIStyle : MonoBehaviour {

	private static UIStyle _instance;
	public static UIStyle instance{
		
		get{
			if( _instance == null ){
				_instance = FindObjectOfType<UIStyle>();
			}
			return _instance;
		}
	}


	public GUIStyle style;



}
