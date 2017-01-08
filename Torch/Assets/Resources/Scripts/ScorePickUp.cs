using UnityEngine;
using System.Collections;

public class ScorePickUp : MonoBehaviour, IPickUp {

	public int scoreValue = 10;
	public float pitchMin = 0.8f;
	public float pitchMax = 1.2f;
	public AudioSource audioSource;

	private GameManager gameManager;

	void Start(){
		gameManager = GameManager.Instance;
	}

	//Update the score with value
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			audioSource.pitch = Random.Range (pitchMin, pitchMax);
			audioSource.Play ();

			gameManager.updateScore (scoreValue);

			//Debug.Log ("Score increased with " + scoreValue + " by picking up " + gameObject);
			transform.parent.GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<Collider> ().enabled = false;
			Destroy (transform.parent.gameObject, 3f);
		}
	}
}
