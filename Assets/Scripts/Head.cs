using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Head : MonoBehaviour {

	public enum HeadState { GROWING, HEAD, NECK, CHARRED };
	public HeadState headState = HeadState.HEAD; 

	public enum EatingState { WAITING, LOWERING, EATING, RAISING };
	protected EatingState eatingState = EatingState.WAITING;
	protected bool biting = false;
	
	public GameObject parent;
	public HeadGeometry geometry;

	public int health;

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
	public float neckGrowTime;


	public AudioClip sfxHurt;

	Color ogColor; // original Color

	// Use this for initialization
	void Start () {
		key = KeyManager.instance.GetKey ();
		ogColor = geometry.GetComponent<Renderer> ().material.color; 

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



			if (Input.GetMouseButtonDown(1) ){
				StartEating ();
			} else if (Input.GetMouseButtonUp(1)) {
				StopEating ();
			} else {
				biting = false;
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
		//Cursor.visible = false;

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

		biting = true;

		// make sure not to fight with the offset when you're holding a head and eating at the same time
		if( held ) heldOffset += eatingOffset;
	}

	public bool IsBiting(){
		return biting;
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

	public void Hurt(){

		AudioSource.PlayClipAtPoint (sfxHurt, Vector3.zero);

		health --;
		Hydra.instance.Hurt ();

		if (Hydra.instance.state != Hydra.HydraState.DEAD) {

			geometry.GetComponent<Renderer> ().material.color = Color.red;
			geometry.GetComponent<Renderer> ().material.DOColor (ogColor, 0.25f);

			if (health <= 0) {
				Cut ();
			}
		}
	}

	public void Cut(){

		if (headState == HeadState.HEAD) {

			if (eatingState == EatingState.EATING)
				StopEating ();
			TurnIntoNeck ();
			Grow ();

		}
	}

	protected void TurnIntoNeck(){
		headState = HeadState.NECK;
		features.SetActive (false);
		KeyManager.instance.ReturnKey (key);

		Hydra.instance.CreateDeadHead (transform);
	}

	public void Grow(){

		for( int i = 0; i < 2; i++ ){

			GameObject growth = Instantiate( Resources.Load ("Prefabs/Pivot")) as GameObject;
			//growth.gameObject.name = "Head" + Random.Range(0,255);
			growth.transform.parent = transform;
			growth.transform.localPosition = Vector3.zero;
			growth.transform.rotation = transform.rotation;
			growth.transform.RotateAround( growth.transform.position, Vector3.up, 60 + -60 * i);

			Transform child = growth.transform.GetChild(0).GetChild(0);

			child.GetComponent<Collider>().enabled = false;
			child.gameObject.SetActive( false );

			StartCoroutine (GrowHeadAnimation (growth));
			 
		}

	
	}

	protected IEnumerator GrowHeadAnimation( GameObject growth ){

		yield return new WaitForSeconds (neckGrowTime);

		Transform child = growth.transform.GetChild(0).GetChild(0);

		Vector3 scale = child.transform.lossyScale;
		child.gameObject.transform.localScale = Vector3.zero;
		child.gameObject.transform.DOScale ( scale, 1 );
		child.gameObject.SetActive( true );
		yield return new WaitForSeconds (1);

		child.GetComponent<Collider>().enabled = true;

		// add the new head to the body parts list
		Hydra.instance.AddBodyPart(growth.transform.GetChild(0).GetComponent<Head>().collider);

	}

	protected void OnDrawGizmos(){
		Gizmos.DrawLine (parent.transform.position, transform.position);
	}

	protected void OnGUI(){
		/*
		if (headState == HeadState.HEAD) {
			Vector3 screenPoint = Camera.main.WorldToScreenPoint (geometry.transform.position + keyUIOffset);
			GUI.Label (new Rect( screenPoint.x - 15, Screen.height - screenPoint.y - 5, 30, 30), keyUI, UIStyle.instance.style);
			GUI.Label (new Rect (screenPoint.x - 15, Screen.height - screenPoint.y, 30, 20), key.ToString (), UIStyle.instance.style);
		}
		*/
	}
}
