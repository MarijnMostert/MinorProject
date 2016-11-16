using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject pauseScreen;
	public float slowMotionFactor;

	private string pauseButton;
	private string slowMotionButton;
	private bool paused;
	private bool slowMotion;

	// Use this for initialization
	void Start () {
		pauseButton = "Pause";
		slowMotionButton = "SlowMotion";
		paused = false;
		slowMotion = false;
	}
	
	// Update is called once per frame
	void Update () {
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
}
