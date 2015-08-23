using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Intro : MonoBehaviour {

	public GameObject hydra;
	public Vector3 hydraStartPos;
	public Vector3 hydraEndPos;


	void Awake(){
		hydra.transform.position = hydraStartPos;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (IntroSequence ());
	}

	protected IEnumerator IntroSequence(){

		yield return new WaitForSeconds (1.5f);

		hydra.transform.DOMove( hydraEndPos, 3);
		yield return new WaitForSeconds(3);

	}

}
