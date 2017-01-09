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
	public Torch torchPrefab;
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
	public string Roomtype;

	public bool paused;
	public GameObject pauseScreen;
	public GameObject inGameCameraPrefab;
	private GameObject inGameCameraObject;
	public GameObject camTarget;
	public GameObject enemyTarget;
	public GameObject UIPrefab;
	public GameObject UI;
	public UIInventory uiInventory;

	public GameAnalytics analytics = new GameAnalytics();
	public Save saver = new Save();

	public List<GameObject> PuzzleRooms;

	public GameObject[] UIHelpItems;
	public bool UIHelp = true;
	public GameObject TorchFOVPrefab;
	private GameObject TorchFOV;

	public Spawner spawner;
	public GameObject triggerFloorPrefab;
	private GameObject triggerFloorObject;

	public ProceduralMaterial[] substances;

    //masterGenerator Vars
	public int width = 20;//40;// = 40;
	public int height = 20;//40;// = 40;
    int radius = 2;// = 2;
    int maxlength = 2;// = 2;
    int timeout = 200;// = 2000;
	int minAmountOfRooms = 2;//4;// = 4;
	int maxAmountOfRooms = 4;//47;// = 7;
    int chanceOfRoom = 5;// = 10; Dit is de 1/n kans op een kamer, dus groter getal is kleinere kans


	//public GameObject homeScreenCanvas;
	public GameObject loadingScreenCanvas;
	public GameObject deathCanvas;
	public GameObject endOfRoundCanvas;
	private GameObject homeScreen;
	private GameObject homeScreenCam;
	public Camera mainCamera;
	public Camera minimap;
	private Vector3 homeScreenPlayerPosition;
    MasterGenerator masterGenerator;
    bool gameStarted;
	bool tutorialStarted;
	public GameObject tutorialPrefab;
	private GameObject tutorialObject;
	public GameObject tutorialTorchPrefab;
	private GameObject tutorialTorchObject;

	public AudioSource audioSourceMusic;
	public AudioSource audioSourceUI;
	public AudioClip[] audioClipsUI;
	public AudioClip audioHomeScreen;
	public AudioClip[] audioDungeon;
	public bool audioMuted;

	private GameObject DebuggerPanel;
	public GameObject[] allWeaponsAvailable;
	public GameObject[] allPowerUpsAvailable;

	public int collectedKeys;
	public int requiredCollectedKeys;
	public GameObject Bold;
	private Bold BoldScript;

	[HideInInspector] public int numberOfPlayers = 1;

    void Awake () {
        gameStarted = false;
		tutorialStarted = false;
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
		minimap = Resources.Load ("Prefabs/Minimap", typeof (Camera)) as Camera;
    }

    public void Start(){
		analytics.WriteStart ();
		pauseScreen.SetActive (false);
		loadingScreenCanvas = Instantiate (loadingScreenCanvas) as GameObject;
		loadingScreenCanvas.SetActive (false);
		homeScreenPlayerPosition = GameObject.Find ("HomeScreenPlayer").transform.position;
		Bold = Instantiate (Bold, homeScreenPlayerPosition, Quaternion.identity) as GameObject;
		BoldScript = Bold.GetComponent<Bold> ();
		BoldScript.speechText.text = "";
		BoldScript.speechImage.gameObject.SetActive (false);
	}

	void Parameters(int level){
		if (level == 1) {
			width = 20;
			height = 20;
			minAmountOfRooms = 2;
			maxAmountOfRooms = 3;
		}
		if (level == 2) {
			width = 25;
			height = 25;
			minAmountOfRooms = 4;
			maxAmountOfRooms = 5;
		}
		if (level == 3) {
			width = 30;
			height = 30;
			minAmountOfRooms = 5;
			maxAmountOfRooms = 6;
		}
		if (level == 4) {
			width = 35;
			height = 35;
			minAmountOfRooms = 5;
			maxAmountOfRooms = 8;
		}
		if (level == 5) {
			width = 40;
			height = 40;
			minAmountOfRooms = 6;
			maxAmountOfRooms = 10;
		}
		if (level > 5) {
			width = 50;
			height = 50;
			minAmountOfRooms = 7;
			maxAmountOfRooms = 20;
		}
	}

	public void StartGame(){
        if (!gameStarted) {
			Time.timeScale = 1f;
			StartTime = Time.time;
			dungeonLevel = saver.Read ();
			dungeonLevel++;
			Parameters (dungeonLevel);
			endOfRoundCanvas.SetActive (false);
			loadingScreenCanvas.transform.Find ("LevelText").GetComponent<Text> ().text = "Dungeon level: " + (dungeonLevel-1).ToString();
            loadingScreenCanvas.SetActive(true);
			homeScreen.SetActive (false);
            StartCoroutine(CreateLevel(1));
            gameStarted = true;
			StartCoroutine (WaitSpawning ());

			//Instantiate (minimap);
		}
	}

	IEnumerator WaitSpawning(){
		float wait = 11.0f - dungeonLevel;
		if (wait < 1.0f) {
			wait = 1.0f;
		}
		yield return new WaitForSeconds (wait);
		spawner.activated = true;
		Debug.Log ("spawner activated");
	}

	//where type 0 is tutorial and type 1 is dungeon.
	IEnumerator CreateLevel(int type){
		if (inGameCameraObject == null) {
			inGameCameraObject = Instantiate (inGameCameraPrefab);
			mainCamera = inGameCameraObject.GetComponentInChildren<Camera> ();
		}

		yield return new WaitForSeconds (.1f);
		//RandomizeTextures ();

		if (type == 1) {
			masterGenerator = new MasterGenerator (this.gameObject, width, height, radius, maxlength, timeout, minAmountOfRooms, maxAmountOfRooms, chanceOfRoom, PuzzleRooms);
			masterGenerator.LoadPrefabs ();
			masterGenerator.Start ();
		} else if (type == 0) {
			tutorialObject = Instantiate(tutorialPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;

		}

		if (UI == null) {
			UI = Instantiate (UIPrefab);
			UIHelpItems = GameObject.FindGameObjectsWithTag ("UI Help");
			uiInventory = UI.GetComponentInChildren<UIInventory> ();
		}

		TorchFOV = Instantiate (TorchFOVPrefab);

		if(triggerFloorObject == null)
			triggerFloorObject = Instantiate (triggerFloorPrefab);

		torch = Instantiate (torchPrefab) as Torch;

		camTarget = torch.gameObject;
		enemyTarget = torch.gameObject;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.UI = UI;
		torch.TorchFOV = TorchFOV.GetComponentInChildren<Animator> ();

		collectedKeys = 0;

		for (int i = 0; i < playerManagers.Length; i++) {
			if (playerManagers [i].playerInstance == null) {
				Debug.Log ("Create Player with id:" + i);
				playerManagers [i].playerInstance = Instantiate (playerPrefab) as GameObject;
				playerManagers [i].playerNumber = i + 1;
				playerManagers [i].Setup ();
				playerManagers [i].playerMovement.mainCamera = mainCamera;
				playerManagers [i].gameManager = this;
			} else {
	//			playerManagers [i].playerInstance.transform.position = masterGenerator.dungeon_instantiate.startPos;
				playerManagers [i].Setup ();
				playerManagers [i].playerInstance.SetActive (true);
			}
		}

		SetNumberOfPlayers (numberOfPlayers);

		//Moving players, torch and Bold to the correct place
		Vector3 startpoint = new Vector3(0f,0f,0f);
		if (type == 1) {
			startpoint = masterGenerator.MovePlayersToStart ();
			torch.isDamagable = true;
		} else if (type == 0) {
			startpoint = tutorialObject.transform.Find ("Spawnpoint").transform.position;
			playerManagers [0].playerInstance.transform.position = startpoint;
			playerManagers [1].playerInstance.transform.position = startpoint + new Vector3 (-2f, 0f, -2f);
			torch.isDamagable = false;
		}

		torch.transform.position = startpoint + new Vector3 (6, .5f, 0);
		Bold.transform.position = startpoint;

		torch.cam = mainCamera;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
		if (type == 1) {
			UI.transform.FindChild ("Dungeon Level").GetComponent<Text> ().text = "Dungeon level " + dungeonLevel;
		} else if (type == 0) {
			UI.transform.FindChild ("Dungeon Level").GetComponent<Text> ().text = "Dungeon Tutorial";
			BoldScript.speechText.text = "Welcome to this tutorial! My name is Bold. Use the WASD-keys to move.";
			BoldScript.speechImage.gameObject.SetActive (true);
		}

		audioSourceMusic.clip = audioDungeon [UnityEngine.Random.Range (0, audioDungeon.Length)];
		audioSourceMusic.Play ();
		homeScreenCam.SetActive (false);
		loadingScreenCanvas.SetActive (false);

		yield return null;
	}

	public void RandomizeTextures () {
		foreach (ProceduralMaterial substance in substances){
			UnityEngine.Random.InitState( (int)Time.time);
			float random_value = (float)UnityEngine.Random.Range(0,100000);
			substance.SetProceduralFloat("$randomseed", random_value);
			substance.RebuildTextures();
		}
	}

	void Update () {
		if (Input.GetButtonDown ("Pause"))
			Pause ();
		/*
		if(Input.GetKeyDown(KeyCode.I)){
			deathCanvas.SetActive (true);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			GameObject obj = ObjectPooler.current.GetObject ();
			obj.SetActive (true);
			obj.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}
		*/
		if (Input.GetKeyDown (KeyCode.H)) {
			if (DebuggerPanel != null) {
				if (DebuggerPanel.activeInHierarchy)
					DebuggerPanel.SetActive (false);
				else
					DebuggerPanel.SetActive (true);
			}
		}

		//Cheatcode to spawn all weapons around the torch
		if (Input.GetKeyDown (KeyCode.N)) {
			SpawnAllWeapons ();
		}
		//Cheatcode to spawn all powerups around the torch
		if (Input.GetKeyDown (KeyCode.B)) {
			SpawnAllPowerUps ();
		}
		//Cheatcode to proceed to the next level
		if (Input.GetKeyDown (KeyCode.L)) {
			Proceed ();
		}
		//Cheatcode to get full health
		if (Input.GetKeyDown (KeyCode.K) && torch != null) {
			torch.HealToStartingHealth ();
		}
		//Cheatcode to toggle if the torch is damagable or not
		if (Input.GetKeyDown (KeyCode.J) && torch != null) {
			torch.ToggleDamagable ();
		}
	}

	void Pause(){
		if (!paused) {
			Time.timeScale = 0;
			paused = true;
			pauseScreen.SetActive (true);
			foreach (PlayerManager PM in playerManagers) {
				if(PM.playerInstance != null)
					PM.EnableMovement (false);
			}
		} else {
			Time.timeScale = 1;
			paused = false;
			pauseScreen.SetActive (false);
			foreach (PlayerManager PM in playerManagers) {
				if(PM.playerInstance != null)
					PM.EnableMovement(true);
			}
		}
	}

	public void GameOver(){
		totalScore += score;
		analytics.WriteDeath (totalScore, dungeonLevel);
		deathCanvas.SetActive (true);
		deathCanvas.transform.Find ("Score Text").GetComponent<Text> ().text = "Your score: " + totalScore;
		RoundEnd ();
		dungeonLevel = 0;
	}

	public void RoundEnd(){
		if(spawner != null)
			Destroy (spawner);
		if (torch != null)
			Destroy (torch.gameObject);
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
		foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("EnemyProjectile")) {
			Destroy (projectile);
		}
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
		if(GameObject.Find("Dungeon") != null)
			Destroy (GameObject.Find ("Dungeon"));
		if(TorchFOV != null)
			Destroy (TorchFOV);
		deathCanvas.SetActive (false);
		gameStarted = false;
		tutorialStarted = false;
		if (tutorialObject != null)
			Destroy (tutorialObject);
	}

	public void LoadHomeScreen(){
		Destroy (inGameCameraObject);
		foreach (PlayerManager playermanager in playerManagers){
			Destroy (playermanager.playerInstance);
		}
		homeScreen.SetActive (true);
		homeScreenCam.SetActive (true);
		audioSourceMusic.clip = audioHomeScreen;
		audioSourceMusic.Play ();
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
		saver.ToFile (dungeonLevel);
		analytics.WriteFinishLevel (dungeonLevel, score, totalScore, StartTime);
		score = 0;
		RoundEnd ();
		DestroyDungeon ();
		StartGame ();
	}

	public void MuteAudio(){
		if (audioMuted) {
			audioSourceMusic.mute = false;
			audioMuted = false;
			Debug.Log ("Audio is unmuted");
		} else {
			audioSourceMusic.mute = true;
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

	public void SetNumberOfPlayers(int number){
		if (number == 1) {
			playerManagers [1].Enable (false);
			if (mainCamera != null)
				mainCamera.GetComponent<CameraController> ().UpdateTargets ();
			if (uiInventory != null) {
				uiInventory.SetNumberOfPlayers (number);
				uiInventory.weaponInventory.RemoveIndicatorOffset ();
			}
			numberOfPlayers = 1;
		} else if (number == 2 && !playerManagers[1].active) {
			playerManagers [1].Enable (true);
			if (mainCamera != null)
				mainCamera.GetComponent<CameraController> ().UpdateTargets ();
			if (uiInventory != null) {
				uiInventory.SetNumberOfPlayers (number);
				uiInventory.weaponInventory.ApplyIndicatorOffset ();
			}
			numberOfPlayers = 2;
		}
	}

	void SpawnAllWeapons(){
		foreach (GameObject weapon in allWeaponsAvailable) {
			Instantiate (weapon, torch.transform.position + new Vector3 (UnityEngine.Random.Range (-2f, 2f), 0f, UnityEngine.Random.Range (-2f, 2f)), Quaternion.identity);
		}
	}

	void SpawnAllPowerUps(){
		foreach (GameObject powerup in allPowerUpsAvailable) {
			Instantiate (powerup, torch.transform.position + new Vector3 (UnityEngine.Random.Range (-2f, 2f), 0f, UnityEngine.Random.Range (-2f, 2f)), Quaternion.identity);
		}
	}

	public void WriteTorchPickup(){
		analytics.WriteTorchPickup (dungeonLevel, StartTime);
	}
		
	public void WritePuzzleStart (){
		analytics.WritePuzzleStart (Roomtype, dungeonLevel);
	}

	public void WritePuzzleComplete(float Time){
		analytics.WritePuzzleComplete (Roomtype, Time, dungeonLevel);
	}

	public void StartTutorial(){
		if (!tutorialStarted) {
			requiredCollectedKeys = 1;
			Time.timeScale = 1f;
			StartTime = Time.time;
			loadingScreenCanvas.SetActive (true);
			homeScreen.SetActive (false);
			StartCoroutine (CreateLevel (0));
			tutorialStarted = true;
		}
	}

	/*
	IEnumerator LoadTutorial(){

		yield return new WaitForSeconds (.1f);

		//Load tutorial prefab
		tutorialObject = Instantiate(tutorialPrefab);
		Transform Spawnpoint = tutorialObject.transform.Find ("Spawnpoint");

		tutorialTorchObject = Instantiate (tutorialTorchPrefab, Spawnpoint.position + new Vector3(0f, -.5f, 0f), Quaternion.identity, tutorialObject.transform) as GameObject;
		Torch tutTorch = tutorialTorchObject.GetComponent<Torch> ();

		if (UI == null) {
			UI = Instantiate (UIPrefab, tutorialObject.transform) as GameObject;
			UIHelpItems = GameObject.FindGameObjectsWithTag ("UI Help");
		}

		TorchFOV = Instantiate (TorchFOVPrefab, tutorialObject.transform) as GameObject;

		camTarget = tutorialTorchObject;
		enemyTarget = tutTorch.gameObject;
		tutTorch.health = torchStartingHealth;
		tutTorch.gameManager = this;
		tutTorch.UI = UI;
		tutTorch.TorchFOV = TorchFOV.GetComponentInChildren<Animator> ();
		torch = tutTorch;

		inGameCameraObject = Instantiate (inGameCameraPrefab, tutorialObject.transform) as GameObject;
		mainCamera = inGameCameraObject.GetComponentInChildren<Camera> ();
		Bold.GetComponentInChildren<LookAtCamera> ().cam = mainCamera;

		for (int i = 0; i < playerManagers.Length; i++) {
			if (playerManagers [i].playerInstance == null) {
				Debug.Log ("Create Player with id:" + i);
				playerManagers [i].playerInstance = Instantiate (playerPrefab, Spawnpoint.position, Quaternion.identity, tutorialObject.transform) as GameObject;
				playerManagers [i].playerNumber = i + 1;
				playerManagers [i].Setup ();
				playerManagers [i].playerMovement.mainCamera = mainCamera;
				playerManagers [i].gameManager = this;
			} else {
				playerManagers [i].playerInstance.transform.position = Spawnpoint.position;
				playerManagers [i].Setup ();
				playerManagers [i].playerInstance.SetActive (true);
			}
		}

		Bold.transform.position = Spawnpoint.position;

		tutTorch.cam = mainCamera;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
		UI.transform.FindChild ("Dungeon Level").GetComponent<Text> ().text = "Dungeon level " + dungeonLevel;

		audioSourceMusic.clip = audioDungeon [UnityEngine.Random.Range (0, audioDungeon.Length)];
		audioSourceMusic.Play ();
		homeScreenCam.SetActive (false);
		loadingScreenCanvas.SetActive (false);

		SetNumberOfPlayers (1);

		yield return null;
	}
	*/

	public void ExitGame(){
		Application.Quit ();
	}

	public void playUISound(int soundNumber){
		audioSourceUI.clip = audioClipsUI [soundNumber];
		audioSourceUI.Play ();
	}
}
