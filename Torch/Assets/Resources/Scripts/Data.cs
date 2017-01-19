using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Data : MonoBehaviour {

	[SerializeField] private GameManager gameManager;

	[Header ("- Saved Data")]
	public bool[] shopItemsOwned;
	public bool[] shopItemsEquipped;
	public int coins;
	public int maxAchievedDungeonLevel;
	public bool highQuality;
	public List<string> highScoreNames;
	public List<int> highScoreScores;

	[Header ("- Non-Saved Data")]
	public int playerMaxHealth = 100;
	public float playerDamageMultiplier = 1;
	public Material[] playerSkin;

	void Start(){
		playerSkin = new Material[4];
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			Save ();
		}
	}

	public void Load(){
		shopItemsOwned = new bool[50];
		shopItemsEquipped = new bool[50];
		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			string strEquipped = "shopItemEquipped" + i;
			shopItemsOwned [i] = intToBool(PlayerPrefs.GetInt (strOwned));
			shopItemsEquipped[i] = intToBool(PlayerPrefs.GetInt (strEquipped));
		}
		this.coins = PlayerPrefs.GetInt ("coins");
		this.maxAchievedDungeonLevel = PlayerPrefs.GetInt ("dungeonLevel");
		if (maxAchievedDungeonLevel == 0)
			maxAchievedDungeonLevel = 1;
		this.highQuality = intToBool(PlayerPrefs.GetInt ("highQuality"));
		highScoreNames = new List<string> ();
		highScoreNames = Serializer.Load<List<string>> ("highScoresNames.txt");
		highScoreScores = new List<int> ();
		highScoreScores = Serializer.Load<List<int>> ("highScores.txt");

		Debug.Log ("Loaded data succesfully");
	}

	public void Save(){
		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			PlayerPrefs.SetInt (strOwned, boolToInt (shopItemsOwned [i]));
			string strEquipped = "shopItemEquipped" + i;
			PlayerPrefs.SetInt (strEquipped, boolToInt (shopItemsEquipped [i]));
		}
		PlayerPrefs.SetInt ("coins", coins);
		PlayerPrefs.SetInt ("dungeonLevel", maxAchievedDungeonLevel);
		PlayerPrefs.SetInt ("highQuality", boolToInt(highQuality));
		Serializer.Save<List<string>> ("highScoresNames.txt", highScoreNames);
		Serializer.Save<List<int>> ("highScores.txt", highScoreScores);

		Debug.Log ("Saved data succesfully");
	}

	public void ResetData(){
		shopItemsOwned = new bool[50];
		shopItemsEquipped = new bool[50];
		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			PlayerPrefs.SetInt (strOwned, 0);
			string strEquipped = "shopItemEquipped" + i;
			PlayerPrefs.SetInt (strEquipped, 0);
		}
		PlayerPrefs.SetInt ("coins", 0);
		PlayerPrefs.SetInt ("dungeonLevel", 1);
		PlayerPrefs.SetInt ("highQuality", 1);

		Debug.Log ("Reset data succesfully");
		Load ();
	}

	bool intToBool(int integer){
		if (integer == 0)
			return false;
		else
			return true;
	}

	int boolToInt(bool boolean){
		if(!boolean)
			return 0;
		else
			return 1;
	}

	void OnApplicationQuit(){
		Debug.Log ("Application quitted");
		Save ();
	}

	public void IncrementCoins(){
		coins += 1;
	}

	public void SaveHighScore(int score, string name){
		if (highScoreNames == null) {
			highScoreNames = new List<string> ();
		}
		highScoreNames.Add (name);
		if (highScoreScores == null) {
			highScoreScores = new List<int> ();
		}
		highScoreScores.Add (score);
		Debug.Log ("New highscore added: " + score + " : " + name);

		GameManager.Instance.HighScoresPanel.GetComponentInChildren<HighScoresPanel> ().UpdateHighScores ();
	}
}
