﻿using UnityEngine;
using System.Collections;

public class HeadGeometry : MonoBehaviour {

	public Head head;

	//protected void Start(){
	//	head = transform.parent.GetComponent<Head> ();
	//}



	protected void Update(){

		if (Pick ()) {
			if (Input.GetMouseButtonDown (0) ) {
				head.Hold ();
			}

			//for debuggin
			//if (Input.GetKeyDown(KeyCode.Space) ) {
		//		head.Cut();
			//}
		}
	}

	// did the mouse click this?
	protected bool Pick(){

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (GetComponent<BoxCollider>().Raycast (ray, out hit, Mathf.Infinity)) {
			return true;
		}
		return false;
		
	}
}
