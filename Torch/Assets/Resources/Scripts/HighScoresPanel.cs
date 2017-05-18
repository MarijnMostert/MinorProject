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
    public Text dateText;
	public List<HighScore> highScores;

	public class HighScore : IComparable<HighScore>{
		public string name;
		public int score;
        public string date;

		public HighScore(string name, int score, string date){
			this.name = name;
			this.score = score;
            this.date = date;
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
        
            UpdateHighScores();
        
	}

	public void UpdateHighScores(){
		if (gameManager == null) {
			gameManager = GameManager.Instance;
			data = gameManager.data;
		}

		highScores = new List<HighScore> ();
        if (data.highscores != null || data.highscores.highscore != null) {
		    foreach (Data.Highscore tmp in data.highscores.highscore) {
			    highScores.Add (new HighScore (tmp.name, tmp.score, tmp.date));
		    }

		    highScores.Sort ();

		    string indexTemp = "";
		    string namesTemp = "";
		    string scoresTemp = "";
            string dateTemp = "";
		    for (int i = 0; i < highScores.Count; i++) {
			    indexTemp += (i+1) + "\n";
			    namesTemp += highScores [i].name + "\n";
			    scoresTemp += highScores [i].score + "\n";
                dateTemp += highScores[i].date + "\n";
		    }

		    indexText.text = indexTemp;
		    namesText.text = namesTemp;
		    scoresText.text = scoresTemp;
            dateText.text = dateTemp;
            Debug.Log("updated highscore");
        }
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
