using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeScreenCollider : MonoBehaviour {

	public GameObject textObject;
	public string SceneToLoad;
	public string interactionButton = "InteractionButton";

	void Start(){

	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			textObject.SetActive (true);
			if (Input.GetButtonDown (interactionButton)) {
				SceneManager.LoadScene (SceneToLoad);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}
}
