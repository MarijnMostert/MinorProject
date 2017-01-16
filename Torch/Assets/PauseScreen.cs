using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

	public GameObject confirmationMainMenu;
	public GameObject confirmationExitGame;
	public GameObject confirmationResetData;
	public GameObject panelCredits;
	public GameObject panelControls;

	public void TurnOffAllPanels(){
		confirmationMainMenu.SetActive (false);
		confirmationExitGame.SetActive (false);
		confirmationResetData.SetActive (false);
		panelCredits.SetActive (false);
		panelControls.SetActive (false);
	}
}
