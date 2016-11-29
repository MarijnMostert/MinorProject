using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public int mapMinX, mapMaxX;
	public int mapMinZ, mapMaxZ;

	public Enemy[] enemiesToSpawn;
	public int enemiesPerWave;

	private string waveButton;
	private string spawnEnemyButton;
//	private int rangeX, rangeZ;

	// Use this for initialization
	void Start () {
		waveButton = "Spawn Wave Button";
		spawnEnemyButton = "Spawn Enemy Button";
//		rangeX = mapMaxX - mapMinX;
//		rangeZ = mapMaxZ - mapMinZ;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (spawnEnemyButton)) {
			spawnEnemy ();
		}
		if (Input.GetButtonDown (waveButton)) {
			spawnWave ();
		}
	}

	void spawnEnemy(){
		Enemy enemy = enemiesToSpawn [Random.Range (0, enemiesToSpawn.Length)];
		float x = (float) Random.Range (mapMinX, mapMaxX);
		float z = (float) Random.Range (mapMinZ, mapMaxZ);
		Instantiate (enemy, new Vector3 (x, 1f, z), transform.rotation);
	}

	void spawnWave(){
		for (int i = 0; i < enemiesPerWave; i++) {
			spawnEnemy ();
		}
	}

	public void spawnTutorial(){
		spawnWave ();
	}
}
