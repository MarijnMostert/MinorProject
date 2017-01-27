using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Data : MonoBehaviour {

	[SerializeField] private GameManager gameManager;

	[Header ("- Saved Data")]
	public bool[] shopItemsOwned;
	public bool[] shopItemsEquipped;

	public bool[] achievementsGotten;

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
        public int id;
        public int score;
        public string name;
        public string date;
    }

    [Serializable]
    public struct WWWHighscores
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
    public GameObject loginCanvas;

    [Serializable]
    public class Score
    {
        public string id;
        public string date;
        public Score() { }
        public Score(string id,string date)
        {
            this.id = id;
            this.date = date;
        }
    }

    public int max_score;

    void Start(){
        if (highscores == null)
        {
            Debug.Log("UPDATE HIGHSCORESSSS");
            highscores = new Highscores();
            highscores.highscore = new List<Highscore>();
        }
		playerSkin = new Material[4];
        Debug.Log(PlayerPrefs.GetInt("id"));
        if (!PlayerPrefs.HasKey("id"))
        {
            loginCanvas.SetActive(true);
			GameManager.Instance.SetTextFieldEnabled (true);
        }
        max_score = 0;
	}

	/*
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			Save ();
		}
	}
	*/


	public void Load(){
		shopItemsOwned = new bool[50];
		shopItemsEquipped = new bool[50];
		achievementsGotten = new bool[50];

		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			string strEquipped = "shopItemEquipped" + i;
			shopItemsOwned [i] = intToBool(PlayerPrefs.GetInt (strOwned));
			shopItemsEquipped[i] = intToBool(PlayerPrefs.GetInt (strEquipped));
		}

		for (int i = 0; i < achievementsGotten.Length; i++) {
			string strAch = "achievement" + i;
			achievementsGotten [i] = intToBool(PlayerPrefs.GetInt(strAch));
		}

		this.coins = PlayerPrefs.GetInt ("coins");
		this.maxAchievedDungeonLevel = PlayerPrefs.GetInt ("dungeonLevel");
		if (maxAchievedDungeonLevel == 0)
			maxAchievedDungeonLevel = 1;
		this.highQuality = intToBool(PlayerPrefs.GetInt ("highQuality"));
        highscores = new Highscores();
        string jsonDataString = Serializer.Load<string>("highScores.txt");
        highscores = JsonUtility.FromJson<Highscores>(jsonDataString);
        max_score = getMaxScore();

        if (PlayerPrefs.HasKey("id"))
        {
            server_communication();
        }
		Debug.Log ("Loaded data succesfully");
	}

	public void Save(){
        string bitItemsOwned = "";
        string bitItemsEquiped = "";
        string bitAchievements = "";

		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
            bitItemsOwned += boolToInt(shopItemsOwned[i]).ToString();
			PlayerPrefs.SetInt (strOwned, boolToInt (shopItemsOwned [i]));
			string strEquipped = "shopItemEquipped" + i;
            bitItemsEquiped += boolToInt(shopItemsEquipped[i]).ToString();
            PlayerPrefs.SetInt (strEquipped, boolToInt (shopItemsEquipped [i]));
		}

		for (int i = 0; i < achievementsGotten.Length; i++) {
			string strAch = "achievement" + i;
            bitAchievements += boolToInt(achievementsGotten[i]).ToString();
			PlayerPrefs.SetInt (strAch, boolToInt (achievementsGotten [i]));
		}

        Debug.Log("---SAVESTRING---");
        Debug.Log(bitItemsOwned);
        Debug.Log(bitItemsEquiped);
        Debug.Log(bitAchievements);

        PlayerPrefs.SetInt ("coins", coins);
		PlayerPrefs.SetInt ("dungeonLevel", maxAchievedDungeonLevel);
		PlayerPrefs.SetInt ("highQuality", boolToInt(highQuality));
		Serializer.Save<string> ("highScores.txt", JsonUtility.ToJson(highscores));
        WWWForm form = new WWWForm();
        Debug.Log("id: "+PlayerPrefs.GetInt("id")+", coins: "+coins+", level: "+maxAchievedDungeonLevel);

        form.AddField("id",PlayerPrefs.GetInt("id"));
        form.AddField("coins", coins);
        form.AddField("level", maxAchievedDungeonLevel);
        form.AddField("itemsOwned", bitItemsOwned);
        form.AddField("itemsEquiped", bitItemsEquiped);
        form.AddField("achievements", bitAchievements);
        WWW wwwData = new WWW("https://insyprojects.ewi.tudelft.nl/ewi3620tu1/unity/update.php",form);
        Debug.Log ("Saved data succesfully");
	}

    public void ResetData(){
		shopItemsOwned = new bool[50];
		shopItemsEquipped = new bool[50];
		achievementsGotten = new bool[50];

		for (int i = 0; i < shopItemsOwned.Length; i++) {
			string strOwned = "shopItem" + i;
			PlayerPrefs.SetInt (strOwned, 0);
			string strEquipped = "shopItemEquipped" + i;
			PlayerPrefs.SetInt (strEquipped, 0);
		}

		PlayerPrefs.SetInt ("shopItem5", 1);
		PlayerPrefs.SetInt ("shopItem8", 1);
		PlayerPrefs.SetInt ("shopItemEquipped5", 1);
		PlayerPrefs.SetInt ("shopItemEquipped8", 1);

			
		for (int i = 0; i < achievementsGotten.Length; i++) {
			string strAch = "achievement" + i;
			PlayerPrefs.SetInt (strAch, 0);
			achievementsGotten [i] = false;
		}

		PlayerPrefs.SetInt ("coins", 0);
		PlayerPrefs.SetInt ("dungeonLevel", 1);
		PlayerPrefs.SetInt ("highQuality", 1);
		gameManager.achievements.resetAchievements ();

        highscores.highscore.Clear();
        File.Delete("highScores.txt");
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

	public void SaveHighScore(int score){
        instantiateHighscores();
        StartCoroutine(addScoreWWW(score));
	}

    void server_communication()
    {
        StartCoroutine(getWWWData());
    }

    IEnumerator getWWWData()
    {
        /**
         * get highscores! 
         */
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("player_id", PlayerPrefs.GetInt("id"));
        wwwform.AddField("score_id", max_score);
        WWW website = new WWW("https://insyprojects.ewi.tudelft.nl/ewi3620tu1/unity/scores.php", wwwform);

        yield return website;
        if (highscores == null)
        {
            Debug.Log("UPDATE HIGHSCORESSSS");
            highscores = new Highscores();
            highscores.highscore = new List<Highscore>();
        }
        WWWHighscores wwwScore;
        if (website.error == null)
        {
            Debug.Log("WWW Ok!: " + website.text);
            wwwScore = JsonUtility.FromJson<WWWHighscores>(website.text);
            Debug.Log(wwwScore);
            Debug.Log(wwwScore.Highscore.Length);

            foreach (Highscore wwwtmp in wwwScore.Highscore)
            {
                DateTime date = DateTime.Now;
                Highscore tmp = new Highscore();
                tmp.id = wwwtmp.id;
                tmp.name = wwwtmp.name;
                tmp.score = wwwtmp.score;
                DateTime tmpdate;
                if (DateTime.TryParse(wwwtmp.date, out tmpdate))
                {
                    tmp.date = tmpdate.Day + "-" + tmpdate.Month + "-" + tmpdate.Year + " " + tmpdate.Hour + ":" + tmpdate.Minute;
                    highscores.highscore.Add(tmp);
                }else
                {
                    Debug.Log("Error parsing time");
                }
            }
        }
        else
        {
            Debug.Log("WWW Error: " + website.error);
        }

        GameManager.Instance.HighScoresPanel.GetComponentInChildren<HighScoresPanel>().UpdateHighScores();
        Save();
    }

    void instantiateHighscores()
    {            
        if(highscores.highscore == null)
        {
            highscores.highscore = new List<Highscore>();
        }
    }

    IEnumerator addScoreWWW(int score)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id",PlayerPrefs.GetInt("id"));
        wwwForm.AddField("score",score);
        DateTime now = DateTime.Now;
        wwwForm.AddField("date",now.ToString());
        WWW wwwData = new WWW("https://insyprojects.ewi.tudelft.nl/ewi3620tu1/unity/addscore.php",wwwForm);
        yield return wwwData;

        Debug.Log(wwwData.text);
        Score wwwscore = JsonUtility.FromJson<Score>(wwwData.text);
        Debug.Log("SCORE: "+ wwwscore.id);
        Debug.Log("DATE: " + wwwscore.date);

        Highscore new_score = new Highscore();
        new_score.id = Int32.Parse(wwwscore.id);
        new_score.name = PlayerPrefs.GetString("name");
        new_score.score = score;

        DateTime tmpdate;
        if (DateTime.TryParse(wwwscore.date, out tmpdate))
        {
            new_score.date = tmpdate.Day + "-" + tmpdate.Month + "-" + tmpdate.Year + " " + tmpdate.Hour + ":" + tmpdate.Minute;
            highscores.highscore.Add(new_score);
            Debug.Log("New highscore added: " + score + " : " + name);
        }

        GameManager.Instance.HighScoresPanel.GetComponentInChildren<HighScoresPanel>().UpdateHighScores();
    }

    public int getMaxScore()
    {
        int max= 0;
        if (highscores!=null) {
            foreach (Highscore tmp in highscores.highscore)
            {
                if (tmp.id > max)
                {
                    max = tmp.id;
                }
            }
        }
        return max;
    }

    public void logout()
    {
        clearPersonalData();
    }

    public void clearPersonalData()
    {
        Save();
        PlayerPrefs.DeleteAll();
        shopItemsOwned = new bool[50];
        shopItemsEquipped = new bool[50];
        achievementsGotten = new bool[50];

        for (int i = 0; i < shopItemsOwned.Length; i++)
        {
            string strOwned = "shopItem" + i;
            PlayerPrefs.SetInt(strOwned, 0);
            string strEquipped = "shopItemEquipped" + i;
            PlayerPrefs.SetInt(strEquipped, 0);
        }

        PlayerPrefs.SetInt("shopItem5", 1);
        PlayerPrefs.SetInt("shopItem8", 1);
        PlayerPrefs.SetInt("shopItemEquipped5", 1);
        PlayerPrefs.SetInt("shopItemEquipped8", 1);


        for (int i = 0; i < achievementsGotten.Length; i++)
        {
            string strAch = "achievement" + i;
            PlayerPrefs.SetInt(strAch, 0);
            achievementsGotten[i] = false;
        }

        PlayerPrefs.SetInt("coins", 0);
        PlayerPrefs.SetInt("dungeonLevel", 1);
        PlayerPrefs.SetInt("highQuality", 1);
        gameManager.achievements.resetAchievements();

        coins = 0;
        maxAchievedDungeonLevel = 1;
        highQuality = true;

        highscores.highscore.Clear();
        File.Delete("highScores.txt");
        Debug.Log("lOGOUT succesfully");
        Application.Quit();
    }
}
