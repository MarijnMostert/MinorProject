using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class endPortal : MonoBehaviour {
    public string next_scene;
    bool toHome;
    [SerializeField]
    bool loading, start_game;
    [SerializeField]
    string scene;
    AsyncOperation async;
    GameObject continueText;
	GameManager gameManager;
	GameObject endOfRoundCanvas;
    //Text winText;

    // Use this for initialization
    void Start () {
        loading = false;
        start_game = false;
        continueText = GameObject.Find("ContinueText") as GameObject;
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		endOfRoundCanvas = gameManager.endOfRoundCanvas;

        //winText = GameObject.Find("UI").transform.FindChild("Win Text").GetComponent<Text>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("endportal");
			onWin ();
        }
    }

    void onWin()
    {
		Time.timeScale = 0f;
		gameManager.totalScore += gameManager.score;
		endOfRoundCanvas.transform.Find ("Score").GetComponent<Text> ().text = gameManager.score.ToString();
		endOfRoundCanvas.transform.Find ("TotalScore").GetComponent<Text> ().text = gameManager.totalScore.ToString();
		endOfRoundCanvas.SetActive (true);
		gameManager.score = 0;

		/*
        //winText.gameObject.SetActive(true);
        GameObject.Find("Spawner").GetComponent<Spawner>().dead = true;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            enemy.GetComponent<Enemy>().Die();
        }
        Torch torch = GameObject.Find("Torch").GetComponent<Torch>();
        toHome = true;
		*/
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(1);
        async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
