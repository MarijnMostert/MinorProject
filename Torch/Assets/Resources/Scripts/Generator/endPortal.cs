using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class endPortal : InteractableItem {
    //AsyncOperation async;
	GameObject endOfRoundCanvas;
	public GameObject slider;
	public bool endPortalActivated = false;
	public Animator anim;
	public bool tutorial = false;
	Vector3 targetposition;
	Vector3 standardposition;

	void Update () {
		slider.transform.position = Vector3.MoveTowards (slider.transform.position, targetposition, 0.1f);
	}

    public override void Start () {
		if (gameManager == null) {
			gameManager = GameManager.Instance;
		}
		if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
			endPortalActivated = true;
		}
		foreach (PlayerManager PM in gameManager.playerManagers) {
			PM.playerMovement.ToggleArenaPointer (endPortalActivated, gameObject);
		}
		UpdateKeyText ();
		targetposition = slider.transform.position;
		standardposition = slider.transform.position;
    }


	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			if (!endPortalActivated) {
				if (anim == null) {
					anim = gameManager.ui.keysText.GetComponent<Animator> ();
				}
				anim.SetTrigger ("Flash");
				Debug.Log (gameManager.collectedKeys + " keys out of " + gameManager.requiredCollectedKeys +
				" keys are collected.\nYou need " + (gameManager.requiredCollectedKeys - gameManager.collectedKeys) + " more keys.");
			}
			if (endPortalActivated) {
				targetposition = standardposition - Vector3.up * 3;
			}
		}
    }

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			targetposition = standardposition;
		}
	}

	public override void action (GameObject triggerObject)
	{
		if (!endPortalActivated) {
			anim.SetTrigger ("Flash");
		} else {
			onWin ();
		}
	}

	public void UpdateKeyText ()
	{
//		Debug.Log (gameManager.collectedKeys + " keys out of " + gameManager.requiredCollectedKeys + " keys are collected.");
		Text keyText = gameManager.ui.keysText;
		keyText.text = "Keys collected: " + gameManager.collectedKeys + "/" + gameManager.requiredCollectedKeys;
		if (gameManager.collectedKeys == gameManager.requiredCollectedKeys) {
			Debug.Log ("Endportal is enabled.");
		}
	}

    void onWin()
    {
		foreach (PlayerManager PM in gameManager.playerManagers) {
			PM.playerMovement.ToggleArenaPointer (false, null);
		}
		Time.timeScale = 0f;
		if (gameManager.spawner != null) {
			gameManager.spawner.activated = false;
		}
		gameManager.Pet.GetComponent <Pet> ().speechImage.gameObject.SetActive (false);
		gameManager.Pet.GetComponent <Pet> ().speechText.text = "";		/*
		gameManager.endOfRoundCanvas.transform.Find ("Score").GetComponent<Text> ().text = gameManager.score.ToString();
		gameManager.endOfRoundCanvas.transform.Find ("TotalScore").GetComponent<Text> ().text = gameManager.totalScore.ToString();
		*/
		gameManager.endOfRoundCanvas.GetComponent<EndOfRoundCanvas> ().Fill ();
		gameManager.endOfRoundCanvas.GetComponent<EndOfRoundCanvas> ().amountOfPlayers(gameManager.numberOfPlayers);
		gameManager.endOfRoundCanvas.SetActive (true);

		if (tutorial) {
			gameManager.achievements.tutortialAchievement ();
		}

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

	/*

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
*/
}
