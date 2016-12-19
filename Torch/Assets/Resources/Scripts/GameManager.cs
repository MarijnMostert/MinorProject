﻿using UnityEngine;
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
	public float StartTime;

	public bool paused;
	public GameObject pauseScreen;
	public GameObject inGameCameraPrefab;
	public GameObject inGameCameraObject;
	public GameObject camTarget;
	public GameObject enemyTarget;
	public GameObject UIPrefab;
	public GameObject UI;

	public List<GameObject> PuzzleRooms;

	public GameObject[] UIHelpItems;
	public bool UIHelp = true;
	public GameObject TorchFOVPrefab;
	public GameObject TorchFOV;

    public Spawner spawner;
	public GameObject triggerFloorPrefab;
	private GameObject triggerFloorObject;


    //masterGenerator Vars
    int width = 40;// = 100;
    int height = 40;// = 90;
    int radius = 2;// = 2;
    int maxlength = 2;// = 3;
    int timeout = 2000;// = 200;
    int minAmountOfRooms = 4;// = 6;
    int maxAmountOfRooms = 7;// = 8;
    int chanceOfRoom = 10;// = 15; Dit is de 1/n kans op een kamer, dus groter getal is kleinere kans

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
	public AudioClip[] audioDungeon;
	public bool audioMuted;

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
		WriteStart ();
		pauseScreen.SetActive (false);
		loadingScreenCanvas = Instantiate (loadingScreenCanvas) as GameObject;
		loadingScreenCanvas.SetActive (false);
		homeScreenPlayerPosition = GameObject.Find ("HomeScreenPlayer").transform.position;
	}

	public void StartGame(){
        if (!gameStarted) {
			Time.timeScale = 1f;
			StartTime = Time.time;
			dungeonLevel++;
			endOfRoundCanvas.SetActive (false);
            loadingScreenCanvas.SetActive(true);
            StartCoroutine(CreateDungeon());
            gameStarted = true;
        }
	}

	IEnumerator CreateDungeon(){
		yield return new WaitForSeconds (.1f);
		masterGenerator = new MasterGenerator(this.gameObject, width, height, radius, maxlength, timeout, minAmountOfRooms, maxAmountOfRooms, chanceOfRoom, PuzzleRooms);
		masterGenerator.LoadPrefabs();
		masterGenerator.Start();

		if (UI == null) {
			UI = Instantiate (UIPrefab);
			UIHelpItems = GameObject.FindGameObjectsWithTag ("UI Help");
		}
		TorchFOV = Instantiate (TorchFOVPrefab);
		if(triggerFloorObject == null)
			triggerFloorObject = Instantiate (triggerFloorPrefab);

		camTarget = torch.gameObject;
		enemyTarget = torch.gameObject;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.UI = UI;
		torch.TorchFOV = TorchFOV.GetComponentInChildren<Animator> ();

		inGameCameraObject = Instantiate (inGameCameraPrefab);
		mainCamera = inGameCameraObject.GetComponentInChildren<Camera> ();

		for (int i = 0; i < playerManagers.Length; i++) {
			if (playerManagers [i].playerInstance == null) {
				Debug.Log ("Create Player with id:" + i);
				playerManagers [i].playerInstance = Instantiate (playerPrefab, masterGenerator.dungeon_instantiate.startPos, playerManagers [i].spawnPoint.rotation) as GameObject;
				playerManagers [i].playerNumber = i + 1;
				playerManagers [i].Setup ();
				playerManagers [i].playerMovement.mainCamera = mainCamera;
			} else {
				playerManagers [i].playerInstance.transform.position = masterGenerator.dungeon_instantiate.startPos;
				playerManagers [i].Setup ();
				playerManagers [i].playerInstance.SetActive (true);
			}
		}

		Vector3 startpoint = masterGenerator.MovePlayersToStart ();
		torch.transform.position = startpoint + new Vector3 (6, .5f, 0);

		torch.cam = mainCamera;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
		UI.transform.FindChild ("Dungeon Level").GetComponent<Text> ().text = "Dungeon level " + dungeonLevel;

		audioSource.clip = audioDungeon [UnityEngine.Random.Range (0, audioDungeon.Length)];
		audioSource.Play ();
		homeScreen.SetActive (false);
		homeScreenCam.SetActive (false);
		loadingScreenCanvas.SetActive (false);

		yield return null;
	}
	
	void Update () {
		if (Input.GetButtonDown ("Pause"))
			Pause ();
		if(Input.GetKeyDown(KeyCode.I)){
			deathCanvas.SetActive (true);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			GameObject obj = ObjectPooler.current.GetObject ();
			obj.SetActive (true);
			obj.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}
	}

	void Pause(){
		if (!paused) {
			Time.timeScale = 0;
			paused = true;
			pauseScreen.SetActive (true);
			if(spawner != null)
				spawner.dead = true;
		} else {
			Time.timeScale = 1;
			paused = false;
			pauseScreen.SetActive (false);
			if (spawner != null)
				spawner.dead = false;
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
			{ "Event", "Death" },
			{ "Score", totalScore },
			{ "Level", dungeonLevel},
			{ "TotalTime", Time.time}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("Death", eventData);
		WriteToFile (eventData);

		deathCanvas.SetActive (true);
		deathCanvas.transform.Find ("Score Text").GetComponent<Text> ().text = "Your score: " + totalScore;
		RoundEnd ();
	}

	public void RoundEnd(){
		if(spawner != null)
			Destroy (spawner);
		for (int i = 0; i < playerManagers.Length; i++) {
			if(playerManagers[i].playerInstance != null)
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
		Destroy(GameObject.FindGameObjectWithTag ("Torch"));
	}

	public void TransitionDeathToMain(){
		RoundEnd ();
		DestroyDungeon ();
		if (UI != null)
			Destroy (UI);
		if (triggerFloorObject != null)
			Destroy (triggerFloorObject);
		LoadHomeScreen ();
		if (paused)
			Pause ();
	}

	public void DestroyDungeon(){
		if(torch != null)
			Destroy (torch);
		if(GameObject.Find("Dungeon") != null)
			Destroy (GameObject.Find ("Dungeon"));
		if(TorchFOV != null)
			Destroy (TorchFOV);
		deathCanvas.SetActive (false);
		gameStarted = false;
		Destroy (inGameCameraObject);
	}

	public void LoadHomeScreen(){
		foreach (PlayerManager playermanager in playerManagers){
			Destroy (playermanager.playerInstance);
		}
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

	public void WriteStart(){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "Event", "StartGame"},
			{ "Time", DateTime.UtcNow}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("LevelStart", eventData);
		WriteToFile (eventData);
	}
		
	public void WriteFinishLevel(){
		Dictionary<string, object> eventData = new Dictionary<string, object> {
			{ "Event", "FinishLevel"},
			{ "Level", dungeonLevel},
			{ "LevelScore", score },
			{ "TimeSpent", Time.time - StartTime}
		};
		UnityEngine.Analytics.Analytics.CustomEvent("LevelComplete", eventData);
		WriteToFile (eventData);
	}

	public void WriteToFile(Dictionary<string, object> dict){
		using (System.IO.StreamWriter file = new System.IO.StreamWriter ("data.txt", true)) {
			file.WriteLine ("{");
			foreach (KeyValuePair<string, object> entry in dict) {
				file.WriteLine (entry.Key + ":" + entry.Value);	
			}
			file.WriteLine ("}");
		}
	}

	public void MuteAudio(){
		if (audioMuted) {
			audioSource.mute = false;
			audioMuted = false;
			Debug.Log ("Audio is unmuted");
		} else {
			audioSource.mute = true;
			audioMuted = true;
			Debug.Log ("Audio is muted");
		}
	}

	public void HideUIHelp(){
		if (UI != null) {
			if (UIHelp) {
				foreach (GameObject obj in UIHelpItems) {
					obj.SetActive (false);
				}
				UIHelp = false;
				Debug.Log ("UI help is turned off");
			} else {
				foreach (GameObject obj in UIHelpItems) {
					obj.SetActive (true);
				}
				UIHelp = true;
				Debug.Log ("UI help is turned on");
			}
		}
	}
}
