using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Ending : MonoBehaviour {

	private static Ending _instance;
	public static Ending instance{
		
		get{
			if( _instance == null ){
				_instance = FindObjectOfType<Ending>();
			}
			return _instance;
		}
	}

	public Button restartButton;

	public void End(){
		StartCoroutine (EndingSequence ());
	}

	protected IEnumerator EndingSequence(){
		yield return new WaitForSeconds (2.5f);

		restartButton.gameObject.SetActive (true);

		//DOTween.ToAlpha (() => restartButton., x => restartButton.colors.normalColor = x, 1, 0.5f);


	}

}
