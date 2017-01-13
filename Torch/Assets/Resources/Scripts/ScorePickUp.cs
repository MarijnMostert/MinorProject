using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class ScorePickUp : AudioObject, IPickUp {

	public int scoreValue = 10;
	public AudioClip clip;
	private GameManager gameManager;

	void Start(){
		gameManager = GameManager.Instance;
	}

	//Update the score with value
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			ObjectPooler.Instance.PlayAudioSource (clip, mixerGroup, pitchMin, pitchMax, transform);

			gameManager.updateScore (scoreValue);
			other.GetComponent<PlayerData> ().IncrementScorePickedUp (scoreValue);

			//Debug.Log ("Score increased with " + scoreValue + " by picking up " + gameObject);
			transform.parent.GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<Collider> ().enabled = false;
			Destroy (transform.parent.gameObject, 3f);
		}
	}
}
