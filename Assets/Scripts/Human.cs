using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {

	public int health;
	public float nutrition;

	// Use this for initialization
	void Start () {
		health = 1;
	}
	
	void OnCollisionEnter( Collision collision ){
		
		if (collision.gameObject.CompareTag ("hydra") && collision.gameObject.transform.parent.GetComponent<Head> ().IsBiting ()) {
			GetHurt ();
		}
	}

	protected void GetHurt(){
		health -= 1;
		if (health <= 0) {
			Die();
		}
	}

	protected virtual void Die(){
		ScoreKeeper.instance.AddScore (1);
		Hydra.instance.Heal (nutrition);
		WaveSpawner.instance.HumanKilled ();
		Destroy (gameObject);
	}
}
