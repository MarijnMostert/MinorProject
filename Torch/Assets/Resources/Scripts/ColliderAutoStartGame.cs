using UnityEngine;
using System.Collections;

public class ColliderAutoStartGame : MonoBehaviour {
	[SerializeField] private GameManager gameManager;

	void Start(){
		gameManager = GameManager.Instance;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			if (!gameManager.dungeonStartCanvas.gameObject.activeInHierarchy) {
				gameManager.ToggleDungeonStartCanvas ();
			}
		}
	}

}
