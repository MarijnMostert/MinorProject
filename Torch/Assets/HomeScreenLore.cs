using UnityEngine;
using System.Collections;

public class HomeScreenLore : MonoBehaviour {

	AudioClip remember;
	public GameObject ActivateOnNear;
	public GameObject ActivateOnPlay;
	public GameObject ActivateOnStop;
	private string interactionButton = "InteractionButton";
	[HideInInspector] private GameManager gameManager;

	void Start () {
		remember = GameManager.Instance.audioSourceMusic.clip;
		gameManager = GameManager.Instance;

		ActivateOnPlay.SetActive (false);
		ActivateOnStop.SetActive (true);
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			ActivateOnNear.SetActive (true);
			if (Input.GetButtonDown (interactionButton) && !gameManager.GetTextFieldEnabled()) {
				StartLore ();
				ActivateOnPlay.SetActive (true);
				ActivateOnStop.SetActive (false);
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			ActivateOnNear.SetActive (false);
			ActivateOnPlay.SetActive (false);
			ActivateOnStop.SetActive (true);
			EndLore ();
		}
	}

	void StartLore () {
		gameManager.audioSourceMusic.clip = gameManager.loreAudio;
		gameManager.audioSourceMusic.Play();
	}

	void EndLore () {
		if (gameManager.audioSourceMusic.clip != gameManager.audioHomeScreen) {
			gameManager.audioSourceMusic.clip = gameManager.audioHomeScreen;
			gameManager.audioSourceMusic.Play ();
		}
	}
}
