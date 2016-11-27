using System;
using UnityEngine;

[Serializable]
public class PlayerManager {

	public Transform spawnPoint;
	public Color playerColor;
	public GameObject cursorPointer;
	public GameObject UI;

	[HideInInspector] public int playerNumber;
	[HideInInspector] public GameObject playerInstance;
	[HideInInspector] public bool dead;
	[HideInInspector] public PlayerMovement playerMovement;
	[HideInInspector] public PlayerWeaponController playerWeaponController;

	public void Setup () {
		dead = false;
	//	UI = Instantiate (UI);
		playerInstance.GetComponent<MeshRenderer> ().material.color = playerColor;
	//	cursorPointer = Instantiate (cursorPointer, playerInstance.transform) as Sprite;

		//Setup movement script
		playerMovement = playerInstance.GetComponent<PlayerMovement> ();
		playerMovement.playerNumber = playerNumber;
		playerMovement.cursorPointer = cursorPointer;

		//Setup weapon controller script
		playerWeaponController = playerInstance.GetComponent<PlayerWeaponController>();
		playerWeaponController.playerNumber = playerNumber;
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
