using UnityEngine;
using System.Collections;

public class IfFallingOfMountain : MonoBehaviour {

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
			if (((ything > 270 && ything < 360) || (ything > 0 && ything < 30)) 
				|| ((ything > -90 && ything < 0) || (ything > -360 && ything < 330))) {
				homescreencam.minimalheight = 7.0f;
				homescreencam.maximalheight = 8.0f;
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
