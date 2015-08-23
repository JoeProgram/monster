using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

	protected int health;
	protected NavMeshAgent agent;

	protected Collider target;

	void Start(){
		health = 1;
		agent = GetComponent<NavMeshAgent> ();

		target = Hydra.instance.GetRandomBodyPart();
	}

	void Update(){
		agent.SetDestination (target.transform.position);
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
		WaveSpawner.instance.HumanKilled ();
		Destroy (gameObject);
	}
}
