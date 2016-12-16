using UnityEngine;
using System.Collections;

public class TriggerActivator : MonoBehaviour {

	public bool fallblock;
	public float timeleft = 1.0f;

	private bool entered = false;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.transform.SetParent (transform);
			entered = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			transform.DetachChildren();
		}
	}

	void Update () {
		if (fallblock && entered) {
			timeleft -= Time.deltaTime;
		}
		if (fallblock && timeleft < 0) {
			GetComponentInParent<Rigidbody> ().isKinematic = false;
		}
	}

	public void setTimeLeft(float left){
		timeleft = left;
		Debug.Log (left);
	}
}




































