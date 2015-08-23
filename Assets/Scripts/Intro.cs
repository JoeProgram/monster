using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Intro : MonoBehaviour {

	public Text titleLabel;
	public Text scoreLabel;
	protected Color scoreLabelColor;

	public Text healthLabel;
	public Image healthFill;
	public Image healthBackground;

	public WaveSpawner waveSpawner;

	public GameObject hydra;
	public Vector3 hydraStartPos;
	public Vector3 hydraEndPos;

	public GameObject startingHead;

	public SpriteRenderer leftClickMouse;
	public SpriteRenderer rightClickMouse;
	public SpriteRenderer tutorialSelect;
	public Text instructions;

	public Human princess;

	public Canvas hydraCanvas;

	public AudioClip sfxGrowl;



	void Awake(){
		hydra.transform.position = hydraStartPos;
		waveSpawner.enabled = false;

		hydraCanvas.gameObject.SetActive (true);

		scoreLabelColor = scoreLabel.color;
		scoreLabel.color = new Color (scoreLabelColor.r, scoreLabelColor.g, scoreLabelColor.b, 0);

		DOTween.ToAlpha (() => healthLabel.color, x => healthLabel.color = x, 0, 0);
		DOTween.ToAlpha (() => healthFill.color, x => healthFill.color = x, 0, 0);
		DOTween.ToAlpha (() => healthBackground.color, x => healthBackground.color = x, 0, 0);

	}

	// Use this for initialization
	void Start () {

		StartCoroutine (IntroSequence ());

		tutorialSelect.transform.DOLocalRotate (new Vector3(0,0,360), 1, RotateMode.LocalAxisAdd).SetLoops (-1,LoopType.Incremental);

	}

	protected IEnumerator IntroSequence(){

		yield return new WaitForSeconds (1.5f);

		AudioSource.PlayClipAtPoint(sfxGrowl, Vector3.zero);

		Color titleLabelColor = new Color (titleLabel.color.r, titleLabel.color.g, titleLabel.color.b, 1);
		titleLabel.DOColor(titleLabelColor, 3f);
		yield return new WaitForSeconds (3f);


		hydra.transform.DOMove( hydraEndPos, 5);
		yield return new WaitForSeconds(5);


		titleLabelColor = new Color (titleLabel.color.r, titleLabel.color.g, titleLabel.color.b, 0);
		titleLabel.DOColor(titleLabelColor, 1f);
		yield return new WaitForSeconds (1f);


		// How to drag

		Vector3 headPos = startingHead.transform.position;

		instructions.text = "Drag";
		DOTween.ToAlpha (() => tutorialSelect.color, x => tutorialSelect.color = x, 1, 0.5f);
		DOTween.ToAlpha (() => leftClickMouse.color, x => leftClickMouse.color = x, 1, 0.5f);
		DOTween.ToAlpha (() => instructions.color, x => instructions.color = x, 1, 0.5f);
		yield return new WaitForSeconds (0.5f);

		// wait for player to move head
		while (Vector3.Distance(headPos,startingHead.transform.position) < 3) {
			yield return null;
		}

		yield return new WaitForSeconds (1);

		DOTween.ToAlpha (() => tutorialSelect.color, x => tutorialSelect.color = x, 0, 0.5f);
		DOTween.ToAlpha (() => leftClickMouse.color, x => leftClickMouse.color = x, 0, 0.5f);
		DOTween.ToAlpha (() => instructions.color, x => instructions.color = x, 0, 0.5f);
		yield return new WaitForSeconds (0.5f);

		DOTween.ToAlpha (() => healthLabel.color, x => healthLabel.color = x, 1, 0.5f);
		DOTween.ToAlpha (() => healthFill.color, x => healthFill.color = x, 1, 0.5f);
		DOTween.ToAlpha (() => healthBackground.color, x => healthBackground.color = x, 1, 0.5f);
		scoreLabel.DOColor (scoreLabelColor, 0.5f);
		
		instructions.text = "Bite";
		DOTween.ToAlpha (() => rightClickMouse.color, x => rightClickMouse.color = x, 1, 0.5f);
		DOTween.ToAlpha (() => instructions.color, x => instructions.color = x, 1, 0.5f);

		yield return new WaitForSeconds (1.5f);


		//wait for person to right click
		while (princess != null) {
			yield return null;
		}



		DOTween.ToAlpha (() => rightClickMouse.color, x => rightClickMouse.color = x, 0, 0.5f);
		DOTween.ToAlpha (() => instructions.color, x => instructions.color = x, 0, 0.5f);

		yield return new WaitForSeconds (0.5f);

		yield return new WaitForSeconds (1);
		waveSpawner.enabled = true;

		yield return new WaitForSeconds(3);
		hydraCanvas.gameObject.SetActive (false);

		
	}

}
