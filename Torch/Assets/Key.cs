using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Key : MonoBehaviour {

	private GameManager gameManager;
	[SerializeField] private AudioSource audioSource;
	public Collider collideR;

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

			audioSource.Play ();

			GetComponent<MeshRenderer> ().enabled = false;
			collideR.enabled = false;

			Destroy (transform.parent.gameObject);
		}
	}
}
