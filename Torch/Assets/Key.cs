using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Key : MonoBehaviour {

	GameManager gameManager;

	void Start(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			
			gameManager.collectedKeys++;
			Debug.Log ("Key number " + gameManager.collectedKeys + " out of " + gameManager.requiredCollectedKeys + " collected");

			endPortal EndPortal = GameObject.FindGameObjectWithTag ("EndPortal").GetComponentInChildren<endPortal> ();
			EndPortal.UpdateKeyText ();

			if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
				//End portal is enabled.
				EndPortal.enabled = true;
			}
			Destroy (gameObject);
		}
	}
}
