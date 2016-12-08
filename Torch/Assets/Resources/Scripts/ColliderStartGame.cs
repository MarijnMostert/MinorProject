using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ColliderStartGame : MonoBehaviour {

	public GameObject textObject;

	private string interactionButton = "InteractionButton";
	private GameManager gameManager;

	void Awake(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			textObject.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				gameManager.StartGame ();
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}
}
