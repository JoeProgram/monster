using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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


	public bool limitAngle;
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
	public AudioClip sfxGrowHead;

	public bool showTutorialSelectors;
	public GameObject tutorialSelectorPrefab;

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

			if (eatingState == EatingState.WAITING && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) ){
				StartEating ();
			} else if (eatingState == EatingState.EATING && (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Space))) {
				StopEating ();
			} else {
				biting = false;
			}
		}


		// attempting the nuclear option to fix heads getting too low.
		if (eatingState != EatingState.EATING)
			transform.position = new Vector3 (transform.position.x, HeadPlane.instance.transform.position.y, transform.position.z);


	}

	// handle the head moving around.
	protected void UpdateHold(){

		Vector3 ogHeadPlanePos = new Vector3 (transform.position.x, HeadPlane.instance.transform.position.y, transform.position.z);


		// move the head based on the mouse
		Vector3 hitPoint = GetMouseInWorldPoint ();
		transform.position = hitPoint + heldOffset;

		//correct for it being too far away

		Vector3 parentPlanePos = new Vector3 (parent.transform.position.x, HeadPlane.instance.transform.position.y, parent.transform.position.z);
		Vector3 headPlanePos = new Vector3 (transform.position.x, HeadPlane.instance.transform.position.y, transform.position.z);

		float distance = Vector3.Distance (parentPlanePos, headPlanePos);
		if (distance > maxDistance) {
			distance = maxDistance;
			Vector3 unitVector = (parentPlanePos - headPlanePos).normalized;
			transform.position = parent.transform.position - (unitVector * maxDistance); 
			if (eatingState == EatingState.EATING)
				transform.position += eatingOffset;
		} else if (distance < minDistance) {
			distance = minDistance;
			Vector3 unitVector = (parentPlanePos - headPlanePos).normalized;
			transform.position = parent.transform.position - (unitVector * minDistance);
			if (eatingState == EatingState.EATING)
				transform.position += eatingOffset;
		}

		if (eatingState == EatingState.EATING)
			parentPlanePos -= eatingOffset;

		transform.LookAt (parentPlanePos);

		if (limitAngle) {
			
			//bit of a kludge
			// if the angle is bad, use the previous vector with the new distance 
			if (transform.localEulerAngles.y > 55 && transform.localEulerAngles.y < 275){ 

				Vector3 ogUnitVec = (ogHeadPlanePos - parentPlanePos).normalized;
				transform.position = parentPlanePos + (ogUnitVec * distance) ;
				if (eatingState == EatingState.EATING)
					parentPlanePos -= eatingOffset;
				transform.LookAt (parentPlanePos);

			}
		}
			
	}

	public void Hold(){

		held = true;

		if (eatingState == EatingState.EATING) {
			StopEating();
		}

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
		heldOffset = Vector3.zero;

		//bug fix for our errant head rotation
		if( eatingState != EatingState.EATING ) transform.LookAt (new Vector3( parent.transform.position.x, HeadPlane.instance.transform.position.y, parent.transform.position.z));

	}

	protected void StartEating(){


		eatingState = EatingState.EATING;
		transform.localPosition += eatingOffset;
		transform.RotateAround(transform.position, transform.right, -30);
	
		biting = true;

		// make sure not to fight with the offset when you're holding a head and eating at the same time
		if( held ) heldOffset += eatingOffset;
	}

	public bool IsBiting(){
		return biting;
	}

	protected void StopEating(){

		if (eatingState == EatingState.EATING) {
			eatingState = EatingState.WAITING;
			transform.localPosition -= eatingOffset;
			transform.RotateAround(transform.position, transform.right, 30);

			if( held ) heldOffset -= eatingOffset;

			if( eatingState != EatingState.EATING ) transform.LookAt (new Vector3( parent.transform.position.x, HeadPlane.instance.transform.position.y, parent.transform.position.z));
		}
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

		AudioSource.PlayClipAtPoint (sfxGrowHead, Vector3.zero);

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

		if (showTutorialSelectors) {
			showTutorialSelectors = false;
			StartCoroutine (GrowTutorial ());
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

		// very sloppy on my part.  Running low on time!
		growth.transform.GetChild(0).GetComponent<Head>().headState = HeadState.HEAD;

	}

	// should be moved.  Last 10 minutes!
	protected IEnumerator GrowTutorial(){
		yield return new WaitForSeconds (neckGrowTime + 3);

		List<GameObject> selectors = new List<GameObject> ();

		foreach(Head head in FindObjectsOfType<Head>() ){
			GameObject selector = Instantiate(tutorialSelectorPrefab, head.transform.position, head.transform.rotation) as GameObject;
			selector.transform.parent = head.transform;
			selector.transform.DOLocalRotate (new Vector3(0,0,360), 1, RotateMode.LocalAxisAdd).SetLoops (-1,LoopType.Incremental);
			selectors.Add( selector );
		}

		yield return new WaitForSeconds (5);

		for(int i = selectors.Count - 1; i >=  0; i-- ) {
			Destroy( selectors[i].gameObject );
		}

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
