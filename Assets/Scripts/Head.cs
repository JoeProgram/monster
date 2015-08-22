using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

	protected GameObject parent;
	public HeadGeometry geometry;
	
	public float maxAngle;
	public float minDistance;
	public float maxDistance;

	public float distanceFactor;

	protected bool held = false;
	public Collider collider;

	protected Vector3 mousePrev;
	protected float distancePrev;

	// Use this for initialization
	void Start () {
		Debug.Log ("Head Start");
		parent = transform.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {

		if (held) {


			Vector3 parentPos = GetParentScreenPosition();

			float distance = ( Input.mousePosition - parentPos ).magnitude;
			float adjustedDistance = (distancePrev - distance) * distanceFactor;

			transform.localPosition += new Vector3(0,0,adjustedDistance);
			transform.localPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, Mathf.Max (minDistance, Mathf.Min (maxDistance, transform.localPosition.z)) );
		
			mousePrev = Input.mousePosition;
			distancePrev = distance;

			if(Input.GetMouseButtonUp(0)){
				Release();
			}

		}



	}

	public void Hold(){

		held = true;
		Cursor.visible = false;

		mousePrev = Input.mousePosition;
		Vector3 parentPos = GetParentScreenPosition(); 

		distancePrev = (mousePrev - parentPos ).magnitude;

	}

	protected void Release(){
		held = false;
		Cursor.visible = true;
	}


	protected Vector3 GetParentScreenPosition(){

		Vector3 parentPos = Camera.main.WorldToScreenPoint(parent.transform.position);
		parentPos.Set (parentPos.x, parentPos.y, 0);
		return parentPos;
	}


	// did the mouse click this?
	protected bool Pick(){

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (collider.Raycast (ray, out hit, Mathf.Infinity)) {
			return true;
		}
		return false;

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
}
