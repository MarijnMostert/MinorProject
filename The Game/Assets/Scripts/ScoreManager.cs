using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	public Text scoreOnDeathText;

	private int score;
	private PlayerPrefsManager ppM;

	void Awake() {
		scoreText = GameObject.Find ("Score Text").GetComponent<Text> ();
		if (scoreText == null)
			Debug.Log ("Score text is not found. Add UI Prefab to the scene.");
		scoreOnDeathText = GameObject.Find ("UI").transform.FindChild ("Death Text").FindChild("Your Score Text").GetComponent<Text> ();
		if (scoreOnDeathText == null)
			Debug.Log ("Score on death text is not found. Add UI Prefab to the scene.");
		ppM = GameObject.Find ("SceneManager").GetComponent<PlayerPrefsManager> ();
		if (ppM == null)
			Debug.Log ("Player Preferences manager is not found. Add Scene Manager prefab to the scene.");
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
		if (score > ppM.highscore) {
			ppM.highscore = score;
		}
	}
}
