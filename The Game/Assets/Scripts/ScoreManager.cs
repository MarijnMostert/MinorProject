using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;

	private int score;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText.text = "Score: " + score;
	}

	// Update is called once per frame
	void Update () {
	}

	public void updateScore(int addedScore){
		score += addedScore;
		scoreText.text = "Score: " + score;
	}
}
