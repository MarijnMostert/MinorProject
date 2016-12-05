using UnityEngine;
using System.Collections;

public class ScorePickUp : MonoBehaviour, IPickUp {

	public int scoreValue = 10;
    public GameObject gameManager;

	//Update the score with value
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
            gameManager.GetComponent<GameManager>().updateScore(scoreValue);
//            Debug.Log ("Score increased with " + scoreValue + " by picking up " + gameObject);
			Destroy (gameObject);
		}
	}

    public void setGameManager(GameObject gameManager)
    {
        this.gameManager = gameManager;
    }
}
