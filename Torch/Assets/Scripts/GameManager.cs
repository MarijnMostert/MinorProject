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
	public Torch torch;
	public Transform torchSpawnPoint;
	public int torchStartingHealth;
	public int torchHealth;

	//Score info
	public int score = 0;

	public bool paused;
	public GameObject pauseScreen;
	public GameObject cameraPrefab;
	public GameObject camTarget;
	public GameObject enemyTarget;
	public Canvas UI;
	public Spawner spawner;

	private Camera mainCamera;


	void Awake () {

		//Makes sure this object is not deleted when another scene is loaded.
		if (Instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			GameObject.DontDestroyOnLoad (this.gameObject);
			Instance = this;
		}
	}

	public void Start(){
		Initialize ();

		torch = Instantiate (torch, torchSpawnPoint.position, torchSpawnPoint.rotation) as Torch;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
		torch.UI = UI;

		SetUpCameraPart1 ();
		for (int i = 0; i < playerManagers.Length; i++) {
			playerManagers [i].playerInstance = Instantiate (playerPrefab, playerManagers [i].spawnPoint.position, playerManagers [i].spawnPoint.rotation) as GameObject;
			playerManagers [i].playerNumber = i + 1;
			playerManagers [i].Setup ();
			playerManagers [i].playerMovement.mainCamera = mainCamera;
		}

		camTarget = torch.gameObject;
		enemyTarget = torch.gameObject;
		SetUpCameraPart2 ();
		torch.cam = mainCamera;

		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
	}
	
	void Update () {
		LoadScene ();
		Pause ();
	}

	void OnLevelWasLoaded(){
		Start ();
	}

	void LoadScene(){
		if (Input.GetKeyUp(KeyCode.F1)){
			SceneManager.LoadScene (0);
			OnLevelWasLoaded ();
		}
		if (Input.GetKeyUp (KeyCode.F2)) {
			SceneManager.LoadScene (1);
			OnLevelWasLoaded ();
		}
		if (Input.GetKeyUp (KeyCode.F3)) {
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
		pauseScreen = Instantiate (pauseScreen);
		pauseScreen.SetActive (false);
		UI = Instantiate (UI);
		spawner = Instantiate (spawner);
	}

	void SetUpCameraPart1(){
		cameraPrefab = Instantiate (cameraPrefab) as GameObject;
		mainCamera = cameraPrefab.GetComponentInChildren<Camera> ();
	}

	void SetUpCameraPart2(){
		cameraPrefab.GetComponentInChildren<CameraController> ().target = camTarget;
	}

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
