﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	public int controllerInput; //0 is mouseinput, 1 is controllerinput
	public int highscore;

	public string moveHorizontalAxis;
	public string moveVerticalAxis;
	public string turnHorizontalAxis;
	public string turnVerticalAxis;
	public string interactionButton;
	public string attackButton;
	public string pauseButton;
	public string slowmotionButton;
	public string spawnEnemyButton;
	public string spawnWaveButton;
	public string cameraShakeButton;
	public string saveButton;

	// Use this for initialization
	void Start () {
		LoadAll ();
		SetUpControlsFirstTime ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)){
			SetUpControlsFirstTime();
		}
		if(Input.GetButtonDown(saveButton)){
			SaveAll();
		}
	}

	void SaveAll(){
		PlayerPrefs.SetInt ("controllerInput", controllerInput);
		PlayerPrefs.SetInt ("highscore", highscore);
		PlayerPrefs.SetString ("moveHorizontalAxis", moveHorizontalAxis);
		PlayerPrefs.SetString ("moveVerticalAxis", moveVerticalAxis);
		PlayerPrefs.SetString ("turnHorizontalAxis", turnHorizontalAxis);
		PlayerPrefs.SetString ("turnVerticalAxis", turnVerticalAxis);
		PlayerPrefs.SetString ("interactionButton", interactionButton);
		PlayerPrefs.SetString ("attackButton", attackButton);
		PlayerPrefs.SetString ("pauseButton", pauseButton);
		PlayerPrefs.SetString ("slowmotionButton", slowmotionButton);
		PlayerPrefs.SetString ("spawnEnemyButton", spawnEnemyButton);
		PlayerPrefs.SetString ("spawnWaveButton", spawnWaveButton);
		PlayerPrefs.SetString ("cameraShakeButton", cameraShakeButton);
		PlayerPrefs.SetString ("saveButton", saveButton);
	}

	void LoadAll(){
		controllerInput = PlayerPrefs.GetInt ("controllerInput");
		highscore = PlayerPrefs.GetInt ("highscore");
		moveHorizontalAxis = PlayerPrefs.GetString ("moveHorizontalAxis");
		moveVerticalAxis = PlayerPrefs.GetString ("moveVerticalAxis");
		turnHorizontalAxis = PlayerPrefs.GetString ("turnHorizontalAxis");
		turnVerticalAxis = PlayerPrefs.GetString ("turnVerticalAxis");
		interactionButton = PlayerPrefs.GetString ("interactionButton");
		attackButton = PlayerPrefs.GetString ("attackButton");
		pauseButton = PlayerPrefs.GetString ("pauseButton");
		slowmotionButton = PlayerPrefs.GetString ("slowmotionButton");
		spawnEnemyButton = PlayerPrefs.GetString ("spawnEnemyButton");
		spawnWaveButton = PlayerPrefs.GetString ("spawnWaveButton");
		cameraShakeButton = PlayerPrefs.GetString("cameraShakeButton");
		saveButton = PlayerPrefs.GetString ("saveButton");
	}

	public void changeControllerInput(){
		if (controllerInput == 0) {
			moveHorizontalAxis = "MovingControllerHorizontal1";
			moveVerticalAxis = "MovingControllerVertical1";
			turnHorizontalAxis = "TurningControllerHorizontal1";
			turnVerticalAxis = "TurningControllerVertical1";
			interactionButton = "InteractionButton";
			attackButton = "Attack1";
			pauseButton = "Pause";
			slowmotionButton = "SlowMotion";
			spawnEnemyButton = "Spawn Enemy Button";
			spawnWaveButton = "Spawn Wave Button";
			cameraShakeButton = "CameraShake";
			saveButton = "Save Button";
			controllerInput = 1;


		} else if (controllerInput == 1) {
			moveHorizontalAxis = "Horizontal1";
			moveVerticalAxis = "Vertical1";
			turnHorizontalAxis = null;
			turnVerticalAxis = null;
			interactionButton = "InteractionButton";
			attackButton = "Attack1";
			pauseButton = "Pause";
			slowmotionButton = "SlowMotion";
			spawnEnemyButton = "Spawn Enemy Button";
			spawnWaveButton = "Spawn Wave Button";
			cameraShakeButton = "CameraShake";
			saveButton = "Save Button";
			controllerInput = 0;
		}
			
	}

	public void SetUpControlsFirstTime(){
		changeControllerInput ();
		changeControllerInput ();
		SaveAll ();
	}

}
