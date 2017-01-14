using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathCanvas : MonoBehaviour {

	public Text scoreText;
	public Text coinText;

	public void SetScoreText(int score){
		scoreText.text = "Your score: " + score;
	}

	public void SetCoinText(int coins){
		coinText.text = "Collected coins: " + coins;
	}
}
