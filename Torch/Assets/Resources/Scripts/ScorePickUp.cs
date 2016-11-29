using UnityEngine;
using System.Collections;

public class ScorePickUp : MonoBehaviour, IPickUp {

	public int scoreValue = 10;

	//Update the score with value
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			GameObject.Find ("Game Manager").GetComponent<GameManager> ().UpdateScore (scoreValue);
			Debug.Log ("Score increased with " + scoreValue + " by picking up " + gameObject);
			Destroy (gameObject);
		}
	}
}
