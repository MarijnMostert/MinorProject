using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject pauseScreen;
	public float slowMotionFactor;
	public AudioSource audiosource;
	public Text audioButtonText;

	private string pauseButton;
	private string slowMotionButton;
	private bool paused;
	private bool slowMotion;
	private bool audioPlaying;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		pauseButton = "Pause";
		slowMotionButton = "SlowMotion";
		paused = false;
		slowMotion = false;
		audioPlaying = true;
	//	pauseScreen = GameObject.FindGameObjectWithTag ("PauseScreen");

	}
	
	// Update is called once per frame
	void Update () {

		//Pauses the game
		if (Input.GetButtonDown (pauseButton)) {
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
		if (Input.GetButtonDown (slowMotionButton)) {
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
