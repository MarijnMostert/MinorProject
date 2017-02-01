using UnityEngine;
using UnityEngine.UI;
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
	public Text name;

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

	public void Start(){
		name.text = PlayerPrefs.GetString ("name");
	}

	public void PlayUISound(){
		GameManager.Instance.playUISound (0);
	}

	public void SetNumberOfPlayers(int number){
		GameManager.Instance.SetNumberOfPlayers (number);
	}

	public void MuteAudio(bool mute){
		GameManager.Instance.MuteAudio (mute);
	}

	public void toggleRandomizeTexturesAllowed (bool newBool) {
		GameManager.Instance.toggleRandomizeTexturesAllowed (newBool);
	}

	public void InvertY(bool newBool){
		GameManager.Instance.homeScreenMovement.InvertY (newBool);
	}

	public void SetQuality(bool highQuality){
		GameManager.Instance.setQuality (highQuality);
	}
}
