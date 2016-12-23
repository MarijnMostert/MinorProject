using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Key : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameManager gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
			gameManager.collectedKeys++;
			Debug.Log ("Key number " + gameManager.collectedKeys + " out of " + gameManager.requiredCollectedKeys + " collected");

			GameObject.FindGameObjectWithTag ("EndPortal").GetComponent<endPortal> ().UpdateKeyText ();

			if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
				//End portal is enabled.
				GameObject.FindGameObjectWithTag("EndPortal").GetComponentInChildren<endPortal>().enabled = true;
			}
			Destroy (gameObject);
		}
	}
}
