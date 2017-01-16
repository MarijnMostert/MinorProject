using UnityEngine;
using System.Collections;

public class IfIntoMountain : MonoBehaviour {

	public HomeScreenMovement homescreencam;
	bool isset = false;

	void OnTriggerStay (Collider other) {
		GameObject player = other.gameObject;
		if (player.CompareTag ("Player")) {
			float ything = player.transform.rotation.eulerAngles.y;
			Debug.Log (ything);
			if (ything > 110 && ything < 230) {
				homescreencam.minimalheight = 4.0f;
				homescreencam.maximalheight = 5.0f;
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
