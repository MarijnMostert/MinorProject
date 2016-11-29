using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public int mapMinX, mapMaxX;
	public int mapMinZ, mapMaxZ;

	public Enemy[] enemiesToSpawn;
	public int enemiesPerWave;

	private string waveButton;
	private string spawnEnemyButton;

	void Start () {
		waveButton = "SpawnWave";
		spawnEnemyButton = "SpawnEnemy";
	}

	void Update(){
		if (Input.GetButtonDown (spawnEnemyButton)) {
			spawnEnemy ();
		}
		if (Input.GetButtonDown (waveButton)) {
			spawnWave ();
		}
	}

	public void spawnEnemy(){
		Enemy enemy = enemiesToSpawn [Random.Range (0, enemiesToSpawn.Length)];
		float x = (float) Random.Range (mapMinX, mapMaxX);
		float z = (float) Random.Range (mapMinZ, mapMaxZ);
		Instantiate (enemy, new Vector3 (x, 1f, z), transform.rotation);
	}

	public void spawnWave(){
		for (int i = 0; i < enemiesPerWave; i++) {
			spawnEnemy ();
		}
	}

}
