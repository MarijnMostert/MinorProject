using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoresPanel : MonoBehaviour {

	private GameManager gameManager;
	private Data data;
	public GameObject panel;
	public Text indexText;
	public Text namesText;
	public Text scoresText;

	void Start(){
		panel.SetActive (false);
	}

	void OnEnable(){
		UpdateHighScores ();
	}

	void UpdateHighScores(){
		if (gameManager == null) {
			gameManager = GameManager.Instance;
			data = gameManager.data;
		}

		string indexTemp = "";
		string namesTemp = "";
		string scoresTemp = "";
		for (int i = 0; i < data.highScoreNames.Count; i++) {
			indexTemp += (i+1) + "\n";
			namesTemp += data.highScoreNames [i] + "\n";
			scoresTemp += data.highScoreScores [i] + "\n";
		}

		indexText.text = indexTemp;
		namesText.text = namesTemp;
		scoresText.text = scoresTemp;
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			panel.SetActive (true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			panel.SetActive (false);
		}
	}
}
