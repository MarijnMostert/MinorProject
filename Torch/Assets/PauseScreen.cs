﻿using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

	public GameObject confirmationMainMenu;
	public GameObject confirmationExitGame;
	public GameObject confirmationResetData;
	public GameObject confirmationLogOut;
	public GameObject panelCredits;
	public GameObject panelControls;
	public GameObject panelOptions;
	public GameObject panelAchievements;

	public void TurnOffAllPanels(){
		confirmationMainMenu.SetActive (false);
		confirmationExitGame.SetActive (false);
		confirmationResetData.SetActive (false);
		confirmationLogOut.SetActive (false);
		panelCredits.SetActive (false);
		panelControls.SetActive (false);
		panelAchievements.SetActive (false);
		panelOptions.SetActive (false);
	}

	public void PlayUISound(){
		GameManager.Instance.playUISound (0);
	}
}
