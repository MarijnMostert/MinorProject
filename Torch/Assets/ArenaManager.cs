using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour {

	private bool ArenaStarted = false;
	public float timeBetweenRounds = 5f;
	public Enemy[] enemiesToSpawn;
	public int[] numberOfEnemiesSpawnTogether;
	public int enemiesPerWave;
	private GameManager gameManager;
	private Vector3 spawnPosition;
	public float maxSpawningDistanceEnemy;
	public float minSpawningDistanceEnemy;
	public float maxSpawningDistanceChest;
	public float minSpawningDistanceChest;
	private int waveNumber = 0;
	public bool[] enemiesKilled;
	public ArenaCanvas arenaCanvasPrefab;
	private ArenaCanvas arenaCanvas;
	public GameObject chestPrefab;
	private int counter;


	void Start () {
		gameManager = GetComponentInParent<GameManager> ();
		if (arenaCanvas == null) {
			arenaCanvas = Instantiate (arenaCanvasPrefab).GetComponent<ArenaCanvas>();
		}
	}
	
	void Update () {
		
	}

	public void StartArena(){
		ArenaStarted = true;
		StartCoroutine (ArenaMode ());
	}

	public void StopArena(){
		ArenaStarted = false;
	}

	IEnumerator ArenaMode(){
		yield return new WaitForSeconds (timeBetweenRounds);
		waveNumber++;
		StartCoroutine(SpawnWave ());
	}

	IEnumerator SpawnWave(){

		Debug.Log ("Wave " + waveNumber + " has started!");
		arenaCanvas.WaveStarted (waveNumber);

		enemiesKilled = new bool[enemiesPerWave];
		int enemyCounter = 0;
		while (true) {
			
			spawnPosition = GetValidSpawnPosition (minSpawningDistanceEnemy, maxSpawningDistanceEnemy);

		//	for (int i = 0; i < Random.Range (numberOfEnemiesSpawnTogether [0], numberOfEnemiesSpawnTogether [1]); i++) {
				GameObject enemy = ObjectPooler.Instance.GetObject (enemiesToSpawn [Random.Range (0, enemiesToSpawn.Length)].ObjectPoolIndex, true, 
					this.spawnPosition, Quaternion.Euler (0f, Random.Range (0, 360), 0f));
				enemiesKilled [enemyCounter] = enemy;
				enemy.GetComponent<Enemy> ().arenaProps.arenaEnemy = true;
				enemy.GetComponent<Enemy> ().arenaProps.index = enemyCounter;
				enemyCounter++;
				//Debug.Log ("Spawn enemy on " + spawnPosition);
		//	}

			if (enemyCounter < enemiesPerWave) {
				yield return new WaitForSeconds (5f);
			} else {
				break;
			}
		}
		Debug.Log ("End of spawnWave Coroutine");
	}

	public void CheckIfWaveComplete(){
		
		counter = 0;
		for (int i = 0; i < enemiesKilled.Length; i++) {
			if (enemiesKilled [i])
				counter++;
		}

		if (counter == enemiesKilled.Length) {
			Debug.Log ("Wave " + waveNumber + " cleared.");
			arenaCanvas.WaveCleared (waveNumber);
			StartCoroutine (BetweenRounds ());
		}
	}

	public void MarkEnemyKilled(int index){
		enemiesKilled [index] = true;
		Debug.Log ("Enemy " + index + " is killed\nCounter: " + counter + " out of " + (enemiesKilled.Length-1));
		CheckIfWaveComplete ();
	}

	IEnumerator BetweenRounds(){
		waveNumber++;
		//Spawnchests
		StartCoroutine(SpawnChests());
		yield return new WaitForSeconds (10f);
		StartCoroutine (SpawnWave ());
	}

	Vector3 GetValidSpawnPosition(float minSpawningDistance, float maxSpawningDistance){
		Vector2 torchPosition = new Vector2 (gameManager.torch.transform.position.x, gameManager.torch.transform.position.z);
		int i = 0;
		while (i < 500) {
			Vector2 spawnPosition = Random.insideUnitCircle * maxSpawningDistance;
			float Distance = (spawnPosition - torchPosition).magnitude;
			if (Distance > minSpawningDistance) {
				return (new Vector3 (spawnPosition.x + gameManager.torch.transform.position.x, 0f, spawnPosition.y + gameManager.torch.transform.position.z)) ;
			}
			i++;
		}
		Debug.Log ("Could not find suitable spawnposition");
		return new Vector3 (0, 0, 0);
	}

	IEnumerator SpawnChests(){
		for (int i = 0; i < Random.Range(2, 5); i++){
			yield return new WaitForSeconds (.2f);
			spawnPosition = GetValidSpawnPosition (minSpawningDistanceChest, maxSpawningDistanceChest);
			GameObject chest = Instantiate (chestPrefab, spawnPosition, Quaternion.Euler (new Vector3(-90f, Random.Range(0, 360), 0f))) as GameObject;
			chest.GetComponent<Chest> ().SetUp (gameManager.dungeonData.dungeonParameters [waveNumber], null);
		}
	}
}
