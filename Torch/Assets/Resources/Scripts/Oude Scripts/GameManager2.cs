using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager2 : MonoBehaviour {

	public GameObject pauseScreen;
	public float slowMotionFactor;
	public AudioSource audiosource;
	public Text audioButtonText;

	private string pauseButton;
	private string slowMotionButton;
	private bool paused;
	private bool slowMotion;
	private bool audioPlaying;

	private PlayerPrefsManager ppM;

	void Awake(){
		pauseScreen = GameObject.Find ("UI").transform.FindChild ("Pause Screen").gameObject;
		if (pauseScreen == null)
			Debug.Log ("No pausescreen is found. Add UI Prefab to the scene");
		audioButtonText = pauseScreen.transform.FindChild ("Audio Button").gameObject.GetComponentInChildren<Text>();
		if (audioButtonText == null)
			Debug.Log ("No audio button is found. Add UI Prefab to the scene");
		ppM = GameObject.Find ("SceneManager").GetComponent<PlayerPrefsManager> ();
		if (ppM == null)
			Debug.Log ("No Player Preferences Manager is found. Add Scene Manager Prefab to the scene.");
		audiosource = GetComponent<AudioSource> ();
	}

	void Start () {
		paused = false;
		slowMotion = false;
		audioPlaying = true;

	}
	
	void Update () {

		//Pauses the game
		if (Input.GetButtonDown (ppM.pauseButton)) {
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

		//Turns on slowmotion mode
		if (Input.GetButtonDown (ppM.slowmotionButton)) {
			if (!slowMotion) {
				Time.timeScale = slowMotionFactor;
				slowMotion = true;
			} else {
				Time.timeScale = 1;
				slowMotion = false;
			}
		}
	}

	//(Un)mutes audio when called
	public void MuteAudio(){
		if (audioPlaying) {
			audiosource.mute = true;
			audioButtonText.text = "Audio muted";
			audioPlaying = false;
		} else {
			audiosource.mute = false;
			audioButtonText.text = "Audio playing";
			audioPlaying = true;
		}
	}
}
