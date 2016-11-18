using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	public Text scoreOnDeathText;

	private int score;

	void Start () {
		score = 0;
		updateScoreText ();
	}

	//Updates the score when called
	public void updateScore(int addedScore){
		score += addedScore;
		updateScoreText ();
	}

	public void updateScoreText(){
		scoreText.text = "Score: " + score;
		scoreOnDeathText.text = "Your score: " + score;
	}
}
