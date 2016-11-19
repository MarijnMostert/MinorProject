using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStart : MonoBehaviour {

	public GameObject textObject;

	void Start(){
		//textObject = Instantiate (textObject, transform.position, transform.rotation) as GameObject;
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			textObject.SetActive (true);
			if (Input.GetButtonDown ("InteractionButton")) {
				SceneManager.LoadScene ("Scene 1");
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}
}
