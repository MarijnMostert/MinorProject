using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class loginForm : MonoBehaviour
{
    public InputField name;
    public InputField password;
    public GameObject error;

    public class communication
    {
        public string succes;
        public string message;
        public int coins;
        public int level;
        public string items_equiped;
        public string items_owned;
        public string achievements;

        public communication() { }

        public communication(string succes, string message, int coins, int level, string items_equiped, string items_owned, string achievements)
        {
            this.succes = succes;
            this.message = message;
            this.coins = coins;
            this.level = level;
            this.items_equiped = items_equiped;
            this.items_owned = items_owned;
            this.achievements = achievements;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
       // Time.timeScale = 0;
		GameManager.Instance.homeScreenMovement.enabled = false;
    }

    public void login()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name.text);
        form.AddField("password", password.text);
        WWW login = new WWW("https://insyprojects.ewi.tudelft.nl/ewi3620tu1/unity/tmplogin.php", form);

        StartCoroutine(communicate(login));
    }

    IEnumerator communicate(WWW login)
    {
        yield return login;

        if (login.error == null)
        {
            Debug.Log("WWW Ok!: " + login.text);
            communication wwwData = JsonUtility.FromJson<communication>(login.text);
            if (wwwData.succes.Equals("1"))
            {
                PlayerPrefs.SetString("name", name.text);
                PlayerPrefs.SetString("password", password.text);
                PlayerPrefs.SetInt("id", int.Parse(wwwData.message));
                PlayerPrefs.SetInt("coins",wwwData.coins);
                PlayerPrefs.SetInt("dungeonLevel",wwwData.level);


                Debug.Log("---LOADSTRING---");
                Debug.Log(wwwData.items_owned);
                Debug.Log(wwwData.items_equiped);
                Debug.Log(wwwData.achievements);

                char[] itemsOwned = wwwData.items_owned.ToCharArray();
                char[] itemsEquiped = wwwData.items_equiped.ToCharArray();
                char[] achievements = wwwData.achievements.ToCharArray();
                for (int i = 0; i < itemsOwned.Length; i++)
                {
                    string strOwned = "shopItem" + i;
                    PlayerPrefs.SetInt(strOwned, charToInt(itemsOwned[i]));
                    string strEquipped = "shopItemEquipped" + i;
                    PlayerPrefs.SetInt(strEquipped, charToInt(itemsEquiped[i]));
                }

                for (int i = 0; i < achievements.Length; i++)
                {
                    string strAch = "achievement" + i;
                    PlayerPrefs.SetInt(strAch, charToInt(achievements[i]));
                }

                GameManager.Instance.SetDungeonLevel(wwwData.level);
                GameManager.Instance.data.maxAchievedDungeonLevel = wwwData.level;
                GameManager.Instance.dungeonStartCanvas.buttons.Clear();
                GameManager.Instance.SetUpDungeonStartCanvas();
				HomeScreenProgress.Instance.UpdateProgress (GameManager.Instance.data.maxAchievedDungeonLevel);

                Time.timeScale = 1;
                GameManager.Instance.data.Load();
                transform.parent.gameObject.SetActive(false);
				GameManager.Instance.SetTextFieldEnabled (false);
				GameManager.Instance.homeScreenMovement.enabled = true;
            }
            else
            {
                StartCoroutine(showError(wwwData.message));
            }
        }
        else
        {
            Debug.Log("WWW Error: " + login.error);
        }
    }

    IEnumerator showError(string message)
    {
        error.GetComponentInChildren<Text>().text = message;
        error.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        error.SetActive(false);
        yield return null;
    }

    int charToInt(char c)
    {
        if (c.Equals('1'))
        {
            return 1;
        }
        return 0;
    }
}
