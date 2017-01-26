using UnityEngine;
using System.Collections;

public class IfIntoMountain : MonoBehaviour {

	public HomeScreenMovement homescreencam;

	void Update () {
		if (homescreencam == null) {
			homescreencam = GameObject.Find ("HomeScreenCam").GetComponent<HomeScreenMovement> ();
		}
	}

	void OnTriggerStay (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			float ything = player.transform.rotation.eulerAngles.y;
			if ((ything > 110 && ything < 230) || (ything > -250 && ything < -130)) {
				homescreencam.minimalheight = 4.0f;
				homescreencam.maximalheight = 5.0f;
			}
		}
	}

	void OnTriggerExit (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			homescreencam.minimalheight = -2.0f;
			homescreencam.maximalheight = 2.5f;
		}
	}

}
