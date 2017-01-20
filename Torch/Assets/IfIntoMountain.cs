using UnityEngine;
using System.Collections;

public class IfIntoMountain : MonoBehaviour {

	public HomeScreenMovement homescreencam;
	bool isset = false;

	void Update () {
		if (homescreencam == null) {
			homescreencam = GameObject.Find ("HomeScreenCam").GetComponent<HomeScreenMovement> ();
		}
	}

	void OnTriggerStay (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			float ything = player.transform.rotation.eulerAngles.y;
			if (ything > 110 && ything < 230) {
				homescreencam.minimalheight = 4.0f;
				homescreencam.maximalheight = 5.0f;
				isset = true;
			}
		}
	}



	void OnTriggerExit (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			homescreencam.minimalheight = -2.0f;
			homescreencam.maximalheight = 2.5f;
			isset = false;
		}
	}

}
