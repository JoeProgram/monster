using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Soldier : Human {

	public enum SoldierState { WALKING, ATTACKING, CELEBRATING };
	public SoldierState soldierState = SoldierState.WALKING;

	public float attackDistance;
	public float jumpForce;
	public float minJumpUp = 100;

	protected NavMeshAgent agent;
	
	protected Collider target;

	public GameObject sword;

	public AudioClip sfxJump;

	protected Tween swordTween;

	public int celeberateForce;

	void Start(){
		health = 1;
		agent = GetComponent<NavMeshAgent> ();

		target = Hydra.instance.GetRandomBodyPart();

	}

	void Update(){

		if (Hydra.instance.state == Hydra.HydraState.DEAD && soldierState != SoldierState.CELEBRATING) {
			StartCoroutine(StartCelebration());
		}

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

		if (collision.gameObject.CompareTag ("hydra") && collision.gameObject.transform.parent.GetComponent<Head> ().IsBiting ()) {
			GetHurt ();
		} else if (collision.gameObject.CompareTag ("hydra") && collision.collider == target && soldierState == SoldierState.ATTACKING ) {
			collision.gameObject.GetComponentInParent<Head>().Hurt();
		}

		if (soldierState == SoldierState.ATTACKING && collision.gameObject.CompareTag ("floor")) {
			StartWalk();
		}

		if (soldierState == SoldierState.CELEBRATING && collision.gameObject.CompareTag ("floor")) {
			GetComponent<Rigidbody> ().AddForce(Vector3.up * celeberateForce);
		}
	}

	public void StartAttack(){
		soldierState = SoldierState.ATTACKING;
		agent.enabled = false;
		swordTween = sword.transform.DOLocalRotate (new Vector3 (0, -83, -20), 0.2f).SetLoops (-1, LoopType.Yoyo);
		StartCoroutine (AttackJump ());
	}

	protected IEnumerator AttackJump(){

		// wait a frame to make sure the agent is disabled
		yield return null;

		AudioSource.PlayClipAtPoint (sfxJump, Camera.main.transform.position);
		GetComponent<Rigidbody> ().AddForce((target.transform.position - transform.position).normalized * jumpForce + Vector3.up * minJumpUp);
	}

	public void StartWalk(){
		soldierState = SoldierState.WALKING;
		agent.enabled = true;
	}

	public IEnumerator StartCelebration(){
		soldierState = SoldierState.CELEBRATING;
		agent.enabled = false;
		yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
		transform.LookAt (new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z) );
		GetComponent<Rigidbody> ().AddForce(Vector3.up * celeberateForce);
	}


}
