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
}
