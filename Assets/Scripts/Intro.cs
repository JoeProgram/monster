using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Intro : MonoBehaviour {

	public Text titleLabel;
	public Text scoreLabel;
	protected Color scoreLabelColor;

	public WaveSpawner waveSpawner;

	public GameObject hydra;
	public Vector3 hydraStartPos;
	public Vector3 hydraEndPos;



	void Awake(){
		hydra.transform.position = hydraStartPos;
		waveSpawner.enabled = false;

		scoreLabelColor = scoreLabel.color;
		scoreLabel.color = new Color (scoreLabelColor.r, scoreLabelColor.g, scoreLabelColor.b, 0);
	}

	// Use this for initialization
	void Start () {

		StartCoroutine (IntroSequence ());

	}

	protected IEnumerator IntroSequence(){

		yield return new WaitForSeconds (1.5f);

		Color titleLabelColor = new Color (titleLabel.color.r, titleLabel.color.g, titleLabel.color.b, 1);
		titleLabel.DOColor(titleLabelColor, 3f);
		yield return new WaitForSeconds (3f);


		hydra.transform.DOMove( hydraEndPos, 6);
		yield return new WaitForSeconds(6);


		titleLabelColor = new Color (titleLabel.color.r, titleLabel.color.g, titleLabel.color.b, 0);
		titleLabel.DOColor(titleLabelColor, 1.5f);
		yield return new WaitForSeconds (1.5f);


		scoreLabel.DOColor (scoreLabelColor, 3f);


		waveSpawner.enabled = true;

	}

}
