using System;
using UnityEngine;

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
	[HideInInspector] public bool movementEnabled = true;
	[HideInInspector] public bool active = true;
	[HideInInspector] public GameManager gameManager;

	public void Setup () {
		cursorPointer = Resources.Load ("Prefabs/Cursor Pointer", typeof(GameObject)) as GameObject;
		dead = false;
	//	UI = Instantiate (UI);
		playerInstance.GetComponent<MeshRenderer> ().material.color = playerColor;
	//	cursorPointer = Instantiate (cursorPointer, playerInstance.transform) as Sprite;

		//Setup movement script
		playerMovement = playerInstance.GetComponent<PlayerMovement> ();
        playerMovement.setMoves(playerNumber);
		playerMovement.cursorPointer = cursorPointer;
		//Setup weapon controller script
		playerWeaponController = playerInstance.GetComponent<PlayerWeaponController>();
		playerWeaponController.setNumber(playerNumber);
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
			playerMovement.cursorPointer.SetActive (true);
			Vector3 spawnLocation = gameManager.torch.transform.position;
			spawnLocation.y = .5f;
			playerInstance.transform.position = spawnLocation;
			playerInstance.SetActive(true);
			active = true;
		} else {
			//Disable player
			if (playerInstance.gameObject.GetComponentInChildren<Torch> () != null) {
				gameManager.torch.releaseTorch ();
			}
			EnableMovement (false);
			playerMovement.cursorPointer.SetActive (false);
			playerInstance.SetActive(false);
			active = false;
		}

	}
}
