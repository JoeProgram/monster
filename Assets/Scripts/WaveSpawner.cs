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
	public int soldierSpeedEndGameRamp = 1; // how much faster soliders end up moving when the final wave is repeated.

	protected struct WaveData{

		public int soldiers;
		public int stragglers;
		public float timeOut;
		public float soldierSpeedMultiplier;

		public WaveData( int s, int st, float t, float ssm){
			soldiers = s;
			stragglers = st;
			timeOut = t;
			soldierSpeedMultiplier = ssm;
		}
	}

	protected List<WaveData> waves = new List<WaveData>{
		//new WaveData(50,0,60),  //test wave
		new WaveData(1,0,30,1.8f),
		new WaveData(3,0,30,1.7f),
		new WaveData(5,1,30,1.6f),
		new WaveData(10,1,30,1.4f),
		new WaveData(15,1,30,1.2f),
		new WaveData(20,2,30,1),
		new WaveData(25,2,30,1),
		new WaveData(30,3,30,1),
		new WaveData(50,5,30,1),
		new WaveData(50,0,30,1.5f),
		new WaveData(50,0,30,2f),
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
			if( waveCounter < waves.Count - 1){
				waveCounter += 1;
			} else {
				waves[waveCounter] = new WaveData(waves[waveCounter].soldiers,waves[waveCounter].stragglers,waves[waveCounter].timeOut,waves[waveCounter].soldierSpeedMultiplier + soldierSpeedEndGameRamp);
			}
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
		s.GetComponent<NavMeshAgent>().speed *= waves [waveCounter].soldierSpeedMultiplier;
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
