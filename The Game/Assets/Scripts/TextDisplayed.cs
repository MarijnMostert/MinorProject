using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextDisplayed : MonoBehaviour {

	public string message;
	public GameObject messageFrame;

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			messageFrame.GetComponent<Text> ().text = message;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			messageFrame.GetComponent<Text> ().text = "";
		}
	}

}
