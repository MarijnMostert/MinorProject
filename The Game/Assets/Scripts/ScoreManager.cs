using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	public Text scoreOnDeathText;

	private int score;
	private PlayerPrefsManager playerPrefsManager;

	void Awake() {
		playerPrefsManager = GameObject.Find ("SceneManager").GetComponent<PlayerPrefsManager> ();
	}

	void Start () {
		score = 0;
		updateScoreText ();
	}

	//Updates the score when called
	public void updateScore(int addedScore){
		score += addedScore;
		updateScoreText ();
		updateHighScore ();
	}

	public void updateScoreText(){
		scoreText.text = "Score: " + score;
		scoreOnDeathText.text = "Your score: " + score;
	}

	private void updateHighScore(){
		if (score > playerPrefsManager.highscore) {
			playerPrefsManager.highscore = score;
		}
	}
}
