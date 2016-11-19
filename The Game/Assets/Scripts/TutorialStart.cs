using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialStart : MonoBehaviour {

	public GameObject textObject;

	void Start(){
		//textObject = Instantiate (textObject, transform.position, transform.rotation) as GameObject;
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			textObject.SetActive (true);
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			textObject.SetActive (false);
		}
	}
}
