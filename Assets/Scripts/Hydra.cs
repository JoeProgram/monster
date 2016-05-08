using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hydra : MonoBehaviour {

	public enum HydraState {ALIVE, DEAD};
	public HydraState state = HydraState.ALIVE;

	public List<Collider> bodyParts;

	public List<AudioClip> biteSounds;

	public float health;
	public float maxHealth;
	public Slider healthBar;
	public GameObject healthFill;

	public float deadHeadSidewaysForce;
	public float deadHeadUpForce;

	public Material deadMaterial;
	public AudioClip sfxDeath;

	private static Hydra _instance;
	public static Hydra instance{
		
		get{
			if( _instance == null ){
				_instance = FindObjectOfType<Hydra>();
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		healthBar.maxValue = maxHealth;
		healthBar.value = health;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (1) || Input.GetKeyDown(KeyCode.Space)) {
			PlayBiteSound();
		}
	}

	public void PlayBiteSound(){
		if (state == HydraState.ALIVE) {
			AudioSource.PlayClipAtPoint (biteSounds [Random.Range (0, biteSounds.Count)],Camera.main.transform.position);
		}
	}

	public void AddBodyPart(Collider c){
		bodyParts.Add (c);
	}

	public Collider GetRandomBodyPart(){
		return bodyParts[ Random.Range(0, bodyParts.Count) ];
	}

	public void Heal(float amount){
		if (state == HydraState.ALIVE) {
			health += amount;
			health = Mathf.Min(maxHealth,health);
			healthBar.value = health;
		}
	}

	public void Hurt(){

		health -= 1;
		healthBar.value = health;

		if (health <= 0) {
			healthFill.SetActive(false);
			Die();
		}
	}

	protected void Die(){
		state = HydraState.DEAD;

		AudioSource.PlayClipAtPoint (sfxDeath, Camera.main.transform.position);

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material = deadMaterial;
		}

		foreach (Head head in GetComponentsInChildren<Head>()) {
			if( head.headState == Head.HeadState.HEAD ){
				CreateDeadHead(head.transform);
				head.gameObject.SetActive(false);
			}
		}



		foreach (GameObject neckPiece in GameObject.FindGameObjectsWithTag("neck_piece")) {
			neckPiece.GetComponent<BoxCollider>().enabled = true;
		}

		foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()){

			rb.isKinematic = false;
			rb.AddForce(Vector3.up * 500);

		}

		Ending.instance.End ();

		 
	}

	public void CreateDeadHead(Transform head){

		GameObject deadhead = Instantiate(Resources.Load ("Prefabs/DeadHead")) as GameObject;
		deadhead.transform.position = head.transform.position + Vector3.up;
		deadhead.transform.rotation = head.transform.rotation;
		deadhead.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range (-deadHeadSidewaysForce, deadHeadSidewaysForce),  deadHeadUpForce, Random.Range (-deadHeadSidewaysForce, deadHeadSidewaysForce)));
	}


}
