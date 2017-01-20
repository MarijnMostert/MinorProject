using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class createPlayerForm : MonoBehaviour {
    public InputField name;
    public InputField password;
    public GameObject error;

    public class communication{
        public string succes;
        public string message;
        
        public communication() {}

        public communication(string succes,string message)
        {
            this.succes = succes;
            this.message = message;
        }
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void CreateUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("name",name.text);
        form.AddField("password", password.text);
        WWW login = new WWW("https://insyprojects.ewi.tudelft.nl/ewi3620tu1/unity/createuser.php",form);

        StartCoroutine(communicate(login));
    }

    IEnumerator communicate(WWW login)
    {
        yield return login;

        if (login.error == null)
        {
            Debug.Log("WWW Ok!: " + login.text);
            communication wwwData = JsonUtility.FromJson<communication>(login.text);
            Debug.Log(wwwData);
            Debug.Log("succes: "+ wwwData.succes);
            Debug.Log("message: "+ wwwData.message);
            if(wwwData.succes.Equals("1")){
                PlayerPrefs.SetString("name",name.text);
                PlayerPrefs.SetString("password",password.text);
                PlayerPrefs.SetInt("id", int.Parse(wwwData.message));
                Time.timeScale = 1;
                GameObject highscore = GameManager.Instance.HighScoresPanel;
                highscore.GetComponentInChildren<HighScoresPanel>().UpdateHighScores();
                transform.parent.gameObject.SetActive(false);
            } else
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
}
