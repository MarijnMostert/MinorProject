using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathCanvas : MonoBehaviour {

	private bool cheatMemory;
	public Text scoreText;
	public Text coinText;
	public Text cause;
	public GameObject HighScoreSubmitText;
	public GameObject HighScoreDeniedText;

	public void OnEnable(){

		Debug.Log ("Cheats are " + GameManager.Instance.getCheat ());
		HighScoreSubmitText.SetActive (!GameManager.Instance.getCheat ());
		HighScoreDeniedText.SetActive (GameManager.Instance.getCheat ());

		if (!GameManager.Instance.getCheat())
			SubmitHighScore ();
	}

	public void SetScoreText(int score){
		scoreText.text = "Your score: " + score;
	}

	public void SetCoinText(int coins){
		coinText.text = "Collected coins: " + coins;
	}

	public void SetCauseText(string cause){
		this.cause.text = cause;
	}

	public void SubmitHighScore(){
		int score = GameManager.Instance.totalScore;
	    GameManager.Instance.data.SaveHighScore (score);
	}
}
