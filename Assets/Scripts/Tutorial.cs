using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Tutorial : MonoBehaviour {

	public Image tutorialBox;
	public Text tutorialText;

	public List<string> tutorialMessages;
	public int timePerMessage = 10;

	// Use this for initialization
	void Start () {
	
		StartCoroutine (TutorialMessages ());

	}
	
	protected IEnumerator TutorialMessages(){

		yield return new WaitForSeconds (1.5f);
		tutorialBox.gameObject.SetActive(true);

		for (int i = 0; i < tutorialMessages.Count; i++) {

			tutorialBox.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f);

			tutorialText.text = tutorialMessages[i];
			yield return new WaitForSeconds( timePerMessage );
		}

		tutorialBox.gameObject.SetActive (false);

		yield return null;
	}
}
