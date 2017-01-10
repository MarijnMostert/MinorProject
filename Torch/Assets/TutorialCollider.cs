using UnityEngine;
using System.Collections;

public class TutorialCollider : MonoBehaviour {

	public GameObject textObject;

	private string interactionButton = "InteractionButton";
	private GameManager gameManager;

	void Awake(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")) {
			textObject.SetActive(true);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			if (Input.GetButtonDown (interactionButton)) {
				textObject.SetActive (false);
				gameManager.StartTutorial ();
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}
}
