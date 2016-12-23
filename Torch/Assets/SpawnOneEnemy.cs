using UnityEngine;
using System.Collections;

public class SpawnOneEnemy : MonoBehaviour {
	Spawner mySpawner;

	// Use this for initialization
	void Start () {
		mySpawner = GameObject.Find ("Game Manager").GetComponent<GameManager> ().spawner as Spawner;	
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			mySpawner.spawnEnemy ();
		}
	}
}