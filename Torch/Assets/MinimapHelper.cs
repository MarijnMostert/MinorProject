﻿using UnityEngine;
using System.Collections;

public class MinimapHelper : MonoBehaviour {

	public GameObject tile;

	void Start() {
		if (tile == null) {
			tile = transform.parent.Find("Tile").gameObject as GameObject;
		}
		tile.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			action ();
		}
	}

	void OnTriggerStay(Collider other){
		if (other.CompareTag ("Player")) {
			action ();
		}
	}

	void action(){
		tile.SetActive (true);
		this.gameObject.SetActive (false);
	}
}
