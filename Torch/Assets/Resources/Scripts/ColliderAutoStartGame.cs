using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ColliderAutoStartGame : MonoBehaviour {

	private GameManager gameManager;

	void Awake(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			gameManager.StartGame ();
		}
	}
}