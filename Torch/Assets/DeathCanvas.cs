using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathCanvas : MonoBehaviour {

	private bool cheatMemory;
	public Text scoreText;
	public Text coinText;
	public GameObject Confirmation;

	public void OnEnable(){
		Reset ();
		cheatMemory = GameManager.Instance.getCheat ();
		if (cheatMemory) {
			GameManager.Instance.toggleCheat ();
		}
        SubmitHighScore();
	}

	public void OnDisable(){
		if (cheatMemory) {
			GameManager.Instance.toggleCheat ();
		}
	}

	public void SetScoreText(int score){
		scoreText.text = "Your score: " + score;
	}

	public void SetCoinText(int coins){
		coinText.text = "Collected coins: " + coins;
	}

	public void SubmitHighScore(){
		int score = GameManager.Instance.totalScore;
	    GameManager.Instance.data.SaveHighScore (score);
	}

	void Reset(){
		Confirmation.SetActive (false);
	}
}
