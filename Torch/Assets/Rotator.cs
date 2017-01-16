using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	private string interactionButton;
	public bool is_on = false;

	void Update () {
		if (is_on) {
			transform.Rotate(0, 5, 0);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (Input.GetButtonDown (interactionButton)) {
				is_on = true;
			} 
			else {
				is_on = false;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			is_on = false;
		}
	}

}
