using UnityEngine;
using System.Collections;

public class SpawnOneEnemy : MonoBehaviour {

	public Enemy enemyToSpawn;
	public Transform spawnPoint;
	private bool enemySpawned = false;

	void OnTriggerEnter (Collider other) {
		if (!enemySpawned && other.gameObject.CompareTag ("Player")) {
			Instantiate (enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
			enemySpawned = true;
		}
	}
}