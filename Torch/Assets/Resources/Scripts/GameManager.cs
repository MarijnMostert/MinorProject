using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Analytics;
using UnityEditor;

public class GameManager : MonoBehaviour {

	//Stored player preferences and info
	public Data data;
	public DungeonData dungeonData;
	public Achievements achievements;
	private HomeScreenProgress homeScreenProgress;

	//Player data
	public static GameManager Instance;
	public PlayerManager[] playerManagers;
	public GameObject playerPrefab;
	public Vector3 RespawnPosition;

	public GameObject homeScreenPlayer;
	public HomeScreenMovement homeScreenMovement;

	//Torch data
	public Torch torchPrefab;
	public Torch torch;
	public GameObject torchMinimapIndicator;
	public Transform torchSpawnPoint;
	public int torchStartingHealth = 100;
	public int torchHealth;

	//Score info
	public int score = 0;
	public int totalScore = 0;
	public int coinsInGame = 0;
	public int dungeonLevel = 0;
	public float StartTime;
	public string Roomtype;

	public static int totalKeysCollected = 0;

	public bool paused;
	public GameObject pauseScreen;
	public GameObject inGameCameraPrefab;
	private GameObject inGameCameraObject;
	public GameObject camTarget;
	public GameObject enemyTarget;
	public UI UIPrefab;
	public UI ui;
	public UIInventory uiInventory;

	public GameAnalytics analytics = new GameAnalytics();
	public Save saver = new Save();

	//public List<GameObject> PuzzleRooms;

	public GameObject TorchFOVPrefab;
	private GameObject TorchFOV;

	public Spawner spawner;
	public GameObject triggerFloorPrefab;
	private GameObject triggerFloorObject;

	public bool RandomizeTexturesAllowed;
	public ProceduralMaterial[] substances;

	private bool cheat;
	int cheatindex;
	private string[] cheatCode;

	private bool TextFieldEnabled = false;

    //masterGenerator Vars
    int radius = 2;// = 2;
    int maxlength = 3;// = 2;
    int timeout = 200;// = 2000;


	//public GameObject homeScreenCanvas;
	public GameObject homeScreenEnvironmentPrefab;
	public GameObject homeScreenEnvironment;
	public GameObject HighScoresPanel;
	public GameObject startingScreen;
	public GameObject loadingScreenCanvas;
	public DeathCanvas deathCanvas;
	public GameObject endOfRoundCanvas;
	public DungeonStartCanvas dungeonStartCanvas;
	public GameObject dungeonLevelButtonPrefab;
	private GameObject homeScreen;
	private GameObject homeScreenCam;
	public Camera mainCamera;
	private Camera minimapPrefab;
	public Camera minimap;
	public GameObject minimapUIElement;
	private Vector3 homeScreenPlayerPosition;
    MasterGenerator masterGenerator;
	[HideInInspector] public Transform levelTransform;
    bool gameStarted;

	bool tutorialStarted;
	public GameObject tutorialPrefab;
	private GameObject tutorialObject;
	public GameObject tutorialTorchPrefab;
	private GameObject tutorialTorchObject;

	bool arenaStarted;
	public GameObject arenaPrefab;
	private GameObject arenaObject;
	public GameObject arenaTorchPrefab;
	private GameObject arenaTorchObject;

	[Header("- Audio")]
	public AudioSource audioSourceMusic;
	public AudioSource audioSourceUI;
	public AudioClip[] audioClipsUI;
	public AudioClip loreAudio;
	public AudioClip audioHomeScreen;
	public AudioClip[] audioDungeon;
	public AudioClip audioPartyTorch;
	public AudioClip audioGoldenTorch;
	private bool audioMuted = false;

	[Header("- Debugging properties")]
	public GameObject DebuggerPanel;
	public GameObject[] allWeaponsAvailable;
	public GameObject[] allPowerUpsAvailable;
	public GameObject KeyPrefab;

	public int collectedKeys;
	public int requiredCollectedKeys;
	public GameObject Pet;
	private Pet PetScript;
	public GameObject Bold;
	private Pet BoldPetScript;

	public Shop shopPrefab;
	private Shop shop;
	private bool shopActive = false;

	public List<GameObject> highQualityItems;

	[HideInInspector] public ArenaManager arenaManager;

	[HideInInspector] public int numberOfPlayers = 1;

    void Awake () {
		data.Load ();
		setQuality (data.highQuality);

		dungeonData = GetComponent<DungeonData> ();
		homeScreenProgress = GetComponent<HomeScreenProgress> ();

		//////////////////maar er zitten echt 8 scripts op ???????????????
		arenaManager = GetComponentInChildren<ArenaManager> ();

		startingScreen.SetActive (true);

		DebuggerPanel = Instantiate (DebuggerPanel);
		DebuggerPanel.SetActive (false);

        if(PlayerPrefs.HasKey("id"))
    		SetUpDungeonStartCanvas ();

        gameStarted = false;
		tutorialStarted = false;
		//Makes sure this object is not deleted when another scene is loaded.
		if (Instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			GameObject.DontDestroyOnLoad (this.gameObject);
			Instance = this;
		}

		cheat = false; //put this to false in build
		cheatindex = 0;
		cheatCode = new string[] { "w", "o", "c", "h", "e", "n", "e", "n", "d", "e" };

		//homeScreenCanvas = GameObject.Find ("Home Screen Canvas");
		homeScreen = GameObject.Find ("HomeScreen");
		homeScreenCam = GameObject.Find ("HomeScreenCam");
		mainCamera = homeScreenCam.GetComponent<Camera> ();
		minimapPrefab = Resources.Load ("Prefabs/Minimap", typeof (Camera)) as Camera;

		HighScoresPanel = Instantiate (HighScoresPanel) as GameObject;
    }

    public void Start(){
		analytics.WriteStart ();
		pauseScreen.SetActive (false);
		loadingScreenCanvas = Instantiate (loadingScreenCanvas) as GameObject;
		loadingScreenCanvas.SetActive (false);
		homeScreenPlayerPosition = homeScreenPlayer.transform.position;
		Pet = Instantiate (Pet, homeScreenPlayerPosition, Quaternion.identity) as GameObject;
		PetScript = Pet.GetComponent<Pet> ();
		PetScript.speechText.text = "";
		PetScript.speechImage.gameObject.SetActive (false);
		Bold = Instantiate (Bold) as GameObject;
		BoldPetScript = Bold.GetComponent<Pet> ();
		BoldPetScript.attacking = false;
		Bold.SetActive (false);

		shopPrefab.EquipActives ();
		if (PlayerPrefs.HasKey ("id")) {
			homeScreenProgress.UpdateProgress (data.maxAchievedDungeonLevel);
		}

	}

	public void StartGame(){
        if (!gameStarted) {
			Time.timeScale = 1f;
			endOfRoundCanvas.SetActive (false);
			loadingScreenCanvas.transform.Find ("LevelText").GetComponent<Text> ().text = "Dungeon level: " + (dungeonLevel).ToString();
			loadingScreenCanvas.transform.Find ("RandomizingTexturesText").gameObject.SetActive(RandomizeTexturesAllowed);
            loadingScreenCanvas.SetActive(true);
			Destroy (homeScreenEnvironment);
			homeScreen.SetActive (false);
            StartCoroutine(CreateLevel(1));
            gameStarted = true;
		}
	}

	//where type 0 is tutorial and type 1 is dungeon and type 2 is arena.
	IEnumerator CreateLevel(int type){
		if (inGameCameraObject == null) {
			inGameCameraObject = Instantiate (inGameCameraPrefab);
			mainCamera = inGameCameraObject.GetComponentInChildren<Camera> ();
		}

		yield return new WaitForSeconds (.1f);
		if (RandomizeTexturesAllowed || (dungeonLevel == 1 && type == 1) || type == 2 || type == 0) {
			RandomizeTextures ();
		}

		if (type == 1) {
			masterGenerator = new MasterGenerator (this.gameObject, dungeonData.dungeonParameters[dungeonLevel], radius, maxlength, timeout);
			masterGenerator.LoadPrefabs ();
			masterGenerator.Constructing ();
		} else if (type == 0) {
			tutorialObject = Instantiate(tutorialPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			levelTransform = tutorialObject.transform;

		} else if (type == 2) {
			arenaObject = Instantiate(arenaPrefab, new Vector3(-150f,0f,150f), Quaternion.identity) as GameObject;
			levelTransform = arenaObject.transform;
		}

		if (ui == null) {
			ui = Instantiate (UIPrefab);
			uiInventory = ui.uiInventory;
		}

		TorchFOV = Instantiate (TorchFOVPrefab);

		triggerFloorObject = Instantiate (triggerFloorPrefab, levelTransform) as GameObject;

		torch = Instantiate (torchPrefab) as Torch;
		Instantiate (torchMinimapIndicator, torch.transform.position + new Vector3 (0f, -3f, 0f), Quaternion.Euler(new Vector3(90f, 0f, 0f)), torch.transform);


		camTarget = torch.gameObject;
		enemyTarget = torch.gameObject;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.ui = ui;
		torch.TorchFOV = TorchFOV.GetComponentInChildren<Animator> ();

		collectedKeys = 0;

		for (int i = 0; i < playerManagers.Length; i++) {
			if (playerManagers [i].playerInstance == null) {
//				Debug.Log ("Create Player with id:" + i);
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

		ApplySkins ();

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
		} else if (type == 2) {
			startpoint = arenaObject.transform.Find ("Spawnpoint").transform.position;
			playerManagers [0].playerInstance.transform.position = startpoint;
			playerManagers [1].playerInstance.transform.position = startpoint + new Vector3 (-2f, 0f, -2f);
			torch.isDamagable = true;
		}
		RespawnPosition = startpoint;
		torch.transform.position = startpoint + new Vector3 (6, .5f, 0);

		if (type == 1 || type == 2) {
			Pet.transform.position = playerManagers [0].playerInstance.transform.position + new Vector3 (3f, 0f, 0f);
			Pet.SetActive (true);
			Bold.SetActive (false);
		} else if (type == 0) {
			Pet.SetActive (false);
			Bold.SetActive (true);
			Bold.transform.position = playerManagers [0].playerInstance.transform.position;
		}

		torch.torchPickUp.cam = mainCamera;
		if (type == 1) {
			ui.dungeonLevelText.text = "Dungeon level " + dungeonLevel;
		} else if (type == 0) {
			ui.dungeonLevelText.text = "Dungeon Tutorial";
			BoldPetScript.speechCanvas.SetActive (true);
			BoldPetScript.speechText.text = "Welcome to this tutorial! My name is Bold. Use the WASD-keys to move.";
			BoldPetScript.speechImage.gameObject.SetActive (true);
		} else if (type == 2) {
			ui.dungeonLevelText.text = "ArenaMode";
			arenaManager.StartArena ();
		}

		audioSourceMusic.clip = audioDungeon [UnityEngine.Random.Range (0, audioDungeon.Length)];
		if (torch.GetComponent<PartyTorch> () != null) {
			audioSourceMusic.clip = audioPartyTorch;
		}
		if (torch.GetComponent<GoldenTorch> () != null) {
			audioSourceMusic.clip = audioGoldenTorch;
		}
		audioSourceMusic.Play ();
		score = 0;
		SetScore (totalScore);
		SetInGameCoin (coinsInGame);
		homeScreenCam.SetActive (false);
		loadingScreenCanvas.SetActive (false);

		minimap = Instantiate (minimapPrefab);

		ui.timer.Reset ();

		StartTime = Time.time;
		mainCamera.GetComponent<CameraController> ().SetMode ("Normal");

		yield return null;
	}

	public void toggleRandomizeTexturesAllowed (bool newBool) {
		RandomizeTexturesAllowed = newBool;
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
		if (Input.GetButtonDown ("Pause") && !TextFieldEnabled)
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

		if (Input.anyKeyDown) {
			if (Input.GetKeyDown(cheatCode[cheatindex])) {
				cheatindex++;
			} else {
				cheatindex = 0;    
			}
		}
		if (cheatindex == cheatCode.Length) {
			cheatindex = 0;
			Debug.Log ("hoch die hande!");
			cheat = !cheat;

			if (!achievements.cheats_unlocked) {
				achievements.cheatsAchievement ();
			}
		}

		if (Input.GetKeyDown (KeyCode.H) && cheat == true && !TextFieldEnabled) {
			if (DebuggerPanel != null) {
				if (DebuggerPanel.activeInHierarchy)
					DebuggerPanel.SetActive (false);
				else
					DebuggerPanel.SetActive (true);
			}
		}

		//Cheatcode to spawn all weapons around the torch
		if (Input.GetKeyDown (KeyCode.N) && cheat && !TextFieldEnabled) {
			SpawnAllWeapons ();
		}
		//Cheatcode to spawn all powerups around the torch
		if (Input.GetKeyDown (KeyCode.B) && cheat && !TextFieldEnabled) {
			SpawnAllPowerUps ();
		}
		//Cheatcode to proceed to the next level
		if (Input.GetKeyDown (KeyCode.L) && cheat && !TextFieldEnabled) {
			Proceed ();
		}
		//Cheatcode to get full health
		if (Input.GetKeyDown (KeyCode.K) && torch != null && cheat && !TextFieldEnabled) {
			torch.HealToStartingHealth ();
		}
		//Cheatcode to toggle if the torch is damagable or not
		if (Input.GetKeyDown (KeyCode.J) && torch != null && cheat && !TextFieldEnabled) {
			torch.ToggleDamagable ();
		}
		//Cheatcode to kill all active enemies
		if(Input.GetKeyDown(KeyCode.LeftBracket) && cheat && !TextFieldEnabled){
			KillAllEnemies();
		}
		//Cheatcode to spawn a key
		if (Input.GetKeyDown (KeyCode.O) && cheat && !TextFieldEnabled) {
			SpawnKey ();
		}
		//Cheatcode to spawn to highscores
		if (Input.GetKeyDown (KeyCode.Alpha0) && cheat && !TextFieldEnabled) {
			TeleportToHighScores ();
		}
		//Cheatcode to get 1000 coins
		if (Input.GetKeyDown (KeyCode.Backslash) && cheat && !TextFieldEnabled) {
			data.coins += 1000;
		}
		//Cheatcode to reveal the whole minimap
		if (Input.GetKeyDown (KeyCode.V) && cheat && !TextFieldEnabled) {
			RevealMinimap ();
		}
		//MASTER CHEATCODE
		if (Input.GetKeyDown (KeyCode.Alpha9) && cheat && !TextFieldEnabled) {
			SpawnAllWeapons ();
			SpawnAllPowerUps ();
			torch.HealToStartingHealth ();
			torch.ToggleDamagable ();
			KillAllEnemies ();
			RevealMinimap ();
			spawner.ToggleSpawner ();
		}

		//Toggle minimap
		if(Input.GetButtonDown("ToggleMinimap") && !TextFieldEnabled){
			ToggleMiniMap();
		}

		//Add second player
		if (Input.GetButtonDown ("ToggleSecondPlayer") && !TextFieldEnabled) {
			int newNumberOfPlayers;
			if (numberOfPlayers == 1)
				newNumberOfPlayers = 2;
			else
				newNumberOfPlayers = 1;
			SetNumberOfPlayers (newNumberOfPlayers);
		}
	}

	void Pause(){
		if (!paused) {
			Time.timeScale = 0;
			paused = true;
			pauseScreen.SetActive (true);
			pauseScreen.GetComponent<PauseScreen> ().TurnOffAllPanels ();
			foreach (PlayerManager PM in playerManagers) {
				if(PM.playerInstance != null)
					PM.EnableMovement (false);
			}
			homeScreenMovement.enabled = false;
		} else {
			Time.timeScale = 1;
			paused = false;
			pauseScreen.SetActive (false);
			foreach (PlayerManager PM in playerManagers) {
				if(PM.playerInstance != null)
					PM.EnableMovement(true);
			}
			homeScreenMovement.enabled = true;
		}
	}

	public void GameOver(){
		analytics.WriteDeath (totalScore, dungeonLevel);
		deathCanvas.SetScoreText (totalScore);
		deathCanvas.SetCoinText (coinsInGame);
		deathCanvas.gameObject.SetActive (true);
		RoundEnd ();
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
		Destroy (GameObject.FindGameObjectWithTag ("Minimap"));
	}

	public void TransitionDeathToMain(){
		score = 0;
		totalScore = 0;
		coinsInGame = 0;
		RoundEnd ();
		DestroyDungeon ();
		if (ui != null)
			Destroy (ui.gameObject);
		if (triggerFloorObject != null)
			Destroy (triggerFloorObject);
		LoadHomeScreen ();
		if (paused)
			Pause ();
		homeScreenPlayer.SetActive (true);
		mainCamera = homeScreenCam.GetComponent<Camera> ();
	}

	public void DestroyDungeon(){
		if(GameObject.Find("Dungeon") != null)
			Destroy (GameObject.Find ("Dungeon"));
		if(TorchFOV != null)
			Destroy (TorchFOV);
		deathCanvas.gameObject.SetActive (false);
		gameStarted = false;
		tutorialStarted = false;
		if (tutorialObject != null)
			Destroy (tutorialObject);
		arenaStarted = false;
		if (arenaObject != null)
			Destroy (arenaObject);
	}

	public void LoadHomeScreen(){
		Destroy (inGameCameraObject);
		foreach (PlayerManager playermanager in playerManagers){
			Destroy (playermanager.playerInstance);
		}
		if (homeScreenEnvironment == null) {
			homeScreenEnvironment = Instantiate (homeScreenEnvironmentPrefab, homeScreen.transform) as GameObject;
		}
		homeScreen.SetActive (true);
		homeScreenCam.SetActive (true);
		homeScreenProgress.UpdateProgress (data.maxAchievedDungeonLevel);

		audioSourceMusic.clip = audioHomeScreen;
		audioSourceMusic.Play ();
		resetHomeScreenPlayer ();

		Pet.SetActive (true);
		Bold.SetActive (false);

		PetScript.speechCanvas.SetActive (false);
		Pet.transform.position = homeScreenPlayer.transform.position;

	}

	public void updateScore(int addedScore){
		totalScore += addedScore;
		score += addedScore;
		ui.scoreText.text = "Score: " + totalScore;
	}

	public void SetScore(int score){
		ui.scoreText.text = "Score: " + score;
	}

	public void addInGameCoin(){
		coinsInGame += 1;
		ui.coinsText.text = "Coins: " + coinsInGame;
	}

	public void SetInGameCoin(int coins){
		ui.coinsText.text = "Coins: " + coins;
	}

	public void resetHomeScreenPlayer(){
		homeScreenPlayer.transform.position = homeScreenPlayerPosition;
	}

	public void Proceed(){
		//saver.ToFile (dungeonLevel);
		analytics.WriteFinishLevel (dungeonLevel, score, totalScore, StartTime);

		achievements.addPlayedTime (Time.time - StartTime);

		RoundEnd ();
		DestroyDungeon ();
		dungeonLevel++;
		if (dungeonLevel > data.maxAchievedDungeonLevel)
			data.maxAchievedDungeonLevel = dungeonLevel;
		
		achievements.levelAchievement (dungeonLevel);

		StartGame ();
	}

	public void MuteAudio(bool newBool){
		audioMuted = newBool;
		audioSourceMusic.mute = newBool;
		if (audioMuted)
			Debug.Log ("Audio is muted");
		else
			Debug.Log ("Audio is unmuted");
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
			loadingScreenCanvas.transform.Find ("LevelText").GetComponent<Text> ().text = "Dungeon level: " + "Tutorial";
			loadingScreenCanvas.transform.Find ("RandomizingTexturesText").gameObject.SetActive(RandomizeTexturesAllowed);
			loadingScreenCanvas.SetActive (true);
			homeScreen.SetActive (false);
			StartCoroutine (CreateLevel (0));
			tutorialStarted = true;
		}
	}

	public void StartArena(){
		if (!arenaStarted) {
			requiredCollectedKeys = 1;
			Time.timeScale = 1f;
			StartTime = Time.time;
			loadingScreenCanvas.transform.Find ("LevelText").GetComponent<Text> ().text = "Dungeon level: " + "Arena";
			loadingScreenCanvas.transform.Find ("RandomizingTexturesText").gameObject.SetActive(RandomizeTexturesAllowed);
			loadingScreenCanvas.SetActive (true);
			homeScreen.SetActive (false);
			StartCoroutine (CreateLevel (2));
			arenaStarted = true;
		}
	}

	public void ExitGame(){
		Application.Quit ();
	}

	public void playUISound(int soundNumber){
		audioSourceUI.clip = audioClipsUI [soundNumber];
		audioSourceUI.Play ();
	}

	public void ToggleShop(){
		if (shop == null) {
			shop = Instantiate (shopPrefab);
			shop.gameObject.SetActive (false);
			shopActive = false;
		}

		if (shopActive) {
			shop.gameObject.SetActive (false);
			shop.shopCam.gameObject.SetActive (false);
			homeScreenCam.SetActive (true);
			homeScreenPlayer.SetActive (true);
		} else {
			shop.gameObject.SetActive (true);
			shop.shopCam.gameObject.SetActive (true);
			homeScreenCam.SetActive (false);
			homeScreenPlayer.SetActive (false);
		}
		shopActive = !shopActive;
	}

	public void ChangePet(GameObject newPet){
		if (Pet != null) {
			Transform positionPet = Pet.transform;
			Destroy (Pet);
			Pet = Instantiate (newPet, positionPet.position, positionPet.rotation) as GameObject;
			PetScript = Pet.GetComponent<Pet>();
		}
	}

	public void ChangeTorch(GameObject newTorch){
		torchPrefab = newTorch.GetComponent<Torch>();
	}

	public void SetDungeonLevel(int dungeonLevel){
		this.dungeonLevel = dungeonLevel;
	}

	public void SetUpDungeonStartCanvas(){
		for (int i = 0; i < 40; i++) {
			GameObject button = Instantiate (dungeonLevelButtonPrefab, dungeonStartCanvas.transform) as GameObject;
			float x = -590f + (i%10) * 130f;
			float y = 200f + ((int)(i/10)) * -130f;
			button.transform.localPosition = new Vector3 (x, y, 0f);
			button.GetComponent<DungeonLevelButton> ().SetDungeonLevel (i + 1);
			button.GetComponentInChildren<Text> ().text = (i + 1).ToString();
			button.SetActive (false);
			dungeonStartCanvas.buttons.Add (button);
		}
		UpdateDungeonStartCanvas ();
	}

	void UpdateDungeonStartCanvas(){
		for (int i = 0; i < data.maxAchievedDungeonLevel; i++) {
			dungeonStartCanvas.buttons [i].SetActive (true);
		}
	}

	public void ToggleDungeonStartCanvas(){
		if (dungeonStartCanvas.gameObject.activeInHierarchy) {
			dungeonStartCanvas.gameObject.SetActive (false);
			homeScreenMovement.enabled = true;
		} else {
			UpdateDungeonStartCanvas ();
			dungeonStartCanvas.gameObject.SetActive (true);
			homeScreenMovement.enabled = false;
		}
	}

	public void addHighQualityItem(GameObject GO){
		highQualityItems.Add (GO);
		GO.SetActive (data.highQuality);
			
	}

	public void addHighQualityItem(GameObject[] GOs){
		foreach (GameObject GO in GOs) {
			addHighQualityItem (GO);
		}
	}

	//highQuality = true and lowQuality = false
	public void setQuality(bool setHighQuality){
		if (!setHighQuality)
			data.highQuality = false;
		else
			data.highQuality = true;

		for (int i = 0; i < highQualityItems.Count; i++) {
			if (highQualityItems [i] == null) {
				highQualityItems.RemoveAt (i);
				Debug.Log ("Removed at index " + i + ". Length: " + highQualityItems.Count);
				i--;
			}
		}

		foreach (GameObject GO in highQualityItems) {
			GO.SetActive (data.highQuality);
		}
	}

	public void KillAllEnemies(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			Destroy (enemy);
		}
		Debug.Log ("All enemies have been killed");
	}

	public void SpawnKey(){
		Instantiate (KeyPrefab, torch.transform.position, Quaternion.identity);
	}

	void RevealMinimap(){
		GameObject dungeon = GameObject.Find ("Dungeon");
		if (dungeon != null) {
			Transform[] temp = dungeon.GetComponentsInChildren<Transform> (true);
			foreach (Transform t in temp) {
				if (LayerMask.LayerToName (t.gameObject.layer).Equals ("Minimap")) {
					t.gameObject.SetActive (true);
				}
			}
			Debug.Log ("I solemnly swear that I am up to no good...");
		}	
	}

	public bool getCheat(){
		return cheat;
	}

	public void toggleCheat(){
		cheat = !cheat;
	}

	void TeleportToHighScores(){
		homeScreenPlayer.transform.position = GameObject.Find ("HighScoreTeleport").transform.position;
	}

	void ApplySkins(){
		foreach (PlayerManager PM in playerManagers) {
			if(data.playerSkin[0] != null) PM.playerSkin.SetCapeSkin (data.playerSkin [0]);
			if(data.playerSkin[1] != null) PM.playerSkin.SetHatSkin (data.playerSkin [1]);
		}
	}

	void ToggleMiniMap(){
		if (ui != null) {
			ui.toggleMinimap ();
		}
	}

	public void SetTextFieldEnabled(bool enabled){
		TextFieldEnabled = enabled;
	}

	public bool GetTextFieldEnabled(){
		return TextFieldEnabled;
	}
}
