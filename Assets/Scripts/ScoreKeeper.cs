using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ScoreKeeper : MonoBehaviour {

	private static ScoreKeeper _instance;
	public static ScoreKeeper instance{
		
		get{
			if( _instance == null ){
				_instance = FindObjectOfType<ScoreKeeper>();
			}
			return _instance;
		}
	}

	public Text scoreLabel;
	public int score = 0;

	public void AddScore(int toAdd ){
		score += toAdd;
		scoreLabel.text = "Score:" + score;
	}

}
