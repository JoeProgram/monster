using UnityEngine;
using System.Collections;

public class HeadGeometry : MonoBehaviour {

	Head head;

	protected void Start(){
		head = transform.parent.GetComponent<Head> ();
	}

	protected void OnMouseDown(){
		Debug.Log ("onmousedown");
		head.Hold ();
	}

}
