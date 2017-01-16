using UnityEngine;
using System.Collections;

public class IfFallingOfMountain : MonoBehaviour {

	public HomeScreenMovement homescreencam;
	bool isset = false;

	void OnTriggerStay (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			float ything = player.transform.rotation.eulerAngles.y;
			if (((ything > 270 && ything < 360) || (ything > 0 && ything < 30))) {
				homescreencam.minimalheight = 7.0f;
				homescreencam.maximalheight = 8.0f;
				Debug.Log ("Height set");
				isset = true;
			}
		}
	}



	void OnTriggerExit (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			homescreencam.minimalheight = -1.0f;
			homescreencam.maximalheight = 3.5f;
			isset = false;
		}
	}

}
