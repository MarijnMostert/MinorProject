using UnityEngine;
using System.Collections;

public class FellTroughHomeScreen : MonoBehaviour {

	Transform originalplayer;
	Transform originalcamera;
	GameObject cam;

	void Start () {
		cam = GameObject.Find ("HomeScreenCam");
		originalplayer = GameObject.Find ("HomeScreenPlayer").transform;
		originalcamera = cam.transform;
	}

	void OnTriggerEnter (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag("Player")) {
			player.transform.position = originalplayer.position;
			player.transform.rotation = originalplayer.rotation;
		}
	}

}
