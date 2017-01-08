﻿using UnityEngine;
using System.Collections;

public class PowerUpDecoy: MonoBehaviour, IPowerUp {

	public GameObject Decoy;
	public float spawnHeight = -1.5f;

	public void Use(){
		GameObject decoy = Instantiate(Decoy, transform.parent.position + transform.parent.forward * 2, transform.parent.rotation) as GameObject;
		Vector3 temp = decoy.transform.position;
		temp.y = spawnHeight;
		decoy.transform.position = temp;
	}
}