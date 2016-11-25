using UnityEngine;
using System.Collections;

public class TriggerActivator : MonoBehaviour {

	public bool fallblock;
	public float timeleft = 1.0f;

	private bool entered = false;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.transform.SetParent (transform.parent.parent);
			entered = true;
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
}




































