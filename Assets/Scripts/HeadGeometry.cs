using UnityEngine;
using System.Collections;

public class HeadGeometry : MonoBehaviour {

	Head head;

	protected void Start(){
		head = transform.parent.GetComponent<Head> ();
	}



	protected void Update(){

		if (Pick ()) {
			if (Input.GetMouseButtonDown (0) ) {
				head.Hold ();
			}

			if (Input.GetMouseButtonDown (1)) {
				head.Grow();
			}
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
