using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour {

	public void Restart(){
		Application.LoadLevel (Application.loadedLevel);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		} else if (Input.GetKeyDown (KeyCode.R)) {
			Restart();
		}
	}

}
