using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour {

	public void Restart(){
		Application.LoadLevel (Application.loadedLevel);
	}

}
