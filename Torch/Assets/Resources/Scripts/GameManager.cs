using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	//Player data
	public static GameManager Instance;
	public PlayerManager[] playerManagers;
	public GameObject playerPrefab;

	//Torch data
	public GameObject torchObject;
	public Torch torch;
	public Transform torchSpawnPoint;
	public int torchStartingHealth = 100;
	public int torchHealth;
	public int torchHealthMax = 150;

	//Score info
	public int score = 0;

	public bool paused;
	public GameObject pauseScreen;
	public GameObject cameraPrefab;
	public GameObject camTarget;
	public GameObject enemyTarget;
	public GameObject UI;
    //public Spawner spawner;

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
	public GameObject homeScreen;
    public Camera mainCamera;
    MasterGenerator masterGenerator;

	public AudioSource audioSource;
	public AudioClip audioHomeScreen;
	public AudioClip audioDungeon;


    void Awake () {
		//Makes sure this object is not deleted when another scene is loaded.
		if (Instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			GameObject.DontDestroyOnLoad (this.gameObject);
			Instance = this;
		}
		//homeScreenCanvas = GameObject.Find ("Home Screen Canvas");
		homeScreen = GameObject.Find ("HomeScreen");
		audioSource = GetComponent<AudioSource> ();
    }

    public void Start(){
		
	}

	public void StartGame(){
		audioSource.clip = audioDungeon;
		audioSource.Play ();
		homeScreen.SetActive (false);
		//homeScreenCanvas.SetActive (false);
		loadingScreenCanvas.SetActive (true);

		masterGenerator = new MasterGenerator(this.gameObject, width, height, radius, maxlength, timeout, minAmountOfRooms, maxAmountOfRooms, chanceOfRoom);
		masterGenerator.LoadPrefabs();
		masterGenerator.Start();
		pauseScreen = masterGenerator.pause_screen;
		UI = Instantiate (UI);
		torch = torchObject.GetComponent<Torch>();
		camTarget = torchObject;
		enemyTarget = torchObject;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.UI = UI;

		for (int i = 0; i < playerManagers.Length; i++) {
			Debug.Log("Create Player with id:" + i);
			//playerManagers [i].playerInstance = Instantiate (playerPrefab, playerManagers [i].spawnPoint.position, playerManagers [i].spawnPoint.rotation) as GameObject;
			playerManagers[i].playerInstance = Instantiate(playerPrefab, masterGenerator.dungeon_instantiate.startPos, playerManagers[i].spawnPoint.rotation) as GameObject;
			playerManagers [i].playerNumber = i + 1;
			playerManagers [i].Setup ();
			playerManagers [i].playerMovement.mainCamera = mainCamera;
		}

		torch.cam = mainCamera;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;

		loadingScreenCanvas.SetActive (false);
	}
	
	void Update () {
		//LoadScene ();
		Pause ();
	}

	void OnLevelWasLoaded(){
		//Start ();
	}

	void LoadScene(){
		if (Input.GetKeyUp(KeyCode.Alpha0)){
			SceneManager.LoadScene (0);
			OnLevelWasLoaded ();
		}
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			SceneManager.LoadScene (1);
			OnLevelWasLoaded ();
		}
		if (Input.GetKeyUp (KeyCode.Alpha2)) {
			SceneManager.LoadScene (2);
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

	void Initialize(){
		/*pauseScreen = Instantiate (pauseScreen);
		pauseScreen.SetActive (false);
        pauseScreen.GetComponentInChildren<MuteAudio>().game_manager = this.gameObject;
		UI = Instantiate (UI);
		spawner = Instantiate (spawner);*/
	}
/*
	void SetUpCameraPart1(){
		cameraPrefab = Instantiate (cameraPrefab) as GameObject;
		mainCamera = cameraPrefab.GetComponentInChildren<Camera> ();
	}

	void SetUpCameraPart2(){
		cameraPrefab.GetComponentInChildren<CameraController> ().target = camTarget;
	}*/

	public void GameOver(){
		UI.transform.FindChild ("Death Text").gameObject.SetActive (true);
		torch.gameObject.SetActive (false);
		for (int i = 0; i < playerManagers.Length; i++) {
			playerManagers [i].playerInstance.SetActive (false);
		}
	}

	public void updateScore(int addedScore){
		score += addedScore;
		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
	}
}
