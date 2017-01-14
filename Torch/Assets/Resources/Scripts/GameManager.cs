using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

	//Stored player preferences and info
	public Data data;
	public DungeonData dungeonData;

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
	public Transform torchSpawnPoint;
	public int torchStartingHealth = 100;
	public int torchHealth;
	public int torchHealthMax = 150;

	//Score info
	public int score = 0;
	public int totalScore = 0;
	public int coinsInGame = 0;
	public int dungeonLevel = 0;
	public float StartTime;
	public string Roomtype;

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

	public List<GameObject> PuzzleRooms;

	public GameObject TorchFOVPrefab;
	private GameObject TorchFOV;

	public Spawner spawner;
	public GameObject triggerFloorPrefab;
	private GameObject triggerFloorObject;

	public ProceduralMaterial[] substances;

	private bool cheat;
	int cheatindex;
	private string[] cheatCode;

    //masterGenerator Vars
    int radius = 2;// = 2;
    int maxlength = 2;// = 2;
    int timeout = 200;// = 2000;


	//public GameObject homeScreenCanvas;
	public GameObject homeScreenEnvironmentPrefab;
	public GameObject homeScreenEnvironment;
	public GameObject startingScreen;
	public GameObject loadingScreenCanvas;
	public DeathCanvas deathCanvas;
	public GameObject endOfRoundCanvas;
	public DungeonStartCanvas dungeonStartCanvas;
	public GameObject dungeonLevelButtonPrefab;
	private GameObject homeScreen;
	private GameObject homeScreenCam;
	public Camera mainCamera;
	public Camera minimap;
	private Vector3 homeScreenPlayerPosition;
    MasterGenerator masterGenerator;
	[HideInInspector] public Transform levelTransform;
    bool gameStarted;
	bool tutorialStarted;
	public GameObject tutorialPrefab;
	private GameObject tutorialObject;
	public GameObject tutorialTorchPrefab;
	private GameObject tutorialTorchObject;

	[Header("- Audio")]
	public AudioSource audioSourceMusic;
	public AudioSource audioSourceUI;
	public AudioClip[] audioClipsUI;
	public AudioClip loreAudio;
	public AudioClip audioHomeScreen;
	public AudioClip[] audioDungeon;
	public AudioClip audioPartyTorch;
	public bool audioMuted;

	[Header("- Debugging properties")]
	public GameObject DebuggerPanel;
	public GameObject[] allWeaponsAvailable;
	public GameObject[] allPowerUpsAvailable;
	public GameObject KeyPrefab;

	public int collectedKeys;
	public int requiredCollectedKeys;
	public GameObject Pet;
	private Pet PetScript;

	public Shop shop;
	private bool shopActive = false;

	public List<GameObject> highQualityItems;

	[HideInInspector] public int numberOfPlayers = 1;

    void Awake () {
		data.LoadFileToDataAndVars ();
		setQuality (data.highQuality);

		dungeonData = GetComponent<DungeonData> ();

		startingScreen.SetActive (true);

		DebuggerPanel = Instantiate (DebuggerPanel);
		DebuggerPanel.SetActive (false);
	

		shop = Instantiate (shop);
		shop.gameObject.SetActive (false);
		shopActive = false;

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

		cheat = true; //put this to false in build
		cheatindex = 0;
		cheatCode = new string[] { "w", "o", "c", "h", "e", "n", "e", "n", "d", "e" };

		//homeScreenCanvas = GameObject.Find ("Home Screen Canvas");
		homeScreen = GameObject.Find ("HomeScreen");
		homeScreenCam = GameObject.Find ("HomeScreenCam");
		mainCamera = homeScreenCam.GetComponent<Camera> ();
		minimap = Resources.Load ("Prefabs/minimap2", typeof (Camera)) as Camera;
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
	}

	public void StartGame(){
        if (!gameStarted) {
			Time.timeScale = 1f;
			endOfRoundCanvas.SetActive (false);
			loadingScreenCanvas.transform.Find ("LevelText").GetComponent<Text> ().text = "Dungeon level: " + (dungeonLevel).ToString();
            loadingScreenCanvas.SetActive(true);
			Destroy (homeScreenEnvironment);
			homeScreen.SetActive (false);
            StartCoroutine(CreateLevel(1));
            gameStarted = true;
			StartCoroutine (WaitSpawning ());
		}
	}

	IEnumerator WaitSpawning(){
		float wait = 11.0f - .5f * dungeonLevel;
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
			masterGenerator = new MasterGenerator (this.gameObject, dungeonData.dungeonParameters[dungeonLevel], radius, maxlength, timeout, PuzzleRooms);
			masterGenerator.LoadPrefabs ();
			masterGenerator.Start ();
		} else if (type == 0) {
			tutorialObject = Instantiate(tutorialPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			levelTransform = tutorialObject.transform;

		}

		if (ui == null) {
			ui = Instantiate (UIPrefab);
			uiInventory = ui.uiInventory;
		}

		TorchFOV = Instantiate (TorchFOVPrefab);

		triggerFloorObject = Instantiate (triggerFloorPrefab, levelTransform) as GameObject;

		torch = Instantiate (torchPrefab) as Torch;

		camTarget = torch.gameObject;
		enemyTarget = torch.gameObject;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.ui = ui;
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
		RespawnPosition = startpoint;
		torch.transform.position = startpoint + new Vector3 (6, .5f, 0);
		Pet.transform.position = startpoint;

		torch.torchPickUp.cam = mainCamera;
		if (type == 1) {
			ui.dungeonLevelText.text = "Dungeon level " + dungeonLevel;
		} else if (type == 0) {
			ui.dungeonLevelText.text = "Dungeon Tutorial";
			PetScript.speechText.text = "Welcome to this tutorial! My name is Bold. Use the WASD-keys to move.";
			PetScript.speechImage.gameObject.SetActive (true);
		}

		audioSourceMusic.clip = audioDungeon [UnityEngine.Random.Range (0, audioDungeon.Length)];
		if (torch.GetComponent<PartyTorch> () != null) {
			audioSourceMusic.clip = audioPartyTorch;
		}
		audioSourceMusic.Play ();
		score = 0;
		totalScore = 0;
		coinsInGame = 0;
		SetScore (score);
		SetInGameCoin (coinsInGame);
		homeScreenCam.SetActive (false);
		loadingScreenCanvas.SetActive (false);

		Instantiate (minimap);

		StartTime = Time.time;

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
			cheat = true;
		}

		if (Input.GetKeyDown (KeyCode.H) && cheat == true) {
			if (DebuggerPanel != null) {
				if (DebuggerPanel.activeInHierarchy)
					DebuggerPanel.SetActive (false);
				else
					DebuggerPanel.SetActive (true);
			}
		}

		//Cheatcode to spawn all weapons around the torch
		if (Input.GetKeyDown (KeyCode.N) && cheat) {
			SpawnAllWeapons ();
		}
		//Cheatcode to spawn all powerups around the torch
		if (Input.GetKeyDown (KeyCode.B) && cheat) {
			SpawnAllPowerUps ();
		}
		//Cheatcode to proceed to the next level
		if (Input.GetKeyDown (KeyCode.L) && cheat) {
			Proceed ();
		}
		//Cheatcode to get full health
		if (Input.GetKeyDown (KeyCode.K) && torch != null && cheat) {
			torch.HealToStartingHealth ();
		}
		//Cheatcode to toggle if the torch is damagable or not
		if (Input.GetKeyDown (KeyCode.J) && torch != null && cheat) {
			torch.ToggleDamagable ();
		}
		//Cheatcode to kill all active enemies
		if(Input.GetKeyDown(KeyCode.LeftBracket) && cheat){
			KillAllEnemies();
		}

		if (Input.GetKeyDown (KeyCode.O) && cheat) {
			SpawnKey ();
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
		totalScore += score;
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

		score = 0;

	}

	public void TransitionDeathToMain(){
		RoundEnd ();
		DestroyDungeon ();
		if (ui != null)
			Destroy (ui.gameObject);
		if (triggerFloorObject != null)
			Destroy (triggerFloorObject);
		LoadHomeScreen ();
		if (paused)
			Pause ();
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
		audioSourceMusic.clip = audioHomeScreen;
		audioSourceMusic.Play ();
		resetHomeScreenPlayer ();
	}

	public void updateScore(int addedScore){
		score += addedScore;
		ui.scoreText.text = "Score: " + score;
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
		saver.ToFile (dungeonLevel);
		analytics.WriteFinishLevel (dungeonLevel, score, totalScore, StartTime);
		RoundEnd ();
		DestroyDungeon ();
		dungeonLevel++;
		if (dungeonLevel > data.maxAchievedDungeonLevel)
			data.maxAchievedDungeonLevel = dungeonLevel;
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

	public void ExitGame(){
		Application.Quit ();
	}

	public void playUISound(int soundNumber){
		audioSourceUI.clip = audioClipsUI [soundNumber];
		audioSourceUI.Play ();
	}

	public void ToggleShop(){
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

	void SetUpDungeonStartCanvas(){
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
	}

	public void addHighQualityItem(GameObject[] GOs){
		foreach (GameObject GO in GOs) {
			addHighQualityItem (GO);
		}
	}

	//highQuality = true and lowQuality = false
	public void setQuality(bool setHighQuality){
		data.highQuality = false;
		if (!setHighQuality) {
			data.highQuality = false;
		} else if (setHighQuality) {
			data.highQuality = true;
		}

		foreach (GameObject GO in highQualityItems) {
			if (GO == null) {
				highQualityItems.Remove (GO);
			}
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

	public bool getCheat(){
		return cheat;
	}
}
