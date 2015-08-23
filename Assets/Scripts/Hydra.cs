using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hydra : MonoBehaviour {

	public List<Collider> bodyParts;

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

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddBodyPart(Collider c){
		bodyParts.Add (c);
	}

	public Collider GetRandomBodyPart(){
		return bodyParts[ Random.Range(0, bodyParts.Count) ];
	}
}
