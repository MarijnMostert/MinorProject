﻿using UnityEngine;
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
	public bool enabled = false;
	public Animator anim;

    // Use this for initialization
    void Start () {
        loading = false;
        start_game = false;
        continueText = GameObject.Find("ContinueText") as GameObject;
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		anim = gameManager.UI.transform.Find ("Keys Text").GetComponent<Animator> ();
		endOfRoundCanvas = gameManager.endOfRoundCanvas;
		if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
			enabled = true;
		}
		UpdateKeyText ();

        //winText = GameObject.Find("UI").transform.FindChild("Win Text").GetComponent<Text>();
    }


    void OnTriggerEnter(Collider other)
    {
		if (enabled && other.gameObject.CompareTag ("Player")) {
			onWin ();
		} else {
			anim.SetTrigger ("Flash");
			Debug.Log (gameManager.collectedKeys + " keys out of " + gameManager.requiredCollectedKeys + 
				" keys are collected.\nYou need " + (gameManager.requiredCollectedKeys - gameManager.collectedKeys) + " more keys.");
		}
    }

	public void UpdateKeyText ()
	{
//		Debug.Log (gameManager.collectedKeys + " keys out of " + gameManager.requiredCollectedKeys + " keys are collected.");
		Text keyText = gameManager.UI.transform.Find ("Keys Text").GetComponent<Text> ();
		keyText.text = "Keys collected: " + gameManager.collectedKeys + "/" + gameManager.requiredCollectedKeys;
		if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
			Debug.Log ("Endportal is enabled.");
		}
	}

    void onWin()
    {
		Time.timeScale = 0f;
		if (gameManager.spawner != null) {
			gameManager.spawner.activated = false;
		}
		gameManager.Bold.GetComponent <Bold> ().speechImage.gameObject.SetActive (false);
		gameManager.Bold.GetComponent <Bold> ().speechText.text = "";
		gameManager.totalScore += gameManager.score;
		endOfRoundCanvas.transform.Find ("Score").GetComponent<Text> ().text = gameManager.score.ToString();
		endOfRoundCanvas.transform.Find ("TotalScore").GetComponent<Text> ().text = gameManager.totalScore.ToString();
		endOfRoundCanvas.SetActive (true);

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
