using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathCanvas : MonoBehaviour {

	public Text scoreText;

	public void SetScoreText(int score){
		scoreText.text = "Your score: " + score;
	}
}
