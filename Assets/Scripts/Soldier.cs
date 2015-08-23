using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Soldier : MonoBehaviour {

	public enum SoldierState { WALKING, ATTACKING };
	public SoldierState soldierState = SoldierState.WALKING;

	public float attackDistance;
	public float jumpForce;

	protected int health;
	protected NavMeshAgent agent;
	
	protected Collider target;

	public GameObject sword;

	protected Tween swordTween;

	void Start(){
		health = 1;
		agent = GetComponent<NavMeshAgent> ();

		target = Hydra.instance.GetRandomBodyPart();

	}

	void Update(){

		if (soldierState == SoldierState.WALKING) {
			agent.SetDestination (target.transform.position);
		}

		if (soldierState == SoldierState.WALKING && attackDistance > Vector3.Distance(transform.position,target.transform.position)) {
			//Debug.Log ( "" + agent.pathStatus + " " + agent.remainingDistance  );
			//Debug.Log ( "Agents remainin distance to " + target.name + " is " + agent.remainingDistance );
			StartAttack();
		} 

	}

	void OnCollisionEnter( Collision collision ){

		if (collision.gameObject.CompareTag ("hydra") && collision.gameObject.transform.parent.GetComponent<Head>().IsBiting()) {
			GetHurt();
		}

		if (soldierState == SoldierState.ATTACKING && collision.gameObject.CompareTag ("floor")) {
			StartWalk();
		}
	}

	public void StartAttack(){
		soldierState = SoldierState.ATTACKING;
		agent.enabled = false;
		swordTween = sword.transform.DOLocalRotate (new Vector3 (0, -83, -20), 0.2f).SetLoops (-1, LoopType.Yoyo);
		StartCoroutine (AttackJump ());
	}

	protected IEnumerator AttackJump(){
		yield return null;
		GetComponent<Rigidbody> ().AddForce((target.transform.position - transform.position).normalized * jumpForce);
	}

	public void StartWalk(){
		soldierState = SoldierState.WALKING;
		agent.enabled = true;
	}

	protected void GetHurt(){
		health -= 1;
		if (health <= 0) {
			Die();
		}
	}

	protected void Die(){
		ScoreKeeper.instance.AddScore (1);
		WaveSpawner.instance.HumanKilled ();
		Destroy (gameObject);
	}
}
