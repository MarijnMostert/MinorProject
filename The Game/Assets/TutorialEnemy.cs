using UnityEngine;
using System.Collections;

public class TutorialEnemy : MonoBehaviour {

	public GameObject spawner;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			spawner.GetComponent<Spawner> ().spawnTutorial ();
		}
	}
}
