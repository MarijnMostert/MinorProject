using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathCanvas : MonoBehaviour {

	private bool cheatMemory;
	public Text scoreText;
	public Text coinText;
	public Text InputField;
	public GameObject Input;
	public GameObject SubmitButton;
	public GameObject Confirmation;

	public void OnEnable(){
		Reset ();
		cheatMemory = GameManager.Instance.getCheat ();
		if (cheatMemory) {
			GameManager.Instance.toggleCheat ();
		}
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
		string name = InputField.text;
		int score = GameManager.Instance.totalScore;
		if (name != null && score != null) {
			GameManager.Instance.data.SaveHighScore (score, name);
		}
	}

	void Reset(){
		Input.SetActive (true);
		SubmitButton.SetActive (true);
		Confirmation.SetActive (false);
	}
}
