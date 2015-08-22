using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

	protected int health;

	void Start(){
		health = 1;
	}

	void OnCollisionEnter( Collision collision ){

		if (collision.gameObject.CompareTag ("hydra")) {
			GetHurt();
		}
	}

	protected void GetHurt(){
		health -= 1;
		if (health <= 0) {
			Die();
		}
	}

	protected void Die(){
		Destroy (gameObject);
	}
}
