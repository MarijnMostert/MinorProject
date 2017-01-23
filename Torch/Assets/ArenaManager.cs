using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour {

	private bool ArenaStarted = false;
	public float timeBetweenRounds = 5f;
	public Enemy[] enemiesToSpawn;
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
	private GameObject[] ArenaAreas;
	private GameObject ArenaAreasParent;
	public GameObject ArenaAreaPicked;

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
		//find Arena Areas
		ArenaAreasParent = GameObject.FindGameObjectWithTag ("ArenaArea");
		Component[] ArenaAreaComponents = ArenaAreasParent.GetComponentsInChildren (typeof(ArenaArea), true);
		ArenaAreas = new GameObject[ArenaAreaComponents.Length];
		for(int i = 0; i < ArenaAreaComponents.Length; i++) {
			ArenaAreas [i] = ArenaAreaComponents [i].gameObject;
			}
		StartCoroutine (ArenaMode ());
	}

	public void StopArena(){
		waveNumber = 0;
		ArenaStarted = false;
	}

	IEnumerator ArenaMode(){
		yield return new WaitForSeconds (timeBetweenRounds);
		waveNumber++;
		StartCoroutine(AreaPicker());
	}

	IEnumerator SpawnWave(int enemiesPerWave, Enemy[] enemiesToSpawn){
		counter = 0;
		Debug.Log ("Wave " + waveNumber + " has started!");
		arenaCanvas.WaveStarted (waveNumber);

		enemiesKilled = new bool[enemiesPerWave];
		int enemyCounter = 0;
		while (true) {
			
			spawnPosition = GetValidSpawnPosition (minSpawningDistanceEnemy, maxSpawningDistanceEnemy);

		
				GameObject enemy = ObjectPooler.Instance.GetObject (enemiesToSpawn [Random.Range (0, enemiesToSpawn.Length)].ObjectPoolIndex, true, 
					this.spawnPosition, Quaternion.Euler (0f, Random.Range (0, 360), 0f));
				enemiesKilled [enemyCounter] = enemy;
				enemy.GetComponent<Enemy> ().arenaProps.arenaEnemy = true;
				enemy.GetComponent<Enemy> ().arenaProps.index = enemyCounter;
				enemyCounter++;

			if (enemyCounter < enemiesPerWave) {
				yield return new WaitForSeconds (5f);
			} else {
				break;
			}
		}
		Debug.Log ("End of spawnWave Coroutine");
	}

	public void CheckIfWaveComplete(){
		
		/*for (int i = 0; i < enemiesKilled.Length; i++) {
			if (enemiesKilled [i])
				
		}*/

		if (counter == enemiesKilled.Length) {
			Debug.Log ("Wave " + waveNumber + " cleared.");
			arenaCanvas.WaveCleared (waveNumber);
			//Turn off ArenaArea
			ArenaAreaPicked.SetActive (false);
			StartCoroutine (BetweenRounds ());
		}
	}

	public void MarkEnemyKilled(int index){
		enemiesKilled [index] = true;
		counter++;
		Debug.Log ("Enemy " + (index) + " is killed\nCounter: " + counter + " out of " + (enemiesKilled.Length));
		CheckIfWaveComplete ();
	}

	IEnumerator BetweenRounds(){
		waveNumber++;
		//Spawnchests
		StartCoroutine(SpawnChests());
		yield return new WaitForSeconds (10f);
		StartCoroutine (AreaPicker());
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

	IEnumerator AreaPicker(){
		//Pick a random area
		ArenaAreaPicked = ArenaAreas [Random.Range (0, ArenaAreas.Length)];
		//Turn on area boundaries
		ArenaAreaPicked.SetActive (true);
		foreach (PlayerManager PM in gameManager.playerManagers) {
			PM.playerMovement.ToggleArenaPointer (true, ArenaAreaPicked);
		}

		//Tell The player to move there GUI wise
		arenaCanvas.NextArea(ArenaAreaPicked.GetComponent<ArenaArea> ().AreaName);
		//Allow the player in the area by turning off colliders
		BoxCollider[] OuterWallColliders = ArenaAreaPicked.GetComponentsInChildren<BoxCollider> ();
		foreach (BoxCollider OuterWallCollider in OuterWallColliders){
			OuterWallCollider.enabled = false;
			//Debug.Log ("Outerwalls open");
		}
		//check for player being there
		while (!ArenaAreaPicked.GetComponent<ArenaArea>().playerinarea) {
			//Debug.Log ("In while loop Are You in "+ ArenaAreaPicked.GetComponent<ArenaArea>().AreaName +" "+ArenaAreaPicked.GetComponent<ArenaArea>().playerinarea);
			yield return null;
		}
		//Debug.Log ("out of while loop ArenaAreaPicked.GetComponent<ArenaArea>().playerinarea = "+ArenaAreaPicked.GetComponent<ArenaArea>().playerinarea);

		foreach (PlayerManager PM in gameManager.playerManagers) {
			PM.playerMovement.ToggleArenaPointer (false, null);
		}

		//Turn on the colliders locking in the player
		foreach (BoxCollider OuterWallCollider in OuterWallColliders){
			OuterWallCollider.enabled = true;
			//Debug.Log ("Outerwalls closed");
		}
		//Find Wave Data
		Enemy[] enemiesToSpawnNextWave;
		if (waveNumber<2){
			enemiesToSpawnNextWave = new Enemy[]{enemiesToSpawn[0]};
		}else if (waveNumber<3){
			enemiesToSpawnNextWave = new Enemy[]{enemiesToSpawn[0],enemiesToSpawn[1]};
		}else if (waveNumber<4){
			enemiesToSpawnNextWave = new Enemy[]{enemiesToSpawn[0],enemiesToSpawn[1],enemiesToSpawn[2]};
		} else {
			enemiesToSpawnNextWave = enemiesToSpawn;
		}
		int enemiesAmountNextWave = waveNumber * 2;
		//start wave
		StartCoroutine(SpawnWave (enemiesAmountNextWave,enemiesToSpawnNextWave));
	}
}
