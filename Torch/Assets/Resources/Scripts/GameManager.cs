using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Analytics;



public class GameManager : MonoBehaviour {

	//Player data
	public static GameManager Instance;
	public PlayerManager[] playerManagers;
	public GameObject playerPrefab;

	//Torch data
	public GameObject torchPrefab;
	public Torch torch;
	public Transform torchSpawnPoint;
	public int torchStartingHealth = 100;
	public int torchHealth;
	public int torchHealthMax = 150;

	//Score info
	public int score = 0;
	public int totalScore = 0;
	public int dungeonLevel = 0;

	public bool paused;
	public GameObject pauseScreen;
	public GameObject cameraPrefab;
	public GameObject camTarget;
	public GameObject enemyTarget;
	public GameObject UIPrefab;
	public GameObject UI;
	public GameObject TorchFOVPrefab;
	public GameObject TorchFOV;
    public Spawner spawner;

    //masterGenerator Vars
    int width = 20;// = 100;
    int height = 20;// = 90;
    int radius = 1;// = 2;
    int maxlength = 4;// = 3;
    int timeout = 6000;// = 200;
    int minAmountOfRooms = 2;// = 6;
    int maxAmountOfRooms = 5;// = 8;
    int chanceOfRoom = 20;// = 15;

	//public GameObject homeScreenCanvas;
	public GameObject loadingScreenCanvas;
	public GameObject deathCanvas;
	public GameObject endOfRoundCanvas;
	public GameObject homeScreen;
	public GameObject homeScreenCam;
    public Camera mainCamera;
	private Vector3 homeScreenPlayerPosition;
    MasterGenerator masterGenerator;
    bool gameStarted;

	public AudioSource audioSource;
	public AudioClip audioHomeScreen;
	public AudioClip audioDungeon;

    void Awake () {
        gameStarted = false;
		//Makes sure this object is not deleted when another scene is loaded.
		if (Instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			GameObject.DontDestroyOnLoad (this.gameObject);
			Instance = this;
		}
		//homeScreenCanvas = GameObject.Find ("Home Screen Canvas");
		homeScreen = GameObject.Find ("HomeScreen");
		homeScreenCam = GameObject.Find ("HomeScreenCam");
		audioSource = GetComponent<AudioSource> ();
    }

    public void Start(){
		pauseScreen = Instantiate (pauseScreen) as GameObject;
		pauseScreen.SetActive (false);
		loadingScreenCanvas = Instantiate (loadingScreenCanvas) as GameObject;
		loadingScreenCanvas.SetActive (false);
		homeScreenPlayerPosition = GameObject.Find ("HomeScreenPlayer").transform.position;
	}

	public void StartGame(){
        if (!gameStarted) {
			Time.timeScale = 1f;
			dungeonLevel++;
			endOfRoundCanvas.SetActive (false);
            loadingScreenCanvas.SetActive(true);
            StartCoroutine(CreateDungeon());
            gameStarted = true;
        }
	}

	IEnumerator CreateDungeon(){
		yield return new WaitForSeconds (.1f);
		masterGenerator = new MasterGenerator(this.gameObject, width, height, radius, maxlength, timeout, minAmountOfRooms, maxAmountOfRooms, chanceOfRoom);
		masterGenerator.LoadPrefabs();
		masterGenerator.Start();

		UI = Instantiate (UIPrefab);
		TorchFOV = Instantiate (TorchFOVPrefab);

		camTarget = torch.gameObject;
		enemyTarget = torch.gameObject;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.UI = UI;

		for (int i = 0; i < playerManagers.Length; i++) {
			Debug.Log("Create Player with id:" + i);
			playerManagers[i].playerInstance = Instantiate(playerPrefab, masterGenerator.dungeon_instantiate.startPos, playerManagers[i].spawnPoint.rotation) as GameObject;
			playerManagers [i].playerNumber = i + 1;
			playerManagers [i].Setup ();
			playerManagers [i].playerMovement.mainCamera = mainCamera;
		}

		torch.cam = mainCamera;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
		UI.transform.FindChild ("Dungeon Level").GetComponent<Text> ().text = "Dungeon level " + dungeonLevel;

		audioSource.clip = audioDungeon;
		audioSource.Play ();
		homeScreen.SetActive (false);
		homeScreenCam.SetActive (false);
		loadingScreenCanvas.SetActive (false);

		yield return null;
	}
	
	void Update () {
		Pause ();
		if(Input.GetKeyDown(KeyCode.I)){
			deathCanvas.SetActive (true);
		}
	}

	void Pause(){
		if (Input.GetButtonDown ("Pause")) {
			if (!paused) {
				Time.timeScale = 0;
				paused = true;
				pauseScreen.SetActive (true);
			} else {
				Time.timeScale = 1;
				paused = false;
				pauseScreen.SetActive (false);
			}
		}
	}

	/*
	void Initialize(){

	void SetUpCameraPart1(){
		cameraPrefab = Instantiate (cameraPrefab) as GameObject;
		mainCamera = cameraPrefab.GetComponentInChildren<Camera> ();
	}

	void SetUpCameraPart2(){
		cameraPrefab.GetComponentInChildren<CameraController> ().target = camTarget;
	}*/


	public void GameOver(){

		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "test", 10 }
		};
		UnityEngine.Analytics.Analytics.CustomEvent("test", eventData);

		deathCanvas.SetActive (true);
		deathCanvas.transform.Find ("Score Text").GetComponent<Text> ().text = "Your score: " + totalScore;
		RoundEnd ();
	}

	public void RoundEnd(){
		Destroy (spawner);
		for (int i = 0; i < playerManagers.Length; i++) {
			playerManagers [i].playerInstance.SetActive (false);
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			Destroy (enemy);
		}
		foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("PickUp")) {
			Destroy (pickup);
		}
		foreach (GameObject cursor in GameObject.FindGameObjectsWithTag ("CursorPointer")) {
			Destroy (cursor);
		}
	}

	public void TransitionDeathToMain(){
		DestroyDungeon ();
		LoadHomeScreen ();
	}

	public void DestroyDungeon(){
		foreach (PlayerManager playermanager in playerManagers){
			Destroy (playermanager.playerInstance);
		}
		Destroy (torch);
		Destroy (GameObject.Find ("Dungeon"));
		Destroy (UI);
		Destroy (TorchFOV);
		deathCanvas.SetActive (false);
		gameStarted = false;
	}

	public void LoadHomeScreen(){
		homeScreen.SetActive (true);
		homeScreenCam.SetActive (true);
		audioSource.clip = audioHomeScreen;
		audioSource.Play ();
		resetHomeScreenPlayer ();
	}

	public void updateScore(int addedScore){
		score += addedScore;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
	}

	public void resetHomeScreenPlayer(){
        GameObject.Find ("HomeScreenPlayer").transform.position = homeScreenPlayerPosition;
	}

	public void Proceed(){
		RoundEnd ();
		DestroyDungeon ();
		StartGame ();
	}
}
