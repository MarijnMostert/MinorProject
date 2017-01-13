using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ColliderStartGame : MonoBehaviour {

	public GameObject textObject;

	private string interactionButton = "InteractionButton";
	[SerializeField] private GameManager gameManager;

	void Start(){
		gameManager = GameManager.Instance;
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			textObject.SetActive (true);
			if (Input.GetButtonDown (interactionButton) && !gameManager.dungeonStartCanvas.gameObject.activeInHierarchy) {
				gameManager.ToggleDungeonStartCanvas ();
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}

	void OnDisable(){
		textObject.SetActive (false);
	}
}
