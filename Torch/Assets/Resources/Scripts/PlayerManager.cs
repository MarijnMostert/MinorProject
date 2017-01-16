using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerManager {

	public Transform spawnPoint;
	public Color playerColor;

	//public GameObject UI;

	[HideInInspector] public GameObject cursorPointer;
	[HideInInspector] public int playerNumber;
	[HideInInspector] public GameObject playerInstance;
	[HideInInspector] public bool dead;
	[HideInInspector] public PlayerMovement playerMovement;
	[HideInInspector] public PlayerWeaponController playerWeaponController;
	[HideInInspector] public PowerUpInventory powerUpInventory;
	[HideInInspector] public bool movementEnabled = true;
	[HideInInspector] public bool active = true;
	[HideInInspector] public PlayerData playerData;
	[HideInInspector] public GameManager gameManager;

	public void Setup () {
		cursorPointer = Resources.Load ("Prefabs/Cursor Pointer", typeof(GameObject)) as GameObject;
		dead = false;

		//Setup movement script
		playerMovement = playerInstance.GetComponent<PlayerMovement> ();
        playerMovement.setMoves(playerNumber);
		playerMovement.cursorPointerPrefab = cursorPointer;
		playerMovement.playerColor = playerColor;
		playerMovement.distanceTravelled = 0f;

		//Setup Inventory references
		powerUpInventory = playerInstance.GetComponent<PowerUpInventory> ();
		powerUpInventory.playerNumber = playerNumber;
		GameManager.Instance.uiInventory.SetColor (playerNumber, playerColor);

		//Setup weapon controller script
		playerWeaponController = playerInstance.GetComponent<PlayerWeaponController>();
		playerWeaponController.setNumber(playerNumber);
		playerData = playerInstance.GetComponent<PlayerData> ();
		active = true;
		movementEnabled = true;
	}

	void Update(){
		if(Input.GetButton("Reset")){
			Reset();
		}
	}

	public void Reset(){
		playerInstance.SetActive (false);
		playerInstance.transform.position = spawnPoint.position;
		playerInstance.transform.rotation = spawnPoint.rotation;
		playerInstance.SetActive (true);
	}

	public void EnableMovement(bool Bool){
		if (Bool) {
			//Enable player movement
			playerMovement.enabled = true;
			playerWeaponController.enabled = true;
			movementEnabled = true;
		} else {
			//Disable player movement
			playerMovement.enabled = false;
			playerWeaponController.enabled = false;
			movementEnabled = false;
		}
	}

	public void Enable(bool Bool){
		if (Bool) {
			//Enable player
			EnableMovement (true);
			Vector3 spawnLocation = gameManager.torch.transform.position;
			spawnLocation.y = .5f;
			playerInstance.transform.position = spawnLocation;
			playerInstance.SetActive(true);

			if (playerNumber == 2) {
				ToggleGeneralControllerButtons (true);
			}
			active = true;
		} else {
			//Disable player
			if (playerInstance.gameObject.GetComponentInChildren<Torch> () != null) {
				gameManager.torch.torchPickUp.releaseTorch ();
			}
			EnableMovement (false);
			playerInstance.SetActive(false);

			//Set the general controller buttons to inactive, only if player 1 uses keyboard
			if (playerNumber == 2 && !gameManager.playerManagers [0].playerMovement.controllerInput) {
				ToggleGeneralControllerButtons (false);
			}
			active = false;
		}

	}

	void ToggleGeneralControllerButtons(bool Enable){
		foreach (Image img in gameManager.uiInventory.ControllerButtonsGeneral) {
			img.gameObject.SetActive (Enable);
		}
	}
}
