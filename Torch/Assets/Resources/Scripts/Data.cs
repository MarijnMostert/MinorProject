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

	[Header ("- Non-Saved Data")]
	public int playerMaxHealth = 100;
	public float playerDamageMultiplier = 1;
	public Material[] playerSkin;

    [Serializable]
    public struct Highscore
    {
        public string id;
        public int score;
        public string name;
        public DateTime date;
    }

    [Serializable]
    public struct WWWHighscore
    {
        public Highscore[] Highscore;
    }

    [Serializable]
    public class Highscores
    {
        public List<Highscore> highscore;
        public Highscores() { }
        public Highscores(List<Highscore> highscore)
        {
            this.highscore = highscore;
        }
    }

    public Highscores highscores;

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
        highscores = new Highscores();
        string jsonDataString = Serializer.Load<string>("highScores.txt");
        highscores = JsonUtility.FromJson<Highscores>(jsonDataString);

        server_communication();
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
		Serializer.Save<string> ("highScores.txt", JsonUtility.ToJson(highscores));

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
        instantiateHighscores();
        Highscore new_score = new Highscore();
        new_score.name = name;
        new_score.score = score;
        new_score.date = DateTime.Now;

        highscores.highscore.Add(new_score);
        Debug.Log ("New highscore added: " + score + " : " + name);

		GameManager.Instance.HighScoresPanel.GetComponentInChildren<HighScoresPanel> ().UpdateHighScores ();
	}

    void server_communication()
    {
        WWW website = new WWW("https://insyprojects.ewi.tudelft.nl/ewi3620tu1/unity/scores.php");
        StartCoroutine(getWWWData(website));
    }

    IEnumerator getWWWData(WWW website)
    {
        yield return website;

        if(website.error == null)
        {
            Debug.Log("WWW Ok!: " + website.text);
            WWWHighscore wwwData = JsonUtility.FromJson<WWWHighscore>(website.text);
            //Highscore[] wwwData = JsonHelper.getJsonArray<Highscore>(website.text);
            Debug.Log(wwwData);
            Debug.Log(wwwData.Highscore.Length);
        }
        else
        {
            Debug.Log("WWW Error: " + website.error);
        }
    }

    void instantiateHighscores()
    {            
        if(highscores.highscore == null)
        {
            highscores.highscore = new List<Highscore>();
        }
    }
}
