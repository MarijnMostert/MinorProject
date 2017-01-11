using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ColliderStartGame : MonoBehaviour {

	public GameObject textObject;

	private string interactionButton = "InteractionButton";
	private GameManager gameManager;

	void Awake(){
		gameManager = GameManager.Instance;
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			textObject.SetActive (true);
			if (Input.GetButtonDown (interactionButton) && !gameManager.dungeonStartCanvas.activeInHierarchy) {
				gameManager.ToggleDungeonStartCanvas ();
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}
}
