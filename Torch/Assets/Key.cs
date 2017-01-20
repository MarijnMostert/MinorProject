using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Key : AudioObject {

	private GameManager gameManager;
	public AudioClip clip;
	public Collider collideR;

	void Start(){
		gameManager = GameManager.Instance;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			
			gameManager.collectedKeys++;
			gameManager.achievements.collectedKeys ();

			Debug.Log ("Key number " + gameManager.collectedKeys + " out of " + gameManager.requiredCollectedKeys + " collected");

			endPortal EndPortal = GameObject.FindGameObjectWithTag ("EndPortal").GetComponentInChildren<endPortal> ();
			EndPortal.UpdateKeyText ();

			if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
				//End portal is enabled.
				EndPortal.endPortalActivated = true;
				foreach (PlayerManager PM in gameManager.playerManagers) {
					PM.playerMovement.ToggleArenaPointer (true, EndPortal.gameObject);
				}
			}

			ObjectPooler.Instance.PlayAudioSource (clip, mixerGroup, 1f, 1f, transform);

			GetComponent<MeshRenderer> ().enabled = false;
			collideR.enabled = false;

			Destroy (transform.parent.gameObject);
		}
	}
}
