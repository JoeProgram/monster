using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	private static WaveSpawner _instance;
	public static WaveSpawner instance{
		
		get{
			if( _instance == null ){
				_instance = FindObjectOfType<WaveSpawner>();
			}
			return _instance;
		}
	}
	
	public float spawnTime;
	public List<Transform> spawnPoints;

	public Soldier soldierPrefab;


	protected struct WaveData{

		public int soldiers;
		public int stragglers;
		public float timeOut;

		public WaveData( int s, int st, int t){
			soldiers = s;
			stragglers = st;
			timeOut = t;
		}
	}

	protected List<WaveData> waves = new List<WaveData>{
		//new WaveData(50,0,60),  //test wave
		new WaveData(1,0,60),
		new WaveData(3,0,60),
		new WaveData(5,1,60),
		new WaveData(10,1,60),
		new WaveData(15,1,60),
		new WaveData(20,2,60),
		new WaveData(25,2,60),
		new WaveData(30,3,60),
		new WaveData(50,5,60),
	};

	protected int waveCounter = 0;
	public float timeWaveBegan; 
	protected int killed = 0;

	// Use this for initialization
	void Start () {
		StartWave (waveCounter);
	}

	void Update(){

		if ( IsOver() ){
			if( waveCounter < waves.Count - 1) waveCounter += 1;
			StartWave(waveCounter);
		}

	}

	void StartWave(int index){

		killed = 0;
		timeWaveBegan = Time.time;

		for( int i = 0; i < waves[index].soldiers; i++ ){
			SpawnSoldier();
		}
	}

	void SpawnSoldier(){
		Soldier s = Instantiate (soldierPrefab, GetRandomSpawnPosition() + Vector3.right * Random.Range(0,10.0f) + Vector3.back * Random.Range(0,10.0f), Quaternion.identity) as Soldier;
	}

	protected Vector3 GetRandomSpawnPosition(){
		return spawnPoints[ Random.Range(0, spawnPoints.Count)].transform.position;
	}

	public void HumanKilled(){
		killed += 1;
	}

	protected bool IsOver( ){
		return waves[waveCounter].soldiers - killed <= waves[waveCounter].stragglers || Time.time - timeWaveBegan > waves[waveCounter].timeOut ;
	}


}
