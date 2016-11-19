using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;

	private int score;

	void Start () {
		score = 0;
		scoreText.text = "Score: " + score;
	}

	//Updates the score when called
	public void updateScore(int addedScore){
		score += addedScore;
		scoreText.text = "Score: " + score;
	}
}
