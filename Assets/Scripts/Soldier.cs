using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

	protected int health;
	protected NavMeshAgent agent;

	void Start(){
		health = 1;
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update(){
		agent.SetDestination (Vector3.zero);
	}

	void OnCollisionEnter( Collision collision ){

		if (collision.gameObject.CompareTag ("hydra") && collision.gameObject.transform.parent.GetComponent<Head>().IsBiting()) {
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
