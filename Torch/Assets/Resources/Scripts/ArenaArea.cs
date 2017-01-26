using UnityEngine;
using System.Collections;

public class ArenaArea : MonoBehaviour {

	public GameManager gameManager;
	public bool playerinarea = false;
	public string AreaName = "Default Area Name";
	// Use this for initialization
	void Start () {
		gameManager = GameManager.Instance;
	}
	
	void OnTriggerEnter (Collider other) {
		gameManager.RespawnPosition = transform.position;
	}

	void OnTriggerStay (Collider other) {
		if (other.CompareTag ("Player")) {
			playerinarea = true;
		}
	}
	
	void OnDisable (){
		playerinarea = false;
	}
}
