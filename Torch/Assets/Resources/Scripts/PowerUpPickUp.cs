﻿using UnityEngine;
using System.Collections;

public class PowerUpPickUp : MonoBehaviour, IPickUp {

	public GameObject PowerUp;

	public void OnTriggerEnter(Collider collider){
		if(collider.gameObject.CompareTag("Player")){
			GameObject player = collider.gameObject;
			if (!player.GetComponent<Inventory> ().isFull ()) {
				player.GetComponent<Inventory> ().AddItemToInventory (PowerUp, player);
				Destroy (gameObject);
			}
		}
	}
}
