using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TestPunch : MonoBehaviour {

	public Vector3 amount;
	public float duration;
	public int vibration;
	public float elastic;



	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (0))
			transform.DOPunchScale (amount, duration, vibration, elastic);

	}
}
