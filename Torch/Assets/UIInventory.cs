using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInventory : MonoBehaviour {

	public static UIInventory Instance;
	private GameManager gameManager;
	public WeaponInventory weaponInventory;

	public Image[] WeaponBlocks;
	public Image[] WeaponImages;
	public Image[] PowerUpBlocksP1;
	public Image[] PowerUpImagesP1;
	public Image[] PowerUpBlocksP2;
	public Image[] PowerUpImagesP2;

	public Image[] KeyboardButtonsGeneral;
	public Image[] ControllerButtonsGeneral;
	public Image[] KeyboardButtonsP1;
	public Image[] ControllerButtonsP1;
	public Image[] ControllerButtonsP2;

	public Image IndicatorP1;
	public Image IndicatorP2;


	void Start(){
		Instance = this;
		gameManager = GameManager.Instance;
		weaponInventory = GetComponent<WeaponInventory> ();
	}

	//Set up the color of the powerup blocks.
	public void SetColor(int playerNumber, Color playerColor){
		Image[] imageArray = new Image[0];
		if (playerNumber == 1) {
			imageArray = PowerUpBlocksP1;
			IndicatorP1.color = playerColor;
		} else if (playerNumber == 2) {
			imageArray = PowerUpBlocksP2;
			IndicatorP2.color = playerColor;
		}

		foreach (Image image in imageArray) {
			image.color = playerColor;
		}
	}

	//Display only the blocks that are needed.
	public void SetNumberOfPlayers(int numberOfPlayers){
		bool MultiPlayer = false;
		if (numberOfPlayers == 1) {
			MultiPlayer = false;
		} else if (numberOfPlayers == 2) {
			MultiPlayer = true;
		}

		//Disable/Enable all images that concern player 2.
		foreach (Image image in PowerUpBlocksP2) {
			image.gameObject.SetActive (MultiPlayer);
		}
		foreach (Image image in PowerUpImagesP2) {
			image.gameObject.SetActive (MultiPlayer);
		}
		foreach (Image image in ControllerButtonsP2) {
			image.gameObject.SetActive (MultiPlayer);
		}

		IndicatorP2.gameObject.SetActive (MultiPlayer);
	}
}
