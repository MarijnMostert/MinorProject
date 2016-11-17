using UnityEngine;
using System.Collections;

public class CoinPickUp : PickUp, IPickUp {

	public int value = 10;

	new void Start () {
		base.Start ();
	}
	
	new void Update () {
		base.Update ();
		base.rotateZ ();
	}

	//Update the score with value
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			ScoreManager scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager> ();
			scoreManager.updateScore (value);
			Destroy (gameObject);
		}
	}
}
