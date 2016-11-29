using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	//Player settings
	public PlayerManager[] playerManagers;
	public GameObject playerPrefab;

	//Torch settings
	public int torchStartingHealth;
	public Torch torch;
	public int torchHealth;
	//public Transform torchSpawnPoint;

	//Score info
	public int score;

	//Target settings
	public GameObject camTarget;
	public GameObject enemyTarget;

	//Dungeon settings
	//##YORAN WIL JIJ HIER DE DUNGEON SETTINGS INVULLEN?


	//public bool paused;
//	public GameObject pauseScreen; //Komt in scene zelf
//	public GameObject cameraPrefab; //Komt in scene zelf

//	public Canvas UI;		//Komt in scene zelf
//	public Spawner spawner;		//Komt in scene zelf

//	private Camera mainCamera;


	void Awake () {

		//Makes sure this object is not deleted when another scene is loaded.
		if (Instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			GameObject.DontDestroyOnLoad (this.gameObject);
			Instance = this;
		}
	}

	void LoadPrefabs(){
		playerPrefab = Resources.Load ("Prefabs/Player", typeof(GameObject)) as GameObject;
		torch = Resources.Load ("Prefabs/Torch", typeof(GameObject)) as Torch;
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Backspace)) {
			SpawnPlayers ();
		} else if (Input.GetKeyUp(KeyCode.F1)){
			SceneManager.LoadScene (0);
			//OnLevelWasLoaded ();
		} else if (Input.GetKeyUp (KeyCode.F2)) {
			SceneManager.LoadScene (1);
			//OnLevelWasLoaded ();
		} else if (Input.GetKeyUp (KeyCode.F3)) {
			SceneManager.LoadScene (2);
		} else if (Input.GetKeyUp (KeyCode.F4)) {
			SceneManager.LoadScene (3);
		} else if (Input.GetKeyUp (KeyCode.F5)) {
			SceneManager.LoadScene (4);
		} else if (Input.GetKeyUp (KeyCode.F6)) {
			SceneManager.LoadScene (5);
		}
		//LoadScene ();
		//Pause ();
	}

	public void UpdateScore(int addedScore){
		score += addedScore;
		//UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
	}

	public void SpawnPlayers(){
		for (int i = 0; i < playerManagers.Length; i++) {
			playerManagers [i].playerInstance = Instantiate (playerPrefab, playerManagers [i].spawnPoint.position, playerManagers [i].spawnPoint.rotation) as GameObject;
			playerManagers [i].playerNumber = i + 1;
			playerManagers [i].Setup ();
		}
	}

	public void SpawnTorch(Transform spawnposition){
		torch = Instantiate (torch, spawnposition) as Torch;
		torch.health = torchStartingHealth;
		torch.gameManager = this;
	}

	public void SaveAll(){
		//Settings moeten worden teruggeschreven naar file of database op de server
		//Moet nog geimplement worden.
	}

	void LoadScene(){
		if (Input.GetKeyUp(KeyCode.F1)){
			SceneManager.LoadScene (0);
			//OnLevelWasLoaded ();
		}
		if (Input.GetKeyUp (KeyCode.F2)) {
			SceneManager.LoadScene (1);
			//OnLevelWasLoaded ();
		}
		if (Input.GetKeyUp (KeyCode.F3)) {
			SceneManager.LoadScene (2);
		}
	}

	public void LoadScene(int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
	}
}

	////// OUDE MEUK ////////
	/*
	void OnLevelWasLoaded(){
		Start ();
	}
	*/

	/*
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
//		UI = Instantiate (UI);
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
//		UI.transform.FindChild ("Death Text").gameObject.SetActive (true);
		torch.gameObject.SetActive (false);
		for (int i = 0; i < playerManagers.Length; i++) {
			playerManagers [i].playerInstance.SetActive (false);
		}
	}

	public void Start(){
		//Initialize ();

		//torch = Instantiate (torch, torchSpawnPoint.position, torchSpawnPoint.rotation) as Torch;
	//	torch.health = torchStartingHealth;
	//	torch.gameManager = this;
//		torch.UI = UI;

		//SetUpCameraPart1 ();

		for (int i = 0; i < playerManagers.Length; i++) {
			playerManagers [i].playerInstance = Instantiate (playerPrefab, playerManagers [i].spawnPoint.position, playerManagers [i].spawnPoint.rotation) as GameObject;
			playerManagers [i].playerNumber = i + 1;
			playerManagers [i].Setup ();

			//playerManagers [i].playerMovement.mainCamera = mainCamera;
		}

	//	camTarget = torch.gameObject;
	//	enemyTarget = torch.gameObject;
		//SetUpCameraPart2 ();
	//	torch.cam = mainCamera;

//		UI.transform.FindChild ("Score Text").GetComponent<Text> ().text = "Score: " + score;
	}
*/

