using UnityEngine;
using System.Collections;

public class BossFight : MonoBehaviour {

	public GameObject BossPrefab;
	private Vector3 spawnpoint;
	private GameObject BossObject;
	private GameManager gameManager;
	bool boss = false;

	// Use this for initialization
	void Start () {
		spawnpoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void OnTriggerEnter(Collider other){
		if (gameManager.dungeonLevel % 5 == 0) {
			if (boss == false) {
				if (other.gameObject.CompareTag ("Player")) {
					Debug.Log ("boss instantiated");
					boss = true;
					BossObject = Instantiate (BossPrefab);
					BossObject.transform.position = spawnpoint;
					BossObject.transform.parent = gameObject.transform;
				}
			}
		}
	}

}
