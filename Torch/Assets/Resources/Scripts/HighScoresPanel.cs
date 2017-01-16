using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class HighScoresPanel : MonoBehaviour {

	private GameManager gameManager;
	private Data data;
	public GameObject panel;
	public Text indexText;
	public Text namesText;
	public Text scoresText;
	public List<HighScore> highScores;

	public class HighScore : IComparable<HighScore>{
		public string name;
		public int score;

		public HighScore(string name, int score){
			this.name = name;
			this.score = score;
		}

		public int CompareTo(HighScore other){
			if (other == null) {
				return 1;
			}

			return other.score - this.score;
		}
	}

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

		highScores = new List<HighScore> ();

		for (int i = 0; i < data.highScoreNames.Count; i++) {
			highScores.Add (new HighScore (data.highScoreNames [i], data.highScoreScores [i]));
		}

		highScores.Sort ();

		string indexTemp = "";
		string namesTemp = "";
		string scoresTemp = "";
		for (int i = 0; i < highScores.Count; i++) {
			indexTemp += (i+1) + "\n";
			namesTemp += highScores [i].name + "\n";
			scoresTemp += highScores [i].score + "\n";
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
