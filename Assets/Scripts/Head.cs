﻿using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

	public enum HeadState { GROWING, HEAD, NECK, CHARRED };
	public HeadState headState = HeadState.HEAD; 

	public enum EatingState { WAITING, LOWERING, EATING, RAISING };
	protected EatingState eatingState = EatingState.WAITING;
	
	public GameObject parent;
	public HeadGeometry geometry;
	
	public float maxAngle;
	public float minDistance;
	public float maxDistance;

	public float distanceFactor;

	protected bool held = false;
	protected Vector3 heldOffset;
	public Collider collider;

	public KeyCode key;
	public Vector3 keyUIOffset;

	public Vector3 eatingOffset;
	public Vector3 eatingRotationOffset;

	public GameObject features;

	public Texture2D keyUI;


	// Use this for initialization
	void Start () {

		key = KeyManager.instance.GetKey ();

	}
	
	// Update is called once per frame
	void Update () {


		// Movement Code
		if (held) {

			if(Input.GetMouseButtonUp(0)){
				Release();
			} else {
				UpdateHold ();
			}


		}

		// Eat if you're ahead.
		if (headState == HeadState.HEAD) {
			if (Input.GetKeyDown (key)) {
				StartEating ();
			} else if (Input.GetKeyUp (key)) {
				StopEating ();
			}
		}

	}

	// handle the head moving around.
	protected void UpdateHold(){

		// move the head based on the mouse
		Vector3 hitPoint = GetMouseInWorldPoint();
		transform.position = hitPoint + heldOffset;

		//correct for it being too far away

		Vector3 parentPlanePos = new Vector3( parent.transform.position.x, 0, parent.transform.position.z);
		Vector3 headPlanePos = new Vector3(transform.position.x, 0, transform.position.z);

		float distance = Vector3.Distance (parentPlanePos, headPlanePos);
		if (distance > maxDistance) {
			Vector3 unitVector = (parentPlanePos - headPlanePos).normalized;
			transform.position = parent.transform.position - (unitVector * maxDistance); 
			if( eatingState == EatingState.EATING ) transform.position += eatingOffset;
		} else if (distance < minDistance) {
				Vector3 unitVector = (parentPlanePos - headPlanePos).normalized;
				transform.position = parent.transform.position - (unitVector * minDistance);
				if( eatingState == EatingState.EATING ) transform.position += eatingOffset;
		}

		transform.LookAt (parent.transform.position);
	}

	public void Hold(){

		held = true;
		Cursor.visible = false;

		// find the offset between the center of the object and the mouse hit on headplane
		Vector3 hitPoint = GetMouseInWorldPoint();
		heldOffset = transform.position - hitPoint; 

	}

	protected Vector3 GetMouseInWorldPoint(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		HeadPlane.instance.GetComponent<Collider>().Raycast (ray, out hit, Mathf.Infinity);
		return hit.point; 
	}

	protected void Release(){
		held = false;
		Cursor.visible = true;
	}

	protected void StartEating(){
		eatingState = EatingState.EATING;
		transform.localPosition += eatingOffset;
		transform.localEulerAngles += eatingRotationOffset;

		// make sure not to fight with the offset when you're holding a head and eating at the same time
		if( held ) heldOffset += eatingOffset;
	}

	protected void StopEating(){
		eatingState = EatingState.WAITING;
		transform.localPosition -= eatingOffset;
		transform.localEulerAngles -= eatingRotationOffset;

		if( held ) heldOffset -= eatingOffset;
	}


	protected Vector3 GetParentScreenPosition(){

		Vector3 parentPos = Camera.main.WorldToScreenPoint(parent.transform.position);
		parentPos.Set (parentPos.x, parentPos.y, 0);
		return parentPos;
	}

	public void Cut(){
		if (eatingState == EatingState.EATING) StopEating ();
		TurnIntoNeck ();
		Grow ();
	}

	protected void TurnIntoNeck(){
		headState = HeadState.NECK;
		features.SetActive (false);
		KeyManager.instance.ReturnKey (key);
	}

	public void Grow(){

		for( int i = 0; i < 2; i++ ){

			GameObject growth = Instantiate( Resources.Load ("Prefabs/Pivot")) as GameObject;
			growth.gameObject.name = "Head" + Random.Range(0,255);
			growth.transform.parent = transform;
			growth.transform.localPosition = Vector3.zero;
			growth.transform.rotation = transform.rotation;
			growth.transform.RotateAround( growth.transform.position, Vector3.up, -30 + 60 * i);
		}
	
	}

	protected void OnDrawGizmos(){
		Gizmos.DrawLine (parent.transform.position, transform.position);
	}

	protected void OnGUI(){

		if (headState == HeadState.HEAD) {
			Vector3 screenPoint = Camera.main.WorldToScreenPoint (geometry.transform.position + keyUIOffset);
			GUI.Label (new Rect( screenPoint.x - 15, Screen.height - screenPoint.y - 5, 30, 30), keyUI, UIStyle.instance.style);
			GUI.Label (new Rect (screenPoint.x - 15, Screen.height - screenPoint.y, 30, 20), key.ToString (), UIStyle.instance.style);
		}
	}
}